using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveAbility : Ability {

    public float damagePerSecond;

    public override void UseAbility()
    {
        Debug.LogWarning("Using ability from OffensiveAbility class - SHOULD NOT HAPPEN!");
    }

}
