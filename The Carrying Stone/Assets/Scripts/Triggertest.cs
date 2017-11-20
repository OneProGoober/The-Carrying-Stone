using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Triggertest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Test");
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
