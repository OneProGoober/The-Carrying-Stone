using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles the Creation of teams and the capture logic for the Carrying Stone
 * */
public class TeamManager : MonoBehaviour
{
    public Team teamOne;
    public Team teamTwo;
    private static GameObject teamOneStone;
    private static GameObject teamTwoStone;
    private static bool oneTaken;
    private static bool twoTaken;
    public List<Player> teamOneArray = new List<Player>();
    public List<Player> teamTwoArray = new List<Player>();

    void Start()
    {
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        teamOneArray.Add(p);
        //string s = 
        p.GetComponentInParent<Team>().team = "TeamOne";
        //Debug.Log("Set the team for this player to team: " + s);
        /**
         * Eventually useful code...
         * *
        foreach (var player in teamOneArray)
        {

        }
        **/
    }

    /**
     * capturing it from a base
     * */
    public void Capture(Team team, GameObject stone)
    {
        if (team == teamOne && stone.tag.Equals("CarryingTwo"))
        {
            Debug.Log("Captured Team Two Stone!");
            twoTaken = !twoTaken;
        }
        else if (team == teamTwo && stone.tag.Equals("CarryingOne"))
        {
            Debug.Log("Captured Team One Stone!");
            oneTaken = !oneTaken;
        }
    }

    /**
     * picking up the flag to return it to base
     * */
    public void Return(Team team, GameObject stone)
    {
        if (team == teamOne && stone.tag.Equals("CarryingOne"))
        {
            Debug.Log("TeamOne Stone is safe!");
            oneTaken = !oneTaken;
        }
        else if (team == teamTwo && stone.tag.Equals("CarryingTwo"))
        {
            Debug.Log("TeamTwo Stone is safe!");
            twoTaken = !twoTaken;
        }
    }
}