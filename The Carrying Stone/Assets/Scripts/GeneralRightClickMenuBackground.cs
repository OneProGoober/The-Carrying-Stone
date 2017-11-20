using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Used as the background for the right click menu outside of the inventory
 **/
public class GeneralRightClickMenuBackground : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    public GameObject rightClickMenuOptions;
    public GameObject rightClickDisplay;
    public Button clickedButton;
    public ClickToMove clickToMoveScript;
    public Button[] rightclickedButtons;

    public void OnPointerExit(PointerEventData eventData)
    {
        rightClickDisplay.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        List<GameObject> gameObjectsClicked = new List<GameObject>();
        
        StartCoroutine(clickToMoveScript.DisplayMouseClick(clickToMoveScript.clickItemCursor));

        foreach (var go in raycastResults)
        {
            gameObjectsClicked.Add(go.gameObject);
        }
        rightclickedButtons = rightClickMenuOptions.GetComponentsInChildren<Button>();
        List<GameObject> allInteractables= rightClickDisplay.GetComponent<GeneralRightClickMenu>().allGO;

        for (int i = 0; i < rightclickedButtons.Length; i++)
        {
            if (gameObjectsClicked.Contains(rightclickedButtons[i].gameObject))
            {
                string buttonName = rightclickedButtons[i].name;
                Debug.Log("Do the action: " + buttonName);

                foreach (var interactable in allInteractables)
                {
                    if (interactable.gameObject.tag.Equals("Item") || interactable.gameObject.tag.Equals("CarryingOne") || interactable.gameObject.tag.Equals("CarryingTwo"))
                    {
                        clickToMoveScript.itemClicked = interactable.gameObject.GetComponent<Item>();
                        clickToMoveScript.rightClickMenu = true;
                    }
                    else if (interactable.gameObject.tag.Equals("Obstruction"))
                    {
                        Debug.Log("Right clicked an obstruction, didn't implement what to do yet....");
                    }
                    else if (interactable.gameObject.tag.Equals("Player"))
                    {
                        Debug.Log("Right clicked a player, didn't implement what to do yet....");
                    }
                }
                rightClickDisplay.SetActive(false);
            }
        }
    }
}
