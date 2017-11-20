using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Used for the inventory dropdown
 * */
public class DropdownMenu : MonoBehaviour
{
    //public GameObject dropDownMenu;
    public RectTransform dropDownMenuOptions;
    //public GameObject mouseOverBackground;
    private int numButtons;
    public Button dropdownChild;
    public List<Button> buttonList;
    public bool dropdownSet = false;
    public GameObject dropdown;
    public Slot rightClickedSlot;
    public GameObject dropdownBackground;
    public GameObject actualBackground;

    void Start()
    {
        if (dropdownSet == false)
        {
            dropdownSet = true;
            List<GameObject> invySlots = FindObjectOfType<Inventory>().allInventorySlots;
            DropdownMenu dropDownScript = FindObjectOfType<DropdownMenu>();
            foreach (var slot in invySlots)
            {
                //gives each slot the correct reference to the dropdown and the script
                slot.GetComponent<Slot>().PassDropdown(dropdown, dropDownScript, dropdownBackground, actualBackground);
            }
            dropdown.SetActive(false);
        }
        buttonList = new List<Button>();
    }

    public void DeleteButtons()
    {
        //Debug.Log("Before delete, count is: "+ buttonList.Count);
        for (int i = 0; i < buttonList.Count; i++)
        {
            Button temp = buttonList[i];
            Destroy(temp.gameObject);
        }
        buttonList.Clear();
        //dropdown.SetActive(false);
        //Debug.Log("Set the dropdownMenu to false" + dropdown.name);
        //Debug.Log("After delete, count is: " + buttonList.Count);
    }


    //string SplitCamelCase(string source)
    //{
    //    string[] splitArray = Regex.Split(source, @"(?<!^)(?=[A-Z])");
    //    string temp = "";
    //    string finalString = "";
    //    for (int index = 0; index < splitArray.Length; index++)
    //    {
    //        temp = splitArray[index];
    //        //Debug.Log("i is: "+index+ " splitArray is: " +splitArray[index]);
    //        temp = splitArray[index].Substring(0, 1) + splitArray[index].Substring(1).ToLower();
    //        finalString += temp;
    //        Debug.Log("Temp is: " + temp);
    //    }
    //    Debug.Log("FinalString: " + finalString);
    //    return finalString;
    //}

    public void Dropdown(Slot currentSlot)
    {
        rightClickedSlot = currentSlot;
        DeleteButtons();
        FindObjectOfType<Inventory>().toolTipObject.SetActive(false);
        //GameObject sizeTextObject = dropdown.GetComponentInChildren<Text>().gameObject;
        //sizeTextObject.GetComponentInChildren<Text>().text = currentSlot.currentItem.itemName;
        //GameObject.FindGameObjectWithTag("visualText").GetComponent<Text>().text = currentSlot.currentItem.itemName; ;
        numButtons = rightClickedSlot.clickTypeOptions.Count;
        //float width = 0;
        bool firstButton = true;
        float height = 0.0f;
        for (int i = 0; i < numButtons; i++)
        {
            string buttonName = currentSlot.clickTypeOptions[i].ToString();

            //buttonName = SplitCamelCase(buttonName);

            if (firstButton)
            {
                buttonName = currentSlot.currentItem.itemName;
                i--;
            }

            Button temp = Instantiate(dropdownChild);
            height = temp.GetComponent<RectTransform>().rect.height;
            temp.transform.SetParent(dropDownMenuOptions);
            temp.transform.localScale = dropDownMenuOptions.localScale;
            temp.name = buttonName;
            //Capitalize the first letter of the buttonName, don't want to do this to the name of the button because it will be much easier to
            //check cases in the DropDownMenuClick function in the Slot class.
            temp.gameObject.GetComponentInChildren<Text>().text = buttonName.Substring(0,1) + buttonName.Substring(1).ToLower();

            //set the dropdown menu on the MouseOverBackGround Script
            //temp.gameObject.GetComponent<OnMouseOverBackGround>().rightClickDisplay = dropdown;

            //This adds the button functionality.
            //temp.onClick.AddListener(delegate { rightClickedSlot.DropDownMenuClick(buttonName); });
            //Not actually using a button since we're doing a raycast now instead of an OnClick event.
            if (firstButton)
            {
                ColorBlock normalColorBlock = temp.colors;
                normalColorBlock.highlightedColor = normalColorBlock.normalColor;
                normalColorBlock.pressedColor = normalColorBlock.normalColor;
                temp.colors = normalColorBlock;
                temp.gameObject.GetComponentInChildren<Text>().text = buttonName;
                firstButton = false;
            }
            
            //width = temp.transform.GetComponent<RectTransform>().rect.width;
            buttonList.Add(temp);
        }
        //mouseOverBackground.GetComponent<RectTransform>().rect.Set(0, 0, 5, numButtons * buttonHeight);  // height = numButtons * buttonHeight;
        RectTransform clickBackgroundRect = dropdownBackground.GetComponent<RectTransform>();
        RectTransform actualRect = actualBackground.GetComponent<RectTransform>();
        clickBackgroundRect.sizeDelta = new Vector2(100, (numButtons + 1) * height);
        actualRect.sizeDelta = clickBackgroundRect.sizeDelta;
        //rect.Set(dropdownBackground.transform.position.x, 100, dropdownBackground.transform.position.y, numButtons * height);
    }
}
