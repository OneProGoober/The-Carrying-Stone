using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstruction : MonoBehaviour
{
    private ClickToMove clickToMoveScript;
    private SpawnAndTrackItems spawnAndTrackItemsScript;

    private Collider obstructionCollider;
    private Collider[] insideBox;
    private Vector3 colliderHalfExtents;
    private float colliderWidthAndLength;
    private float colliderHeight;

    private List<GameObject> playersInObstructionAtStart;
    private List<GameObject> playersInBox;
    private List<GameObject> playersToRemove;


    private float totalLifetime = 2f;
    private float currentLifeTime;
    private float rebuildGracePeriod = 1f;
    private bool decaying;
    private bool rebuiltRecently;

    private void Start()
    {
        playersInObstructionAtStart = new List<GameObject>();
        playersToRemove = new List<GameObject>();
        spawnAndTrackItemsScript = FindObjectOfType<SpawnAndTrackItems>();
        clickToMoveScript = FindObjectOfType<ClickToMove>();
        playersInBox = new List<GameObject>();


        obstructionCollider = gameObject.GetComponent<Collider>();
        colliderWidthAndLength = obstructionCollider.bounds.size.x;
        colliderHeight = obstructionCollider.bounds.size.y;

        colliderHalfExtents = new Vector3(colliderWidthAndLength, colliderHeight, colliderWidthAndLength) / 2;
        Collider[] insideAtStart = Physics.OverlapBox(transform.position, colliderHalfExtents, Quaternion.identity);

        /**
         * For every player that is in the obstruction on instantiation, add them to a List to allow them to walk through the obstruction temporarily.
         * */
        foreach (var collider in insideAtStart)
        {
            //add one collider per player
            if (collider.gameObject.tag.Equals("Player") && !collider.isTrigger)
            {
                playersInObstructionAtStart.Add(collider.gameObject);
            }
        }
    }

    /**
     * Eventually need to handle using items on obstructions that are under us
     * */
    public void CheckItemUsed(int itemreference)
    {
        GameObject objectUsed = spawnAndTrackItemsScript.allGameItemsReferenceList[itemreference];
        Item itemUsed = objectUsed.GetComponent<Item>();
        if (itemUsed.itemName.Equals("Decayer"))
        {
            if(!decaying && !rebuiltRecently)
            {
                decaying = true;
                StartCoroutine(Decay());
            }
            else
            {
                Debug.Log("Can't decay this obstruction right now...");
            }
        }
        else if (itemUsed.itemName.Equals("Destroyer"))
        {
            if(!decaying)
            {
                Debug.Log("Destroyed");
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("Can't destroy a decaying obstruction");
            }
        }
        else if (itemUsed.itemName.Equals("Rebuilder"))
        {
            Debug.Log("Rebuild!");
            decaying = false;
            StartCoroutine(PreventDestroy());
        }
        else
        {
            Debug.Log("No items have effects other than decayers, destroyers and rebuilders");
        }
    }

    private IEnumerator PreventDestroy()
    {
        rebuiltRecently = true;
        //Debug.Log("Rebuilt recently: " + rebuiltRecently);
        yield return new WaitForSeconds(rebuildGracePeriod);
        if (decaying)
        {
            Debug.Log("No longer decaying");
            decaying = false;
        }
        rebuiltRecently = false;
        //Debug.Log("Rebuilt recently: " + rebuiltRecently);
    }

    private IEnumerator Decay()
    {
        Debug.Log("Decaying");
        currentLifeTime = totalLifetime;
        while (currentLifeTime > 0 && decaying)
        {
            currentLifeTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (!rebuiltRecently && currentLifeTime <= 0)
        {
            GameObject.Destroy(this.gameObject);
            Debug.Log("Decayed");
        }
        else
        {
            Debug.Log("Someone prevented this from fully decaying");
        }
    }

    private void Update()
    {
        /**
         * If any player enters the barrier...
         * */
        playersInBox.Clear();
        playersToRemove.Clear();
        insideBox = Physics.OverlapBox(transform.position, colliderHalfExtents, Quaternion.identity);
        foreach (var collider in insideBox)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                playersInBox.Add(collider.gameObject);
            }
        }

        /**
         * When a player exits the obstruction, we add them to a list to remove
         * */
        foreach (var player in playersInObstructionAtStart)
        {
            if (!playersInBox.Contains(player))
            {
                playersToRemove.Add(player);
            }
        }

        /**
         * remove the players we gathered in the list from the startlist. Eventually this will reach zero, and no players can walk through
         * */
        foreach (var playerToRemove in playersToRemove)
        {
            playersInObstructionAtStart.Remove(playerToRemove);
        }

        /**
         * For every player inside the box currently, check to see if we allow them to walk inside or if we force them to collide
         * */
        foreach (var player in playersInBox)
        {
            if (playersInObstructionAtStart.Contains(player))
            {
                //allow the player to walk through as they please
            }
            else
            {
                if (player.GetComponent<Collider>().bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
                {
                    do
                    {
                        player.transform.position = clickToMoveScript.storedLocations.Pop();
                    } while (player.GetComponent<Collider>().bounds.Intersects(gameObject.GetComponent<Collider>().bounds));
                    
                    SetFinal(player);
                    clickToMoveScript.touching = true;
                    clickToMoveScript.obstructionPosition = gameObject.transform.position;
                    player.GetComponent<NavMeshAgent>().enabled = false;
                }
                else
                {
                    Debug.Log("Not touching the barricade anymore");
                    clickToMoveScript.touching = false;
                }
            }
        }
    }

    public void SetFinal(GameObject player)
    {
        Vector3 obstructionPosition = gameObject.transform.position;
        Vector3 playerPosition = player.transform.position;
        if (Mathf.Abs(playerPosition.x - obstructionPosition.x) < Mathf.Abs(playerPosition.z - obstructionPosition.z))
        {
            //set z (longer position)
            if (playerPosition.z - obstructionPosition.z > 0)
            {
                if (playerPosition.x - obstructionPosition.x > 0)
                {
                    playerPosition.z = obstructionPosition.z + 1.01f;
                }
                else
                {
                    playerPosition.z = obstructionPosition.z + 1.01f;
                }
            }

            else
            {
                if (playerPosition.x - obstructionPosition.x > 0)
                {
                    playerPosition.z = obstructionPosition.z - 1.01f;
                }
                else
                {
                    playerPosition.z = obstructionPosition.z - 1.01f;
                }
            }
        }
        else
        {
            //set x (longer position)
            if (playerPosition.z - obstructionPosition.z > 0)
            {
                if (playerPosition.x - obstructionPosition.x > 0)
                {
                    playerPosition.x = obstructionPosition.x + 1.01f;
                }
                else
                {
                    playerPosition.x = obstructionPosition.x - 1.01f;
                }
            }

            else
            {
                if (playerPosition.x - obstructionPosition.x > 0)
                {
                    playerPosition.x = obstructionPosition.x + 1.01f;
                }
                else
                {
                    playerPosition.x = obstructionPosition.x - 1.01f;
                }
            }
        }
        player.transform.position = playerPosition;
    }
}
