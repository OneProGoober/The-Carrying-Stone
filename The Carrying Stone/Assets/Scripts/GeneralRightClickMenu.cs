using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Used as the generic general pickup/interactivity menu. Not the one used for the inventory.
 * */
public class GeneralRightClickMenu : MonoBehaviour
{
    private Transform dropDownMenuOptions;
    public Button dropdownChild;
    public bool dropdownSet = false;
    public GameObject dropdown;
    public GameObject dropdownBackground;
    public GameObject actualBackground;
    public List<Button> rightClickedButtonList;
    public GeneralRightClickMenuBackground rightClickBackgroundMenuScript;
    //public List<GameObject> validClicksFinalList;
    public List<GameObject> validRightClicks;
    public List<GameObject> items;
    public List<GameObject> players;
    public List<GameObject> obstructions;
    public GameObject carryingOne;
    public GameObject carryingTwo;
    public List<GameObject> allGO;

    float height = 0.0f;

    void Start()
    {
        //validClicksFinalList = dropdownBackground.GetComponent<GeneralRightClickMenuBackground>().validClicksFinalList;
        dropDownMenuOptions = dropdown.GetComponent<Transform>();

        //Make the lists for seperate gameobjects that are clicked.
        rightClickedButtonList = new List<Button>();
        validRightClicks = new List<GameObject>();
        items = new List<GameObject>();
        players = new List<GameObject>();
        obstructions = new List<GameObject>();
        allGO = new List<GameObject>();
        gameObject.SetActive(false);
    }

    public void DeleteButtons()
    {
        //get rid of the actual buttons on the menu
        for (int i = 0; i < rightClickedButtonList.Count; i++)
        {
            Button temp = rightClickedButtonList[i];
            Destroy(temp.gameObject);
        }
        //clear the list that tracks the buttons
        rightClickedButtonList.Clear();
        
        validRightClicks.Clear();
        players.Clear();
        items.Clear();
        obstructions.Clear();
        allGO.Clear();
        carryingOne = null;
        carryingTwo = null;
    }
    
    public void Dropdown(LayerMask itemsPlayersObstructionsMask)
    {
        //Clear the previous clicked items for a new fresh list
        DeleteButtons();
        gameObject.SetActive(true);
        actualBackground.SetActive(true);
        //Raycast all to get all gameobjects (items) (excluding the ground)
        RaycastHit[] rightClicked;
        rightClicked = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, itemsPlayersObstructionsMask);

        CheckRightClicks(rightClicked);

        //set both backgrounds to the same size
        RectTransform clickBackgroundRect = dropdownBackground.GetComponent<RectTransform>();
        RectTransform actualRect = actualBackground.GetComponent<RectTransform>();
        clickBackgroundRect.transform.position = Input.mousePosition;
        actualRect.transform.position = Input.mousePosition;
        clickBackgroundRect.sizeDelta = new Vector2(100, (rightClickedButtonList.Count) * height);
        actualRect.sizeDelta = clickBackgroundRect.sizeDelta;
    }

    /**
     * Adds obstructions, players and items that are clicked.
     * */
    public void CheckRightClicks(RaycastHit[] rightClicked)
    {
        //Seperate the objects in the right click menu.
        foreach (var hitClicked in rightClicked)
        {
            GameObject clickedObject = hitClicked.collider.gameObject;
            if (clickedObject.tag.Equals("Item"))
            {
                items.Add(clickedObject);
                allGO.Add(clickedObject);
            }
            else if (clickedObject.tag.Equals("Player"))
            {
                players.Add(clickedObject);
                allGO.Add(clickedObject);
            }
            else if (clickedObject.tag.Equals("Obstruction"))
            {
                obstructions.Add(clickedObject);
                allGO.Add(clickedObject);
            }
            else if(clickedObject.tag.Equals("CarryingOne"))
            {
                carryingOne = clickedObject;
                allGO.Add(clickedObject);
            }
            else if (clickedObject.tag.Equals("CarryingTwo"))
            {
                carryingOne = clickedObject;
                allGO.Add(clickedObject);
            }
            else
            {
                Debug.Log("Don't add: " + clickedObject + "?");
            }
        }

        //add them in the order desired:
        foreach (var obstruction in obstructions)
        {
            string buttonName = "Obstruction";
            Button obsButton = MakeButton(buttonName);
            rightClickedButtonList.Add(obsButton);
        }
        foreach (var player in players)
        {
            string buttonName = player.GetComponent<Player>().playerName;
            Button playerButton = MakeButton(buttonName);
            rightClickedButtonList.Add(playerButton);
        }
        foreach (var item in items)
        {
            string buttonName = item.GetComponentInParent<Item>().itemName;
            Button itemButton = MakeButton(buttonName);
            rightClickedButtonList.Add(itemButton);
        }
        if(carryingOne != null)
        {
            string buttonName = carryingOne.name;
            Button carryingButton = MakeButton(buttonName);
            rightClickedButtonList.Add(carryingButton);
        }
        if (carryingTwo != null)
        {
            string buttonName = carryingTwo.name;
            Button carryingButton = MakeButton(buttonName);
            rightClickedButtonList.Add(carryingButton);
        }
    }

    public Button MakeButton(string buttonName)
    {
        Button temp = Instantiate(dropdownChild);
        height = temp.GetComponent<RectTransform>().rect.height;
        temp.transform.SetParent(dropDownMenuOptions);
        temp.transform.localScale = dropDownMenuOptions.localScale;
        temp.name = buttonName;
        temp.gameObject.GetComponentInChildren<Text>().text = buttonName;
        dropDownMenuOptions.transform.position = Input.mousePosition;
        return temp;
    }
}
