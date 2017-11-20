using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public SpawnAndTrackItems spawnAndTrackItemsScript;
    public int sceneItemIndexReference;
    
    public IEnumerator SpawnItem(Item item, float spawnTime)
    {
        Debug.Log("Waiting for respawn timer");
        yield return new WaitForSeconds(spawnTime);
        Debug.Log("Respawned " + item.itemName + "!");
        //spawn the new item based on the spawnItemsScript and add them to the script's lists to keep track of the items on the ground.
        GameObject itemRef = (GameObject)Instantiate(spawnAndTrackItemsScript.sceneItems[item.sceneItemIndex], spawnAndTrackItemsScript.sceneItems[item.sceneItemIndex].transform.position, Quaternion.identity);
        spawnAndTrackItemsScript.itemsOnGround.Add(itemRef);
        spawnAndTrackItemsScript.itemPositions.Add(itemRef.transform.position);
    }
}
