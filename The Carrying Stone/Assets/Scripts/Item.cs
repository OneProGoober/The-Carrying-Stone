using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemType {MANA, HEALTH, CARRYINGSTONE, OBSTRUCTION};
public enum Style {MELEE, MAGE, RANGED, NONCOMBAT};
public enum ClickType {USE, DROP, CONSUME, CONSTRUCT};


public class Item : MonoBehaviour
{
    private ClickToMove clickToMove;

    public string itemName;
    public string description;
    public int sceneItemIndex;
    public ItemType type;
    public Style style;
    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxStackSize;
    //public float attackSpeed;
    //public float attackDamage;
    public bool consumable;
    public GameObject prefab;
    //public int dropDownCount;
    public ClickType defaultClick;
    public List<ClickType> clickTypeOptions;
    public bool itemPrefabInScene = false;

    public bool spawn;
    public float spawnTime;
    public int allGameItemsReferenceIndex;

    void Start()
    {
        clickToMove = FindObjectOfType<Player>().gameObject.GetComponent<ClickToMove>();

    }
    public void Consume()
    {
        switch (itemName)
        {
            case "Health Potion":
                Debug.Log("Used a health potion.");
                break;
            default:
                Debug.Log("Ate Something, but we're not sure what.");
                break;
        }
    }

    public void Use(string itemName)
    {
        if(this.itemName == itemName)
        {
            Debug.Log("You're using " + itemName + " on itself.");
            return;
        }

        switch (itemName)
        {
            case "TeamOneStone":
                Debug.Log("Used TeamOneStone on another item.");
                break;
            case "TeamTwoStone":
                Debug.Log("Used TeamTwoStone on another item.");
                break;
            default:
                Debug.Log("Used some item on some other item.....");
                break;
        }
    }

    public string GetHover()
    {
        return itemName;
    }

    public string GetToolTip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string consumeColor = string.Empty;

        string descriptionNewLine = string.Empty;
        string consumableText = string.Empty;


        if (description != string.Empty)
        {
            descriptionNewLine = "\n";
        }

        if (consumable)
        {
            consumeColor = "green";
            consumableText = "Consumable";
        }
        else
        {
            consumeColor = "red";
            consumableText = "Not Consumable";
        }
        switch (style)
        {
            case Style.MELEE:
                color = "red";
                break;
            case Style.MAGE:
                color = "blue";
                break;
            case Style.RANGED:
                color = "green";
                break;
            case Style.NONCOMBAT:
                color = "white";
                break;
        }

        //if (attackDamage > 0)
        //{
        //    stats += "\n" + attackDamage.ToString() + " attackDamage";
        //}

        //if (attackSpeed > 0)
        //{
        //    stats += "\n" + attackSpeed.ToString() + " attackSpeed";
        //}

        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + descriptionNewLine + "{1}</color></i><color=" + consumeColor + ">\n{2}</color>{3}</size>", itemName, description, consumableText, stats);
    }

    public void SetItemStatsOnDrop(Item item)
    {
        this.itemName = item.itemName;
        this.prefab = item.prefab;
        this.type = item.type;
        this.style = item.style;
        this.spriteNeutral = item.spriteNeutral;
        this.spriteHighlighted = item.spriteHighlighted;
        this.maxStackSize = item.maxStackSize;
        //this.attackSpeed = item.attackSpeed;
        //this.attackDamage = item.attackDamage;
        this.description = item.description;
        this.consumable = item.consumable;
        this.defaultClick = item.defaultClick;
        this.clickTypeOptions = item.clickTypeOptions;
        this.itemPrefabInScene = item.itemPrefabInScene;
        this.allGameItemsReferenceIndex = item.allGameItemsReferenceIndex;
        this.spawn = item.spawn;
        this.spawnTime = item.spawnTime;


        switch (type)
        {
            case ItemType.MANA:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case ItemType.HEALTH:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case ItemType.CARRYINGSTONE:
                GetComponent<Renderer>().material.color = Color.black;
                break;
        }
    }
}
