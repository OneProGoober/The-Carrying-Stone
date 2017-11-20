using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stats : MonoBehaviour
{
    private RectTransform statsRect;
    private List<GameObject> statArray;
    private float statWindowWidth;
    private float statWindowHeight;
    private int stats;
    
    public GameObject healthPrefab;
    public GameObject manaPrefab;
    public Canvas canvas;
    public EventSystem eventSystem;

    public float statPaddingLeft;
    public float StatPaddingTop;
    public float statHeight;
    public float statWidth;

    void Start()
    {
        CreateStatsLayout();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (eventSystem.IsPointerOverGameObject(-1))
            {
                MoveStats();
            }
        }
    }
    
    private void MoveStats()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            new Vector3(Input.mousePosition.x + (statsRect.sizeDelta.x / 2 * canvas.scaleFactor),
            Input.mousePosition.y + (statsRect.sizeDelta.y / 2 * canvas.scaleFactor)), canvas.worldCamera, out mousePosition);

        transform.position = canvas.transform.TransformPoint(mousePosition);
    }

    private void CreateStatsLayout()
    {
        //This part of the code just keeps track of the gameobjects themselves. The actual stats are always the same size in the UI
        statArray = new List<GameObject>
        {
            healthPrefab,
            manaPrefab
        };

        stats = statArray.Count;
        statWindowWidth = statWidth + (2 * statPaddingLeft);
        statWindowHeight = stats * (statHeight + StatPaddingTop) + StatPaddingTop;

        statsRect = GetComponent<RectTransform>();
        statsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, statWindowWidth);
        statsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, statWindowHeight);

        for (int i = 0; i < stats; i++)
        {
                GameObject newStat = (GameObject)Instantiate(statArray[i]);
                RectTransform rect = newStat.GetComponent<RectTransform>();
                /**
                 * Temporary names until we figure out what order to do health, mana etc in
                 * */
                newStat.name = statArray[i].name;
                newStat.transform.SetParent(this.transform.parent);

                rect.localPosition = statsRect.localPosition + new Vector3(-(statPaddingLeft + statWidth), -StatPaddingTop * (i + 1) - (statHeight * i));
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, statWidth * canvas.scaleFactor);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, statHeight * canvas.scaleFactor);
                newStat.transform.SetParent(this.transform);
        }
    }
}