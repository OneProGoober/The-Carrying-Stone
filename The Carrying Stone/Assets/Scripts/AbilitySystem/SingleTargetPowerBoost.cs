using UnityEngine;
using System.Collections.Generic;
using System.CodeDom;

[CreateAssetMenu(menuName = "Abilities/Offensive/Single Power Boost")]
public class SingleTargetPowerBoost : DefensiveAbility {

    public GameObject buffParticles;
    public float buffAmount;

    public override void UseAbility()
    {
        GameObject go = Instantiate(buffParticles, playerAbilities.transform.position, Quaternion.identity);
        go.transform.SetParent(playerAbilities.transform);
        go.transform.eulerAngles -= new Vector3(90f, 0f, 0f);
        Destroy(go, abilityLifetime);
    }

}
