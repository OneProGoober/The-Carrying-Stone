using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/**
 * Used as the background for the right click in inventory menu.
 **/
public class OnMouseOverBackGround : MonoBehaviour, IPointerExitHandler, IPointerClickHandler //, IPointerEnterHandler
{
    public GameObject rightClickDisplay;
    public Button clickedButton;

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exited");
        rightClickDisplay.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        List<GameObject> gameObjectsClicked = new List<GameObject>();

        foreach (var go in raycastResults)
        {
            gameObjectsClicked.Add(go.gameObject);
        }

        Button[] clickTypeOptions;
        clickTypeOptions = rightClickDisplay.GetComponentsInChildren<Button>();
        Slot s = GameObject.FindObjectOfType<DropdownMenu>().rightClickedSlot;

        foreach (var button in clickTypeOptions)
        {
            if(gameObjectsClicked.Contains(button.gameObject) && button.name != s.itemName)
            {
                //Debug.Log("Clicked a button: "+ button.GetComponentInChildren<Text>().text);
                s.DropDownMenuClick(button.name);
                gameObject.SetActive(false);
                
            }
        }
    }
}
