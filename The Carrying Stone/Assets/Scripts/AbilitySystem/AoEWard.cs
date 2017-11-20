using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Defensive/AoE Ward")]
public class AoEWard : DefensiveAbility
{
    [Header("Ward Specific Properties")]
	public int buffModifier;
	public GameObject wardParticleEffect;
    public float radius;
	public bool effectsCastingPlayer;
	public PlayerAbilities.EBuffType buff;
	public int maxPlayersAffected;

	public override void UseAbility()
	{
        Collider[] targets = Physics.OverlapSphere(playerAbilities.transform.position, radius);
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
			int playersToAffect = (players.Count > maxPlayersAffected) ? maxPlayersAffected : players.Count;
            
            for (int i = 0; i < playersToAffect; i++)
            {
                GameObject go = Instantiate(wardParticleEffect, players[i].transform.position, Quaternion.identity);
                go.transform.SetParent(players[i].transform);
                go.transform.eulerAngles += new Vector3(90f, 0f, 0f);
                players[i].StartCoroutine(players[i].ApplyBuff(buff, buffModifier, abilityLifetime));
                Destroy(go, abilityLifetime);
            }
        }
    }
}
