using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

/* NOTES ON SPAWN AREAS
now and then it'll respawn. this gives the teams a chance to recontest it to regain vision of the area and the buff (or lose it)
i think the vision will be just some radius, or a rectangle or something near the area. if you have 4 adjacent areas,
maybe it all overlaps and you have full vision?
or we could make it so that the vision doesn't overlap. either way that's for the future to consider
the team with the most camps up at the current time gets the spawnpoint in the middle of the map and can choose 
if they want to respawn there
also i think one of the objectives or the spawn point can allow you to return your own flag to your base. 
so you don't only have the option to run to your base to "safe" it
*/
