using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClassTest : MonoBehaviour {

    public GameObject go;
    public Character x;
    public int expAmount;

    // Use this for initialization
    void Start () {
        x = go.GetComponent<Character>();
    }
	
	// Update is called once per frame
	void Update () {
        AddExp();
	}

    public void AddExp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            x.gainExp(expAmount);
            Debug.Log("Gained " + expAmount + "Exp");
        }
    }
}
