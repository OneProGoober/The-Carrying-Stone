using UnityEngine;
using System.Collections.Generic;
using System.CodeDom;

[CreateAssetMenu(menuName = "Abilities/Offensive/AoE Power Boost")]
public class AoEPowerBoost : DefensiveAbility
{
    public GameObject buffParticles;
    public float abilityRadius;
    public int buffAmount;
    public PlayerAbilities.EBuffType buff;
    public bool effectsCastingPlayer;
    public int maxPlayersToBoost;

    public override void UseAbility()
    {
        Debug.Log("Casting AoE Power Boost");
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
            int effectors = (maxPlayersToBoost > players.Count) ? players.Count : maxPlayersToBoost;
            Debug.Log("Found " + players.Count + " on the same team, applying buff!");
            for (int i = 0; i < effectors; i++)
            {
                GameObject go = Instantiate(buffParticles, players[i].transform.position, Quaternion.identity);
                go.transform.SetParent(players[i].transform);
                go.transform.eulerAngles -= new Vector3(90f, 0f, 0f);
                players[i].StartCoroutine(players[i].ApplyBuff(buff, buffAmount, abilityLifetime));
                Destroy(go, abilityLifetime);
            }
        }
    }
}
