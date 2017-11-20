using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private RectTransform inventoryRect;
    public List<GameObject> allInventorySlots;
    public List<GameObject> backgroundSlots;
    public string currentUsedItem = null;
    public int currentUsedReference = -1;
    public Slot currentUsedSlot;
    private static GameObject hoverObject;
    private float inventoryWidth;
    private float inventoryHeight;
    private float hoverOffset;

    private bool fadeIn;
    private bool fadeOut;

    public GameObject dropItem;
    public float fadeTime;
    public GameObject slotPrefab;
    public GameObject iconPrefab;
    public Canvas canvas;
    public EventSystem eventSystem;
    public int slots;
    public int rows;
    public float slotPaddingLeft;
    public float slotPaddingTop;
    public float slotSize;

    public GameObject toolTipObject;
    public Text sizeTextObject;
    public Text visualTextObject;
    public static int emptySlotCount;

    private static GameObject playerRef;
    private static Slot from;
    private static Slot to;
    private static GameObject toolTip;
    private static CanvasGroup canvasGroup;
    private static Text visualText;
    private static Text sizeText;

    public GameObject SlotBackground;

    public static int EmptySlotCount
    {
        get
        {
            return emptySlotCount;
        }

        set
        {
            emptySlotCount = value;
        }
    }

    public void SetCurrentUsed(string itemName, int itemreference, Slot slot)
    {
        currentUsedItem = itemName;
        currentUsedReference = itemreference;
        currentUsedSlot = slot;
    }

    public static CanvasGroup CanvasGroup
    {
        get
        {
            return Inventory.canvasGroup;
        }
    }

    void Start()
    {
        toolTip = toolTipObject;
        sizeText = sizeTextObject;
        visualText = visualTextObject;
        playerRef = GameObject.Find("Player");
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        CreateInventoryLayout();
        currentUsedItem = null;
    }

    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
        //    {
        //        from.GetComponent<Image>().color = Color.white;

        //        foreach (Item item in from.Items)
        //        {
        //            float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
        //            Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        //            v *= 3;

        //            dropItem = item.prefab;
        //            Debug.Log("Drop right under us");
        //            GameObject temporaryDrop = (GameObject)GameObject.Instantiate(dropItem, playerRef.transform.position, Quaternion.identity);
        //            temporaryDrop.GetComponent<Item>().SetItemStatsOnDrop(item);
        //        }

        //        from.ClearSlot();
        //        Destroy(GameObject.Find("Hover"));
        //        to = null;
        //        from = null;
        //        EmptySlotCount++;
        //    }
        //}

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (CanvasGroup.alpha > 0)
            {
                StartCoroutine("FadeOut");
                //PutItemBack();
            }
            else
            {
                StartCoroutine("FadeIn");
            }
        }

        if (Input.GetKey(KeyCode.M))
        {
            if (eventSystem.IsPointerOverGameObject(-1))
            {
                MoveInventory();
            }
        }
    }

    public void ShowToolTip(GameObject slot)
    {
        Slot tempSlot = slot.GetComponent<Slot>();
        if (!tempSlot.IsEmpty && hoverObject == null)
        {
            visualText.text = tempSlot.currentItem.GetToolTip();
            sizeText.text = visualText.text;
            toolTip.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            float tooltipWidth = toolTip.GetComponent<RectTransform>().rect.width;
            float tooltipHeight = toolTip.GetComponent<RectTransform>().rect.height;

            if (xPos + tooltipWidth > Screen.width)
            {
                xPos = Screen.width - tooltipWidth;
            }

            if (yPos - tooltipHeight < 0)
            {
                yPos = tooltipHeight;
            }

            toolTip.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    private void MoveInventory()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            new Vector3(Input.mousePosition.x - (inventoryRect.sizeDelta.x / 2 * canvas.scaleFactor),
            Input.mousePosition.y + (inventoryRect.sizeDelta.y / 2 * canvas.scaleFactor)), canvas.worldCamera, out mousePosition);

        transform.position = canvas.transform.TransformPoint(mousePosition);
        int i = 0;
        foreach (var slot in backgroundSlots)
        {
            slot.transform.position = allInventorySlots[i].transform.position;
            i++;
        }
    }

    private void CreateInventoryLayout()
    {
        allInventorySlots = new List<GameObject>();
        backgroundSlots = new List<GameObject>();
        EmptySlotCount = slots;
        hoverOffset = slotSize * .01f;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject slotBackground = (GameObject)Instantiate(SlotBackground);
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                newSlot.name = "slot" + " x:" + x + " y: "+ y;
                slotBackground.name = "slot" + " x:" + x + " y: " + y;

                RectTransform rect = newSlot.GetComponent<RectTransform>();
                rect.SetParent(this.transform.parent);
                rect.SetAsLastSibling();

                RectTransform backgroundRect = slotBackground.GetComponent<RectTransform>();
                backgroundRect.SetParent(this.transform.parent);
                backgroundRect.SetAsFirstSibling();

                rect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                rect.SetParent(this.transform);

                backgroundRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                backgroundRect.SetParent(this.transform);

                allInventorySlots.Add(newSlot);
                backgroundSlots.Add(slotBackground);
                
            }
        }
        transform.SetAsFirstSibling();
    }

    /**
     *  Everything eventually goes through "addItemPrfeab" when adding items to the slot.
     * */
    public bool ClickToPickUpItem(Item item, int sceneItemIndex)
    {
        //Debug.Log("Click to pickup item index: " + item.GetComponent<Item>().sceneItemIndex);
        item.GetComponent<Item>().sceneItemIndex = sceneItemIndex;
        //Debug.Log("Click to pickup item index modified: " + item.GetComponent<Item>().sceneItemIndex);
        if (item.maxStackSize == 1 && emptySlotCount == 0)
        {
            Debug.Log("Could not pick up " + item.itemName + ", inventory is too full.");
            return false;
        }
        else if (item.maxStackSize == 1 && emptySlotCount > 0)
        {
            Debug.Log("Picked up " + item.itemName + ".");
            PlaceEmpty(item);
            return true;
        }

        else
        {
            foreach (GameObject slot in allInventorySlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if (!temp.IsEmpty)
                {
                    if (temp.currentItem.type == item.type && temp.canAdd)
                    {
                        //add the item prefab, this is filling an empty slot with a "brand new" item
                        temp.AddItemPrefab(item);
                        return true;
                    }
                }
            }

            if (EmptySlotCount > 0)
            {
                Debug.Log("Picked up " + item.itemName + ".");
                PlaceEmpty(item);
            }

            else
            {
                Debug.Log("Could not pick up " + item.itemName + ", inventory is too full.");
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (EmptySlotCount > 0)
        {
            foreach (GameObject slot in allInventorySlots)
            {
                Slot temp = slot.GetComponent<Slot>();

                if (temp.IsEmpty)
                {
                    temp.AddItemPrefab(item);
                    temp.defaultClick = item.defaultClick;
                    temp.clickTypeOptions = item.clickTypeOptions;
                    EmptySlotCount--;
                    return true;
                }
            }
        }

        return false;
    }

    //private void PutItemBack()
    //{
    //    if (from != null)
    //    {
    //        Destroy(GameObject.Find("Hover"));
    //        from.GetComponent<Image>().color = Color.white;
    //        from = null;
    //    }
    //}

    //public void addToUsedArray(Slot slot)
    //{
    //    usedArray.Add(slot);
    //}

    //public void removeFromUsedArray(Slot slot)
    //{
    //    usedArray.Remove(slot);
    //}

    private IEnumerator FadeOut()
    {
        if (!fadeOut)
        {
            fadeOut = true;
            fadeIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = CanvasGroup.alpha;
            float progress = 0.0f;
            float rate = 1.0f / fadeTime;

            while (progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0.0f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            CanvasGroup.alpha = 0.0f;
            fadeOut = false;
        }
    }

    private IEnumerator FadeIn()
    {
        if (!fadeIn)
        {
            fadeOut = false;
            fadeIn = true;
            StopCoroutine("FadeOut");

            float startAlpha = CanvasGroup.alpha;
            float progress = 0.0f;
            float rate = 1.0f / fadeTime;

            while (progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, 1.0f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            CanvasGroup.alpha = 1.0f;
            fadeIn = false;
        }
    }


    /**
     * Deprecated
     * */
    /**public void MoveItem(GameObject clicked)
    {
        //from is of type <Slot>
        if (from == null && canvasGroup.alpha == 1)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                //holding the slot that had an object in it
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }

        //transfering to an empty slot
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        //Results in a swap
        if (to != null && from != null)
        {
            Stack<Item> tempTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tempTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tempTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            Destroy(GameObject.Find("Hover"));
        }
    }
    **/

}
