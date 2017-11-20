using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Stack<Item> items;

    public string itemName;
    public int itemReference;
    public Text stackText;
    public Sprite slotEmpty;
    public Sprite slotHighlight;
    public float scaleFactor;
    public Vector3 startPosition;
    public Graphic itemBeingDragged;
    public ClickType defaultClick;
    public List<ClickType> clickTypeOptions;
    public DropdownMenu dropdownScript;
    private GameObject dropDownMenuBackground;
    public GameObject actualBackground;

    //private Canvas canvas;
    private List<GameObject> backgroundSlots;
    private List<GameObject> inventorySlots;
    private Slot startSlot;
    private EventSystem eventSystem;

    private GameObject dropDownMenu;
    private SpawnAndTrackItems trackItemsScript;
    private Inventory inventory;
    private PlaceableItem placeableItemScript;
    
    GraphicRaycaster uiRaycast;


    public bool IsEmpty
    {
        get { return Items.Count == 0; }
    }

    public Item currentItem
    {
        get { return Items.Peek(); }
    }

    private bool consumable
    {
        get { return currentItem.consumable; }
    }

    public bool canAdd
    {
        get { return Items.Count < currentItem.maxStackSize; }
    }

    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    /**
     * Used to configure the inventory slots
     * */
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        placeableItemScript = FindObjectOfType<PlaceableItem>();
        trackItemsScript = FindObjectOfType<SpawnAndTrackItems>();
        backgroundSlots = inventory.backgroundSlots;
        inventorySlots = inventory.allInventorySlots;
        //canvas = FindObjectOfType<Canvas>();
        Items = new Stack<Item>();
        RectTransform slotRectTrans = GetComponent<RectTransform>();
        RectTransform textRectTrans = stackText.GetComponent<RectTransform>();

        int textScaleFactor = (int)(slotRectTrans.sizeDelta.x * scaleFactor);

        textRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRectTrans.sizeDelta.y);
        textRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRectTrans.sizeDelta.x);

        stackText.resizeTextMaxSize = textScaleFactor;
        stackText.resizeTextMinSize = textScaleFactor;
    }

    /**
     * When clicking the inventory slot, we need to use the default item for the slot or provide a dropdown menu for further options.
     * */
    public void OnPointerClick(PointerEventData eventData)
    {
        dropDownMenu.SetActive(false);
        if (eventData.button == PointerEventData.InputButton.Left && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0.0f)
        {
            UseItemDefault();
        }
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0.0f)
        {
            //Call dropdown Menu
            Debug.Log("Set the dropdown menu background to active");
            dropDownMenuBackground.SetActive(true);
            DropdownMenu();
        }
    }

    public void AddItemPrefab(Item item)
    {
        Items.Push(item);
        //Debug.Log("AddItemPrefab: " + item.GetComponent<Item>().sceneItemIndex);
        this.itemName = item.itemName;
        this.itemReference = item.allGameItemsReferenceIndex;
        if (Items.Count > 1)
        {
            stackText.text = Items.Count.ToString();
        }
     
        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    /**
     * Used to move items around the inventory
     * */
    public void AddItems(Stack<Item> items, List<ClickType> options)
    {
        this.Items = new Stack<Item>(items);
        this.clickTypeOptions = options;
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(currentItem.spriteNeutral, currentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutralSprite, Sprite highlightSprite)
    {
        GetComponent<Image>().sprite = neutralSprite;
        SpriteState state = new SpriteState
        {
            pressedSprite = neutralSprite,
            highlightedSprite = highlightSprite
        };

        GetComponent<Button>().spriteState = state;
    }

    /**
     * Used to get around an odd bug with setActive (we only need to set inactive once, not for every slot. Also, each slot needs the reference).
     * */
    public void PassDropdown(GameObject dropDownObject, DropdownMenu script, GameObject background, GameObject actualBackground)
    {
        this.actualBackground = actualBackground;
        dropDownMenu = dropDownObject;
        dropdownScript = script;
        dropDownMenuBackground = background;
    }

    private void DropdownMenu()
    {
        Debug.Log("Set Active only if the slot isn't empty" );
        if (!IsEmpty)
        {
            dropDownMenu.SetActive(true);
            Vector3 offset = Input.mousePosition;// + new Vector3(-10,10,0);
            dropDownMenu.transform.position = offset;
            dropdownScript.Dropdown(this);
        }
    }

    /**
     * This function takes care of the default functonality for clicking an item
     * */
    private void UseItemDefault()
    {
        //We have an item that has been used and are waiting for input. We handle the input below, now we set the used slot back to white.
        if (inventory.currentUsedSlot != null)
        {
            inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
        }
        /**
         * "If there's no previously clicked item"
         * (We put all defaultClick checks in this segment except for use item)
         */
        if (inventory.currentUsedItem == null)
        {
            startSlot = this;
            startPosition = startSlot.transform.position;
            if (defaultClick == ClickType.CONSUME)
            {
                ConsumeItem();
            }

            if (defaultClick == ClickType.DROP)
            {
                DropItem();
            }
            
            if(defaultClick == ClickType.CONSTRUCT)
            {
                PlaceObstruction();
            }
            //More to come in here.. Like equip, drop, etc.
        }

        /**
         * We can use items on eachother, this is the reason we don't do this check in the above segment.
         * Check that the slot isn't empty since the default click is actually set to USE.
         * */
        if (defaultClick == ClickType.USE && !IsEmpty)
        {
            UseItem();
        }

        else
        {
            inventory.SetCurrentUsed(null, -1,  this);
        }
    }

    private void ConsumeItem()
    {
        if(!IsEmpty && consumable)
        {
            Items.Pop().Consume();
            stackText.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

            if(IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                Inventory.EmptySlotCount++;
            }
        }
    }

    private void UseItem()
    {
        //Check to see if an item has been selected yet. If not, update. If it is then just use the two items.
        
        //enter this if an item hasn't been used yet...
        if (inventory.currentUsedItem == null)
        {
            if (!IsEmpty)
            {
                GetComponent<Image>().color = Color.cyan;
                inventory.SetCurrentUsed(currentItem.itemName, currentItem.allGameItemsReferenceIndex, this);
                //Debug.Log("Current Used Item set and is:" + inventory.currentUsedItem.itemName);
                Debug.Log("Using an item. Click somewhere to allow it to interact.");
            }
            //else   can't actually get into an else here,
            //(this could only be entered by using a slot or right clicking an empty slot, which is disabled).
            //{
            //    Debug.Log("Made it");
            //    //slot is empty, need to set the currentUsedItem to null
            //    inventory.SetCurrentUsed(null, null);
            //}

        }

        //an item has been used in the past... enter the else.
        else
        {
            if(!IsEmpty)
            {
                //currentItem.Use(inventory.currentUsedItem);
                currentItem.Use(inventory.currentUsedItem);
                //Debug.Log("Current Item: " + currentItem.itemName + " current used item: " + inventory.currentUsedItem);
                Debug.Log("We just used two items on eachother");
                
            }
                GetComponent<Image>().color = Color.white;
                inventory.SetCurrentUsed(null, -1, this);
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackText.text = string.Empty;
    }

    public void OnDrag(PointerEventData eventData)
    {
        FindObjectOfType<Inventory>().toolTipObject.SetActive(false);
        if (!startSlot.IsEmpty)
        {
            Vector3 hoverOffset = new Vector3(5.0f, 5.0f, 0.0f);
            Vector3 posOffset = Input.mousePosition + hoverOffset;
            transform.position = posOffset;
            transform.SetAsLastSibling();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //If an item is currently used, make sure we set that slot back to white.
        if (inventory.currentUsedSlot != null)
        {
            inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
        }

        //inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
        inventory.SetCurrentUsed(null, -1, this);
        startSlot = this;
        startPosition = startSlot.transform.position;
        itemBeingDragged = gameObject.GetComponent<Button>().targetGraphic;
        LoopBackGroundSlots(true);
        dropDownMenu.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
        itemBeingDragged = null;
        LoopBackGroundSlots(false);

        //Debug.Log(eventData.hovered.ToString());
        //Debug.Log(eventData.hovered.Count);

        bool moved = false;
        foreach (var gameObject in eventData.hovered)
        {
            Debug.Log(gameObject.tag);
            if(gameObject.tag.Equals("Slot"))
            {
                //Debug.Log("Move");
                MoveItem(eventData, gameObject);
                moved = true;
            }
            else if(gameObject.tag.Equals("Inventory"))
            {
                //Debug.Log("Here");
                startSlot.transform.position = startPosition;
                moved = true;
            }
        }

        if(!moved)
        {
            //Debug.Log("Drop item");
            DropItem();
        }
    }

    public void MoveItem(PointerEventData eventData, GameObject slot)
    {
        Slot invySlot = LoopInventorySlots(slot).GetComponent<Slot>();
        /**
            * Same slot: no need, handled in the caller method
            * */
        //if (invySlot == startSlot)
        //{
        //    transform.position = startPosition;
        //}

        /**
            * From is empty
            * To is empty
            * => DO NOTHING
            * */
        if (startSlot.IsEmpty && invySlot.IsEmpty)
        {
            startSlot.transform.position = startPosition;
        }

        /**
            * From is empty
            * To is occupied
            * => DO NOTHING
            * */
        else if (startSlot.IsEmpty && !invySlot.IsEmpty)
        {
            startSlot.transform.position = startPosition;
        }

        /**
            * From is occupied
            * To is empty
            * => ACTUALLY MOVE THE ITEM!!!
            * */
        else if (!startSlot.IsEmpty && invySlot.IsEmpty)
        {
            Stack<Item> tempTo = new Stack<Item>(startSlot.Items);

            List<ClickType> clickOptions = startSlot.clickTypeOptions;
            startSlot.ClearSlot();
            startSlot.clickTypeOptions = new List<ClickType>();
            if (tempTo.Count == 0)
            {
                startSlot.ClearSlot();
            }
            else
            {
                invySlot.AddItems(tempTo, clickOptions);
            }
            invySlot.defaultClick = startSlot.defaultClick;
            invySlot.GetComponent<Image>().color = Color.white;
            startSlot.transform.position = startPosition;
            startSlot = null;
            invySlot = null;
        }


        /**
            * From is occupied
            * To is occupied
            * =>swap
            * */
        else if (!startSlot.IsEmpty && !invySlot.IsEmpty)
        {
            Stack<Item> tempTo = new Stack<Item>(startSlot.Items);

            List<ClickType> clickOptions = startSlot.clickTypeOptions;
            startSlot.AddItems(invySlot.Items, invySlot.clickTypeOptions);

            if (tempTo.Count == 0)
            {
                startSlot.ClearSlot();
            }
            else
            {
                invySlot.AddItems(tempTo, clickOptions);
            }
            ClickType temp = invySlot.defaultClick;
            invySlot.defaultClick = startSlot.defaultClick;
            startSlot.defaultClick = temp;
            invySlot.GetComponent<Image>().color = Color.white;
            startSlot.transform.position = startPosition;
            startSlot = null;
            invySlot = null;
        }

        else
        {
            Debug.Log("Shouldn't ever be in here!");
        }

    }

    public void LoopBackGroundSlots(bool boolean)
    {
        string index = this.name;
        foreach (var slot in backgroundSlots)
        {
            if (slot.name.Equals(index))
            {
                slot.SetActive(boolean);
                break;
            }
        }
    }

    public Slot LoopInventorySlots(GameObject destination)
    {
        string index = destination.name;
        Slot slotMovedTo = null;
        foreach (var slot in inventorySlots)
        {
            if (slot.name.Equals(index))
            {
                slotMovedTo = slot.GetComponent<Slot>();
            }
        }
        return slotMovedTo;
    }

    public void DropItem()
    {
        //Drops an item directly below the player
        startSlot.GetComponent<Image>().color = Color.white;

        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = playerRef.transform.position;
        foreach (Item item in startSlot.Items)
        {
            Vector3 dropPosition = new Vector3(pos.x, item.transform.position.y, pos.z);
            GameObject temporaryDrop = (GameObject)Instantiate(item.prefab, dropPosition, Quaternion.identity);
            temporaryDrop.GetComponent<Item>().SetItemStatsOnDrop(item);
            trackItemsScript.itemsOnGround.Add(temporaryDrop);
            trackItemsScript.itemPositions.Add(temporaryDrop.transform.position);

        }

        startSlot.transform.position = startPosition;
        startSlot.ClearSlot();
        Inventory.EmptySlotCount++;
    }

    /**
     * VERY similar to dropItem
     * */
    public void PlaceObstruction()
    {
        //Drops an item directly below the player
        startSlot.GetComponent<Image>().color = Color.white;

        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = playerRef.transform.position;

        //Place one obstruction
        //Add it to the obstruction list as well
        Vector3 dropPosition = new Vector3(pos.x, placeableItemScript.obstruction.transform.position.y, pos.z);
        GameObject obstruction = (GameObject)Instantiate(placeableItemScript.obstruction, dropPosition, Quaternion.identity);
        //trackItemsScript.obstructionObjects.Add(obstruction);
        //trackItemsScript.obstructionLocations.Add(obstruction.transform.position);

        startSlot.transform.position = startPosition;
        startSlot.ClearSlot();
        Inventory.EmptySlotCount++;
    }


    public void DropDownMenuClick(string clickString)
    {
        switch (clickString)
        {
            case "USE":
                if(inventory.currentUsedItem == null)
                {
                    inventory.SetCurrentUsed(null, -1, this);
                    //inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
                    UseItem();
                    dropDownMenu.SetActive(false);
                }
                else
                {
                    UseActiveAndClickedOnDropdownMenu();
                }
            break;

            case "CONSUME":
                if (inventory.currentUsedItem == null)
                {
                    dropDownMenu.SetActive(false);
                    ConsumeItem();
                }
                else
                {
                    UseActiveAndClickedOnDropdownMenu();
                }
                break;

            case "DROP":
                if (inventory.currentUsedItem == null)
                {
                    startSlot = this;
                    startPosition = startSlot.transform.position;
                    dropDownMenu.SetActive(false);
                    DropItem();
                }
                else
                {
                    UseActiveAndClickedOnDropdownMenu();
                }
                break;

            case "CONSTRUCT":
                if (inventory.currentUsedItem == null)
                {
                    startSlot = this;
                    startPosition = startSlot.transform.position;
                    dropDownMenu.SetActive(false);
                    PlaceObstruction();
                }
                else
                {
                    UseActiveAndClickedOnDropdownMenu();
                }
                break;
        }
    }

    private void UseActiveAndClickedOnDropdownMenu()
    {
        //disables the current use functionality.
        inventory.currentUsedSlot.GetComponent<Image>().color = Color.white;
        inventory.SetCurrentUsed(null, -1, null);
        dropDownMenu.SetActive(false);
    }
}
