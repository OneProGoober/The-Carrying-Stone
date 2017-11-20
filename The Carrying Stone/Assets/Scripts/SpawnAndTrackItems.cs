using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndTrackItems : MonoBehaviour
{
    //public Hashtable ItemsOnGround;
    public List<GameObject> sceneItems;

    public List<GameObject> itemsOnGround;
    public List<Vector3> itemPositions;
    public List<GameObject> allGameItemsReferenceList;
    //public List<GameObject> obstructionObjects;
    //public List<Vector3> obstructionLocations;

    void Start ()
	{
        //allGameItemsReferenceList = new List<GameObject>();
        itemsOnGround = new List<GameObject>();
        itemPositions = new List<Vector3>();
        //obstructionObjects = new List<GameObject>();
        //obstructionLocations = new List<Vector3>();
        SpawnSceneItems();
	}

    private void SpawnSceneItems()
    {
        for (int index = 0; index < sceneItems.Count; index++)
        {
            GameObject itemRef = (GameObject)Instantiate(sceneItems[index], sceneItems[index].transform.position, Quaternion.identity);
            itemRef.GetComponent<Item>().sceneItemIndex = index;
            itemsOnGround.Add(itemRef);
            itemPositions.Add(itemRef.transform.position);
        }
    }
}