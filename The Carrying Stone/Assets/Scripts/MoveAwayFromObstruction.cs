using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromObstruction : MonoBehaviour
{
    public List<GameObject> obstructions;

    private void Start()
    {
        obstructions = new List<GameObject>();
    }

    public bool CheckValidMoves(RaycastHit spotClicked, Transform playerLoc)
    {
        //for each obstruction that is touched, get the valid moves as if it were the only obstruction touched.
        //"AND" them together to determine the final valid movements
        foreach (var obs in obstructions)
        {
            Vector3 collisionPoint = new Vector3(0,0,0);
            Vector3 dir = playerLoc.gameObject.transform.position - obs.transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit))
            {
                collisionPoint = GetComponent<Collider>().ClosestPointOnBounds(hit.point);
            }
            
            Debug.Log("collisionPoint Shouldn't be 0,0,0: " + collisionPoint);
            Vector3 collToClickPos = hit.point - collisionPoint; // Direction from mouse click pos to collision point
            Vector3 dir2 = (collisionPoint - obs.transform.position).normalized; // Direction from collision point to collision position
            Vector3 roundedDir = Vector3.zero;

            bool isXGreater = Mathf.Abs(dir.x) > Mathf.Abs(dir.z);

            if (isXGreater)
            {
                roundedDir.x = Mathf.Round(dir.x);
            }
            else
            {
                roundedDir.z = Mathf.Round(dir.z);
            }

            float angle = Vector3.Angle(roundedDir, collToClickPos);

            //Debug.Log("Angle: " + angle);

            if (angle < 90)
            {
                Debug.Log("Valid MOVE!");
            }
            else
            {
                Debug.Log("Invalid MOVE!");
                return false;
            }
        }
        //made it through all obstructions with a valid move...
        obstructions.Clear();
        return true;
    }
}
