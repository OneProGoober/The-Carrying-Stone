using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[CreateAssetMenu(menuName ="Abilities/Offensive/AoE Ability")]
public class OffensiveAoEAbility : OffensiveAbility
{
    public float abilityRadius;

    public GameObject instantiatePrefab;

	public override void UseAbility()
	{
        GameObject go = Instantiate(instantiatePrefab, playerAbilities.transform.position, Quaternion.identity);
        AbilityDamager ad = go.GetComponent<AbilityDamager>();
        ad.teamWhoShot = teamWhoCast;
        ad.damagePerSecond = damagePerSecond;
        
        go.transform.eulerAngles += new Vector3(90f, 0f, 0f);
        go.GetComponent<SphereCollider>().radius = abilityRadius;
        Destroy(go, abilityLifetime);
	}

}
