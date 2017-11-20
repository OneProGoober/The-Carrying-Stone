using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Defensive/Multiple Target Healing")]
public class MultiTargetHeal : DefensiveAbility
{

    [Header("Healing Specific Properties")]
    public int healAmount;
    public float abilityRadius;
    public float betweenCycles;
    public GameObject healParticleSystem;
	public bool effectsCastingPlayer;
	public int healCycles;
	public int maxTargets;

    public override void UseAbility()
    {
        Collider[] targets = Physics.OverlapSphere(playerAbilities.transform.position, abilityRadius);
        List<PlayerAbilities> players = new List<PlayerAbilities>();
        
		foreach (Collider c in targets)
        {
            if (c.GetComponent<PlayerAbilities>() && c.GetComponent<Team>().team == teamWhoCast)
            {
                players.Add(c.GetComponent<PlayerAbilities>());
            }
        }
		
		if (!effectsCastingPlayer)
        {
            players.Remove(playerAbilities);
        }

        Debug.Log(players.Count);

        if (players.Count > 0)
        {
			int targs = (players.Count > maxTargets) ? maxTargets : players.Count;
			
            Debug.Log("Found " + players.Count + " on the same team, applying buff!");
            for (int i = 0; i < targs; i++)
            {
                GameObject go = Instantiate(healParticleSystem, players[i].transform.position, Quaternion.identity);
                go.transform.SetParent(players[i].transform);
                go.transform.eulerAngles -= new Vector3(90f, 0f, 0f);
                players[i].StartCoroutine(players[i].ApplyHealing(healAmount, healCycles, betweenCycles));
                Destroy(go, abilityLifetime);
            }
        }
		
    }
}
