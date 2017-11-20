using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Defensive/Single Target Healing")]
public class SingleTargetHealing : DefensiveAbility {

    [Header("Healing Specific Properties")]
    public int healPerCycle;
    public int numberOfHealCycles;
    public float betweenCycles;
    public GameObject healParticleSystem;
    public bool healSelf;

    public override void UseAbility()
    {
        Debug.Log("Using single target heal");

        if (healSelf)
        {
            GameObject go = Instantiate(healParticleSystem, playerAbilities.transform.position, Quaternion.identity);
            Destroy(go, abilityLifetime);
            Debug.Log("Healing self");
            go.transform.SetParent(playerAbilities.transform);
            playerAbilities.StartCoroutine(playerAbilities.ApplyHealing(healPerCycle, numberOfHealCycles, betweenCycles));
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Found raycast target called : " + hit.collider.name);
                PlayerAbilities p = hit.collider.GetComponent<PlayerAbilities>();
                if (p != null && p.GetComponent<Team>().team == teamWhoCast)
                {
                    Debug.Log("Applied healing affect");
                    p.StartCoroutine(p.ApplyHealing(healPerCycle, numberOfHealCycles, betweenCycles));

                    GameObject go = Instantiate(healParticleSystem, p.transform.position, Quaternion.identity);
                    Destroy(go, abilityLifetime);
                    go.transform.SetParent(p.transform);
                }
            }
        }
    }
}
