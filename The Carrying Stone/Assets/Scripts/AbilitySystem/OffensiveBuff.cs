using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveBuff : Ability {

    [Range(1.01f, 2f)]
    public float buffAmount;

    public override void UseAbility()
    {
        Debug.LogWarning("Using ability from OffensiveAbility class - SHOULD NOT HAPPEN!");
    }

}
