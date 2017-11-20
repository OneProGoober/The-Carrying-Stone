using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : AbilityDamager {

    void Start()
    {
        Debug.Log("DPS: " + damagePerSecond);
    }

    void OnTriggerStay(Collider coll)
    {
        Debug.Log("Inside trigger");
        Debug.Log(coll.gameObject.GetComponent<PlayerAbilities>());
        Debug.Log(coll.gameObject.GetComponent<Team>().team);
        if (coll.gameObject.GetComponent<PlayerAbilities>() && coll.GetComponent<Team>().team != teamWhoShot)
        {
            Debug.Log("Dealing " + damagePerSecond * Time.deltaTime + " damage to " + coll.gameObject.name);
            coll.gameObject.GetComponent<PlayerAbilities>().DealDamage(damagePerSecond * Time.deltaTime);
        }
    }

}
