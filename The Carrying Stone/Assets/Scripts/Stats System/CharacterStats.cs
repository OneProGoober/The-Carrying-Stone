using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    
    //Base stat types
    //=======================================================================
    public int Attack ;
    public int Magic ;
    public int Armor ;
    public int MResistance ;
    public int MSpeed ;
    public float CritRate ;
    public float CritDamage ;
    public int MaxHp ;
    public float Hp ;
    public int MaxMp ;
    public int Barrier ;
    public int Mp ;
    public float HpRegen ;
    public float ManaRegen ;
    public float ASpeed ;
    public float Cdr ;
    public int Range ;
    public float Tenacity ;
    public float LifeSteal ;
    public float SpellVamp ;
    public int ArmorPen ;
    public int MagicPen ;

    //Additional Stat types
    //=======================================================================
    [SerializeField] private int TrueDamage ;
    [SerializeField] private int MagicDamage ;
    [SerializeField] private int AttackDamage ;
    [SerializeField] private float HealMultiplier;
    [SerializeField] private float BarrierMultiplier;
    [SerializeField] private float TenacityMultiplier;

    //Calculated Stat types
    //=======================================================================
    [SerializeField] private float PercentDef ;
    [SerializeField] private float PercentMR ;
    [SerializeField] private float PercentArmorPen ;
    [SerializeField] private float PercentMagicPen ;

    //implement a reload function to refresh stats

    //Methods to Calculate the Calculated Stat types
    //=======================================================================
    public void fullCalc()
    {
        calcPercentDef();
        calcPercentMR();
        calcPercentArmorPen();
        calcPercentMagicPen();
    }

    public void calcPercentDef()
    {
        PercentDef = Armor / 10;
    }
    public void calcPercentMR()
    {
        PercentMR = MResistance / 10;
    }
    public void calcPercentArmorPen()
    {
        PercentArmorPen = ArmorPen / 10;
    }
    public void calcPercentMagicPen()
    {
        PercentMagicPen = MagicPen / 10;
    }

    //ensure to set functions to enfource max stats max value for any stat is stored in base stats configuration

    //Methods that handles refreshing Character Stats when there is a change
    public void reloadCharacterStats()
    {
        //Debug.Log("Character Stats have been updated");
        //Pull in the base stats
        //Pull in the stats from items
        //Pulls in additional stats from buffs etc
        fullCalc();
    }
}
