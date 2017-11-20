using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public string side;
    public string playerName;
    //private TeamManager tm;

    void Start()
    {
        //tm = FindObjectOfType<TeamManager>();
        side = LearnTeamAlliance(this);
    }

    /**
     * this is for non-static items. Istrigger on the collider is not checked.
     * */
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            //inventory.AddItem(other.GetComponent<Item>());
        }
    }

    /**
     * this is for static items (like the carrying stones). Istrigger on the collider is checked.
     * */
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Item")
    //    {
    //        inventory.AddItem(collision.gameObject.GetComponent<Item>());
    //        Destroy(collision.gameObject); //Note - Items with "is Trigger" will persist upon pickup!
    //    }

    //    if(collision.gameObject.tag == "CarryingOne")
    //    {
    //        if (side.Equals("TeamOne"))
    //        {
    //            Debug.Log("We'll need to change this so the stone returns to the base and doesn't go to the player's inventory.");                
    //            //tm.Return(teamAlliance, collision.gameObject);
    //            //Destroy(collision.gameObject);
    //        }

    //        if (side.Equals("TeamTwo"))
    //        {
    //            Debug.Log("Here????");
    //       @@@@@@@@@@@@@@@@@ClickToPickUpItem
    //            inventory.AddItem(collision.gameObject.GetComponent<Item>());
    //            tm.Capture(tm.teamTwo, collision.gameObject);
    //            Destroy(collision.gameObject);
    //        }
    //    }

    //    if(collision.gameObject.tag == "CarryingTwo")
    //    {
    //        if (side.Equals("TeamOne"))
    //        {
    //            //inventory.AddItem(collision.gameObject.GetComponent<Item>());
    //            //tm.Capture(tm.teamOne, collision.gameObject);
    //            //Destroy(collision.gameObject);
    //        }

    //        if (side.Equals("TeamTwo"))
    //        {
    //            Debug.Log("We'll need to change this so the stone returns to the base and doesn't go to the player's inventory.");
    //            //tm.Return(teamAlliance, collision.gameObject);
    //            //Destroy(collision.gameObject);
    //        }
    //    }
    //}

    /**
     * player's team is assigned in the TeamManager Script
     * */
    private string LearnTeamAlliance(Player player)
    {
        return player.GetComponentInParent<Team>().team;
    }
}
