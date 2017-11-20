using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{	
    // Doesn't contain a health or MP stat, as they can't be buffed
    public enum EBuffType { Attack = 0, Magic = 1, Armor = 2, MResistance = 3, MSpeed = 4, CritRate = 5, CritDamage = 6, MaxHP = 7, MaxMP = 9, Barrier = 10, HpRegen = 12, ManaRegen = 13, ASpeed = 14, Cdr = 15, Range = 16, Tenacity = 17, LifeSteal = 18, SpellVamp = 19, ArmorPen = 20, MagicPen = 21}

    public List<Ability> abilities = new List<Ability>();
	public List<KeyCode> keybinds = new List<KeyCode>();

    string t;

    void Start()
    {
        t = GetComponent<Team>().team;

        foreach (Ability a in abilities)
        {
            if (a.abilityLifetime == 0 || a.cooldownTime == 0)
            {
                Debug.LogWarning("Ability " + a.name + " has 0 cooldown or 0 ability lifetime");
            }

            ResetLastUsedTime(a); // Added in as scriptable object stores values during each session

        }

        if (abilities.Count != keybinds.Count)
        {
            Debug.LogWarning("Abilties and keybinds list are not the same!");
        }
    }
	
	void Update()
	{
		for (int i = 0; i < abilities.Count; i++)
        {
            if (Input.GetKeyDown(keybinds[i]))
            {
                if (Time.time >= abilities[i].lastUsedTime)
                {
                    UseAbility(abilities[i], this, t);
                    Debug.Log("Casting " + abilities[i].abilityName);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("Clicked Enemy!");
                    hit.collider.gameObject.GetComponent<EnemyAI>().Damage(this, 9999);
                }
            }
        }
    }
	
	public void UseAbility(Ability ability, PlayerAbilities player, string t)
	{
        ability.playerAbilities = player;
        ability.teamWhoCast = t;
		ability.UseAbility();
        ability.lastUsedTime = Time.time + ability.cooldownTime;
	}	

    void ResetLastUsedTime(Ability ability)
    {
        ability.lastUsedTime = 0f;
    }

    public IEnumerator ApplyBuff(EBuffType _buffType, float buffAmount, float duration)
    {
        Debug.Log("Applying float buff to " + gameObject.name + " of " + _buffType.ToString() + " for " + duration + " seconds with " + buffAmount + " buff amount");
        CharacterStats cs = GetComponent<Character>().Stats;
        switch (_buffType)
        {
            case EBuffType.CritRate:
                cs.CritRate *= buffAmount;
                break;
            case EBuffType.CritDamage:
                cs.CritDamage *= buffAmount;
                break;
            case EBuffType.HpRegen:
                cs.HpRegen *= buffAmount;
                break;
            case EBuffType.ManaRegen:
                cs.ManaRegen *= buffAmount;
                break;
            case EBuffType.ASpeed:
                cs.ASpeed *= buffAmount;
                break;
            case EBuffType.Cdr:
                cs.Cdr *= buffAmount;
                break;
            case EBuffType.Tenacity:
                cs.Tenacity *= buffAmount;
                break;
            case EBuffType.LifeSteal:
                cs.LifeSteal *= buffAmount;
                break;
            case EBuffType.SpellVamp:
                cs.SpellVamp *= buffAmount;
                break;         
        }

        yield return new WaitForSeconds(duration);

        switch (_buffType)
        {
            case EBuffType.CritRate:
                cs.CritRate /= buffAmount;
                break;
            case EBuffType.CritDamage:
                cs.CritDamage /= buffAmount;
                break;
            case EBuffType.HpRegen:
                cs.HpRegen /= buffAmount;
                break;
            case EBuffType.ManaRegen:
                cs.ManaRegen /= buffAmount;
                break;
            case EBuffType.ASpeed:
                cs.ASpeed /= buffAmount;
                break;
            case EBuffType.Cdr:
                cs.Cdr /= buffAmount;
                break;
            case EBuffType.Tenacity:
                cs.Tenacity /= buffAmount;
                break;
            case EBuffType.LifeSteal:
                cs.LifeSteal /= buffAmount;
                break;
            case EBuffType.SpellVamp:
                cs.SpellVamp /= buffAmount;
                break;
        }
    }

    public IEnumerator ApplyBuff(EBuffType _buffType, int buffAmount, float duration)
    {
        Debug.Log("Applying int buff to " + gameObject.name + " of " + _buffType.ToString() + " for " + duration + " seconds with " + buffAmount + " buff amount");
        CharacterStats cs = GetComponent<Character>().Stats;
        switch (_buffType)
        {
            case EBuffType.Attack:
                cs.Attack *= buffAmount;
                break;
            case EBuffType.Magic:
                cs.Magic *= buffAmount;
                break;
            case EBuffType.Armor:
                cs.Armor *= buffAmount;
                break;
            case EBuffType.MResistance:
                cs.MResistance *= buffAmount;
                break;
            case EBuffType.MSpeed:
                cs.MSpeed *= buffAmount;
                break;
            case EBuffType.MaxHP:
                cs.MaxHp *= buffAmount;
                break;
            case EBuffType.MaxMP:
                cs.MaxMp *= buffAmount;
                break;
            case EBuffType.Barrier:
                cs.Barrier *= buffAmount;
                break;
            case EBuffType.Range:
                cs.Range *= buffAmount;
                break;
            case EBuffType.ArmorPen:
                cs.ArmorPen *= buffAmount;
                break;
            case EBuffType.MagicPen:
                cs.MagicPen *= buffAmount;
                break;
        }

        yield return new WaitForSeconds(duration);

        switch (_buffType)
        {
            case EBuffType.Attack:
                cs.Attack /= buffAmount;
                break;
            case EBuffType.Magic:
                cs.Magic /= buffAmount;
                break;
            case EBuffType.Armor:
                cs.Armor /= buffAmount;
                break;
            case EBuffType.MResistance:
                cs.MResistance /= buffAmount;
                break;
            case EBuffType.MSpeed:
                cs.MSpeed /= buffAmount;
                break;
            case EBuffType.MaxHP:
                cs.MaxHp /= buffAmount;
                break;
            case EBuffType.MaxMP:
                cs.MaxMp /= buffAmount;
                break;
            case EBuffType.Barrier:
                cs.Barrier /= buffAmount;
                break;
            case EBuffType.Range:
                cs.Range /= buffAmount;
                break;
            case EBuffType.ArmorPen:
                cs.ArmorPen /= buffAmount;
                break;
            case EBuffType.MagicPen:
                cs.MagicPen /= buffAmount;
                break;
        }
    }

    public IEnumerator ApplyHealing(int healPerCycle, int cycles, float timeBetweenCycles)
    {
        Debug.Log("Healing " + name +" by " + healPerCycle + " for " + cycles + " cycles");

        CharacterStats cs = GetComponent<Character>().Stats;

        for (int i = 0; i < cycles; i++)
        {
            Debug.Log("Healed "+ name + " by " + healPerCycle);
            cs.Hp += healPerCycle;
            yield return new WaitForSeconds(timeBetweenCycles);
        }
    }
	
	public void DealDamage(float damage)
	{
        Debug.Log("Dealing " + damage + " damage!");
        // Decrease health
        CharacterStats cs = GetComponent<Character>().Stats;
        cs.Hp -= damage;
    }
}