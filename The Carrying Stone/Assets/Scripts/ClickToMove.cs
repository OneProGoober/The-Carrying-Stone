using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject currentItem = null;
    private static GameObject itemMouseover;
    private static Text sizeText;
    private static Text visualText;
    Vector3 itemClickedDestination;
    public Item itemClicked;
    Transform playerTransform;
    Vector3 mouseOverOffset;
    public Texture2D clickItemCursor;
    public Texture2D clickDefaultCursor;
    public Texture2D clickInventoryCursor;
    private Inventory inventory;
    public GameObject mouseover;
    public Text sizeTextObject;
    public Text visualTextObject;
    LayerMask itemLayerMask;
    LayerMask players;
    LayerMask groundLayerMask;
    LayerMask obstructions;
    LayerMask itemsGroundObstructions;
    LayerMask itemsPlayersObstructionsMask;
    SpawnAndTrackItems trackItemsScript;
    public GeneralRightClickMenu itemDropdownMenuScript;
    public MoveAwayFromObstruction moveAwayScript;
    public bool rightClickMenu;
    private RaycastHit rightClickMoveTo;

    public SpawnScript spawnScriptReference;

    public bool hitObstruction;
    Vector3 playerToObstruction;
    Vector3 playerToMouseClick;

    public Vector3 collisionPoint;
    public bool set;

    public Stack<Vector3> storedLocations;
    public bool touching;


    Obstruction obstruction;
    int usedOnObstructionReference;
    public Vector3 obstructionPosition;
    public bool interactedOnce = false;



    //right click is 1
    //left click is 0
    //middle button is 2
    void Start()
    {
        rightClickMenu = false;
        //itemDropdownMenuScript = GetComponent<ItemDropdownMenu>();

        storedLocations = new Stack<Vector3>();

        sizeText = sizeTextObject;
        visualText = visualTextObject;
        playerTransform = GetComponentInParent<Transform>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        inventory = FindObjectOfType<Inventory>();
        moveAwayScript = FindObjectOfType<MoveAwayFromObstruction>();
        trackItemsScript = FindObjectOfType<SpawnAndTrackItems>();
        spawnScriptReference = trackItemsScript.gameObject.GetComponent<SpawnScript>();
        itemMouseover = mouseover;
        Rect rt = itemMouseover.GetComponent<RectTransform>().rect;
        mouseOverOffset = new Vector3(-6 + -rt.width, 5 + rt.height, 0);
        players = 1 << LayerMask.NameToLayer("Player");
        itemLayerMask = 1 << LayerMask.NameToLayer("Item");
        groundLayerMask = 1 << LayerMask.NameToLayer("Level");
        obstructions = 1 << LayerMask.NameToLayer("Obstruction");
        itemsGroundObstructions = itemLayerMask | groundLayerMask | obstructions;
        itemsPlayersObstructionsMask = itemLayerMask | obstructions | players;
    }

    void Update()
    {
        storedLocations.Push(this.gameObject.transform.position); //save the positions of the player. Used in Obstruction Class to set correct collision point.

        if (rightClickMenu == true)
        {
            if(agent.enabled)
            {
                agent.SetDestination(itemClickedDestination);
            }
            else
            {
                agent.enabled = true;
                agent.SetDestination(itemClickedDestination);
            }
            rightClickMenu = false;
        }
        //Debug.Log("itemclicked: " + itemClicked);

        if(obstruction != null && touching && !interactedOnce && (obstructionPosition == obstruction.transform.position))
        {
            Debug.Log("Touching desired obstruction, will now attempt to interact with it");
            obstruction.CheckItemUsed(usedOnObstructionReference);
            interactedOnce = true;
        }
        
        /**
         * Pick up the item if it was the last thing that was clicked.
         * */
        if (itemClicked && agent.transform.position == itemClickedDestination || itemClicked && NextToItemUnderObstruction())//  || agent.transform.position == rightClickMoveTo.point)
        {
            //get correct index
            int index = itemClicked.sceneItemIndex;
            //Debug.Log("index is: "+ index);
            //Remove the item from the list and the position
            for (int idx = 0; idx < trackItemsScript.itemsOnGround.Count; idx++)
            {
                if (trackItemsScript.itemsOnGround[idx].GetComponent<Item>() == itemClicked && trackItemsScript.itemPositions[idx] == itemClicked.transform.position)
                {
                    if (itemClicked.spawn)
                    {
                        //Debug.Log("Item can respawn: " + trackItemsScript.sceneItems[index].gameObject.GetComponent<Item>());
                        StartCoroutine(spawnScriptReference.SpawnItem(trackItemsScript.sceneItems[index].gameObject.GetComponent<Item>(), trackItemsScript.sceneItems[index].gameObject.GetComponent<Item>().spawnTime));

                        int nonRespawnIndex = trackItemsScript.sceneItems[index].gameObject.GetComponent<Item>().allGameItemsReferenceIndex;
                        //Debug.Log(nonRespawnIndex+"   "+ trackItemsScript.allGameItemsReferenceList[nonRespawnIndex]);

                        inventory.ClickToPickUpItem(trackItemsScript.allGameItemsReferenceList[nonRespawnIndex].gameObject.GetComponent<Item>(), index);

                    }
                    else
                    {
                        //this next line here is very important, otherwise we keep adding more spawns into the game when dropping regular items.
                        index = itemClicked.allGameItemsReferenceIndex;

                        //Debug.Log("Item can't respawn: " + index);
                        inventory.ClickToPickUpItem(trackItemsScript.allGameItemsReferenceList[index].gameObject.GetComponent<Item>(), index);
                    }
                    GameObject item = trackItemsScript.itemsOnGround[idx];


                    //Debug.Log("ClickToMove remove items from the list index:" + item.GetComponent<Item>().sceneItemIndex);
                    trackItemsScript.itemPositions.RemoveAt(idx);
                    trackItemsScript.itemsOnGround.RemoveAt(idx);
                    Destroy(item);
                }
            }
        }

        /**
         * This code is executed whenever we're not hovered over the inventory.
         * */
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Raycast used to check for items on the ground and to register player movement.
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, itemsGroundObstructions))
            {
                //Ray intersecting with collider (Only find colliders in the mask passed through)
                CheckItemHover(hit);
                CheckGroundClick(hit);
            }

            else
            {
                HideItemMouseOverName();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DisplayMouseClick(clickInventoryCursor));
        }
    }

    /**
     * Allows the player to pick up an item if it's under an obstruction
     * */
    public bool NextToItemUnderObstruction()
    {
        if(touching)
        {
            //make radius of .8 and use overlapsphere
            Collider[] collidersInsideRadius = Physics.OverlapSphere(transform.position, 1.1f);
            foreach (var collider in collidersInsideRadius)
            {
                if (collider.gameObject.Equals(itemClicked.gameObject))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public IEnumerator DisplayMouseClick(Texture2D cursor)
    {
        Cursor.SetCursor(cursor, new Vector2(5, 5), CursorMode.ForceSoftware);
        yield return new WaitForSeconds(0.15f);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ShowItemMouseOverName(RaycastHit hit)
    {
        visualText.text = hit.transform.GetComponent<Item>().GetHover();
        sizeText.text = visualText.text;
        itemMouseover.SetActive(true);
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        itemMouseover.transform.position = new Vector2(xpos, ypos);
    }

    public void ShowObstructionMouseOverName()
    {
        if (inventory.currentUsedItem.Equals("Rebuilder"))
        {
            visualText.text = "Rebuild Obstruction";
        }
        else if (inventory.currentUsedItem.Equals("Decayer"))
        {
            visualText.text = "Decay Obstruction";
        }
        else if (inventory.currentUsedItem.Equals("Destroyer"))
        {
            visualText.text = "Destroy Obstruction";
        }
        else
        {
            visualText.text = "Obstruction";
        }

        sizeText.text = visualText.text;
        itemMouseover.SetActive(true);
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        itemMouseover.transform.position = new Vector2(xpos, ypos);
    }

    public void HideItemMouseOverName()
    {
        itemMouseover.SetActive(false);
    }

    public void CheckItemHover(RaycastHit hit)
    {
        if (hit.transform.tag.Equals("Item") || hit.transform.tag.Equals("CarryingOne") || hit.transform.tag.Equals("CarryingTwo"))
        {
            ShowItemMouseOverName(hit);
            itemMouseover.transform.position = Input.mousePosition - mouseOverOffset;
        }
        else if (hit.transform.tag.Equals("Obstruction") && inventory.currentUsedItem != null)
        {
            ShowObstructionMouseOverName();
            itemMouseover.transform.position = Input.mousePosition - mouseOverOffset;
        }
        else if (itemMouseover.activeInHierarchy)
        {
            HideItemMouseOverName();
        }
    }

    public void CheckGroundClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {            
            //remove the rightclickmenu from the screen when moving
            itemDropdownMenuScript.gameObject.SetActive(false);
            /**
             * set the inventory currentUsedItem to null (This is when using items from the inventory).
             * if the item isn't used on an obstruction, we want movement commands to cancel using an item.
             * otherwise, we want to check if the item used on the obstruction makes sense (and then set the current used to null as well) 
             * */
            if (inventory.currentUsedSlot != null)
            {
                if (hit.collider.tag.Equals("Obstruction"))
                {
                    //Don't call this yet, since it's not implemented yet...
                    obstruction = hit.collider.gameObject.GetComponent<Obstruction>();
                    interactedOnce = false;
                    usedOnObstructionReference = inventory.currentUsedReference;
                    //Debug.Log("itemUsed:" + inventory.currentUsedReference);
                    Debug.Log("Moving to the obstruction...");
                }

                //inventory.currentUsedItem = null;

                inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
                inventory.SetCurrentUsed(null, -1, null); 
                //setting slot to null may have caused problems previously? Might still cause unkown bugs.
                //inventory.currentUsedSlot = null;
                //return;
            }
            else if (hit.collider.gameObject.tag == "Item" || hit.collider.gameObject.tag == "CarryingOne" || hit.collider.gameObject.tag == "CarryingTwo")
            {
                itemClicked = hit.collider.gameObject.GetComponent<Item>();
                StartCoroutine(DisplayMouseClick(clickItemCursor));
                Vector3 temp = new Vector3(hit.point.x, playerTransform.position.y, hit.point.z);
                itemClickedDestination = temp;
            }
            else
            {
                itemClicked = null;
                itemClickedDestination = Vector3.negativeInfinity;
                StartCoroutine(DisplayMouseClick(clickDefaultCursor));
            }
            if (agent.enabled == false)
            {
                agent.enabled = true;
            }
            agent.SetDestination(hit.point);
            
        }

        //right click - bring up the menu
        else if (Input.GetMouseButtonDown(1))
        {
            Vector3 temp = new Vector3(hit.point.x, playerTransform.position.y, hit.point.z);
            itemClickedDestination = temp;
            itemDropdownMenuScript.Dropdown(itemsPlayersObstructionsMask);
        }
    }
}
