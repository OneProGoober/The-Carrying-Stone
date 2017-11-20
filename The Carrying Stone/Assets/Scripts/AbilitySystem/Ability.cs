using UnityEngine;

/* ABILITY IDEAS */
/* TEMPORARY WEAPON (THINK OBLIVION SUMMON WEAPON)
   DEBUFF ENEMIES
   AREA DAMAGE */

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;

    [Range(0.1f, 30f)]
    public float abilityLifetime;
    [Range(1f, 60f)]
    public float cooldownTime;
    
    [HideInInspector] public float lastUsedTime = 0f;
    [HideInInspector] public PlayerAbilities playerAbilities;
    [HideInInspector] public string teamWhoCast;

    // public Sprite abilitySprite; Will eventually be used in ability UI
    // public AudioClip audio; Will eventually be used to play sound when ability is fired

    public abstract void UseAbility();

}
