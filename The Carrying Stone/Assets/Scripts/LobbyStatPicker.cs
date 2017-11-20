using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyStatPicker : MonoBehaviour
{
    //tutorial that resembled a similar UI to what I want: https://www.youtube.com/watch?v=P66SSOzCqFU&t=1517s
    //another potentially usedful tutorial: https://www.youtube.com/watch?v=T-AbCUuLViA
    private RectTransform abilityMenuRect;
    private List<GameObject> abilityArray;
    private float abilityWindowWidth;
    private float abilityWindowHeight;
    private int abilityCount;

    public GameObject AbilityOne;
    public GameObject AbilityTwo;
    public Canvas canvas;
    public EventSystem eventSystem;

    public float abilityPaddingLeft;
    public float abilityPaddingTop;
    public float abilityHeight;
    public float abilityWidth;

    void Start()
    {
        SetUpAbilityChoices();
    }
    
    private void SetUpAbilityChoices()
    {
        //This part of the code just keeps track of the gameobjects themselves. The actual stats are always the same size in the UI
        abilityArray = new List<GameObject>
        {
            AbilityOne,
            AbilityTwo
        };

        abilityCount = abilityArray.Count;
        abilityWindowWidth = abilityWidth + (2 * abilityPaddingLeft);
        abilityWindowHeight = abilityCount * (abilityHeight + abilityPaddingTop) + abilityPaddingTop;

        abilityMenuRect = GetComponent<RectTransform>();
        abilityMenuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, abilityWindowWidth);
        abilityMenuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, abilityWindowHeight);

        for (int i = 0; i < abilityCount; i++)
        {
            GameObject newStat = (GameObject)Instantiate(abilityArray[i]);
            RectTransform rect = newStat.GetComponent<RectTransform>();

            //newStat.name = "Stat " + i;
            newStat.transform.SetParent(this.transform.parent);

            rect.localPosition = abilityMenuRect.localPosition + new Vector3(-(abilityPaddingLeft + abilityWidth), -abilityPaddingTop * (i + 1) - (abilityHeight * i));
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, abilityWidth * canvas.scaleFactor);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, abilityHeight * canvas.scaleFactor);
            newStat.transform.SetParent(this.transform);
        }
    }
}