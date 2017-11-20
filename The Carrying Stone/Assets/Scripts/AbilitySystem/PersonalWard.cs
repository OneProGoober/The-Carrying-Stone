using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Defensive/Ward")]
public class PersonalWard : DefensiveAbility
{
    [Header("Ward Specific Properties")]
	[Range(0f, 100f)]
	public float buffModifier;
	public PlayerAbilities.EBuffType buff;
	public GameObject wardParticleEffect;

	public override void UseAbility()
	{
        GameObject go = Instantiate(wardParticleEffect, playerAbilities.transform.position + (-Vector3.up / 2), Quaternion.identity);
        go.transform.SetParent(playerAbilities.transform);
        go.transform.eulerAngles += new Vector3(90f, 0f, 0f);
		playerAbilities.StartCoroutine(playerAbilities.ApplyBuff(buff, buffModifier, abilityLifetime));

        Destroy(go, abilityLifetime);
    }

}
