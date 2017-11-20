using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DartProjectile : AbilityDamager
{
    public float moveSpeed;
	public float damageToDeal;

    bool hasHitPlayer;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (!hasHitPlayer)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
		PlayerAbilities player = coll.gameObject.GetComponent<PlayerAbilities>();
        rb.isKinematic = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        Invoke("Freeze", 0.005f);
        if (player != null)
		{
            transform.SetParent(player.transform);
            if (teamWhoShot == coll.gameObject.GetComponent<Team>().team)
			{
                // Friendly Fire
                Debug.Log("Hit friendly");
			}
			else
			{
				player.DealDamage(damageToDeal);
                Debug.Log("Dealing " + damageToDeal + " to " + player.name);
			}
		}
		else
		{
			// Hit something that is not the player
		}
    }
	
	void Freeze()
	{
        hasHitPlayer = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnCollisionExit(Collision coll)
    {
        Debug.Log("Exited collider");
    }

}
