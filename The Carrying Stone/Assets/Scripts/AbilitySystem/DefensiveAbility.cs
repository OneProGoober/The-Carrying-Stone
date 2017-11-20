using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAbility : Ability {

    public override void UseAbility()
    {
        Debug.LogWarning("Using UseAbility from DefensiveAbility script - SHOULD NOT HAPPEN");
    }

}
