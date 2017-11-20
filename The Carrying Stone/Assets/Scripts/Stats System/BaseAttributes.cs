using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseAttributes{
    //Base stats of a character
    //====================================================================
    public int Attack ;
    public int Magic ;
    public int Armor ;
    public int MResistance ;
    public int MSpeed ;
    public float CritRate ;
    public float CritDamage ;
    public int MaxHp ;
    public int Hp ;
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

    // Stats Per Level (Pl)
    //====================================================================
    public int AttackPl ;
    public int MagicPl ;
    public int ArmorPl ;
    public int MResistancePl ;
    public int MSpeedPl ;
    public float CritRatePl ;
    public float CritDamagePl ;
    public int MaxHpPl ;
    public int HpPl ;
    public int MaxMpPl ;
    public int BarrierPl ;
    public int MpPl ;
    public float HpRegenPl ;
    public float ManaRegenPl ;
    public float ASpeedPl ;
    public float CdrPl ;
    public int RangePl ;
    public float TenacityPl ;
    public float LifeStealPl ;
    public float SpellVampPl ;
    public int ArmorPenPl ;
    public int MagicPenPl ;

    //Leveling attributes
    //======================================================================
    public int MaxLevel ; //Max Level the character can reach
    public int[] ExpTable; //Exp Table

    public void increaseBaseStats()
    {
        upAttack();
        upMagic();
        upArmor();
        upMResistance();
        upMSpeed();
        upCritRate();
        upCritDamage();
        upMaxHp();
        upHp();
        upMaxMp();
        upBarrier();
        upMp();
        upHpRegen();
        upManaRegen();
        upASpeed();
        upCdr();
        upRange();
        upTenacity();
        upLifeSteal();
        upSpellVamp();
        upArmorPen();
        upMagicPen();
        Debug.Log("BaseStats Increased");
    }

    //Change these methods to public if necessary in the future. Currently base stats will only be increased internally
    //Stat increase methods
    //==========================================================================
    private void upAttack()
    {
        Attack = Attack + AttackPl;
    }

    private void upMagic()
    {
        Magic = Magic + MagicPl;
    }
    
    private void upArmor()
    {
        Armor = Armor + ArmorPl;
    }
    
    private void upMResistance()
    {
        MResistance = MResistance + MResistancePl;
    }

    private void upMSpeed()
    {
        MSpeed = MSpeed + MSpeedPl;
    }

    private void upCritRate()
    {
        CritRate = CritRate + CritRatePl;
    }

    private void upCritDamage()
    {
        CritDamage = CritDamage + CritDamagePl;
    }

    private void upMaxHp()
    {
        MaxHp = MaxHp + MaxHpPl;
    }

    private void upHp()
    {
        Hp = Hp + HpPl;
    }

    private void upMaxMp()
    {
        MaxMp = MaxMp + MaxMpPl;
    }

    private void upBarrier()
    {
        Barrier = Barrier + BarrierPl;
    }

    private void upMp()
    {
        Mp = Mp + MpPl;
    }

    private void upHpRegen()
    {
        HpRegen = HpRegen + HpRegenPl;
    }

    private void upManaRegen()
    {
        ManaRegen = ManaRegen + ManaRegenPl;
    }

    private void upASpeed()
    {
        ASpeed = ASpeed + ASpeedPl;
    }

    private void upCdr()
    {
        Cdr = Cdr + CdrPl;
    }

    private void upRange()
    {
        Range = Range + RangePl;
    }

    private void upTenacity()
    {
        Tenacity = +TenacityPl;
    }

    private void upLifeSteal()
    {
        LifeSteal = LifeSteal + LifeStealPl;
    }

    private void upSpellVamp()
    {
        SpellVamp = SpellVamp + SpellVampPl;
    }

    private void upArmorPen()
    {
        ArmorPen = ArmorPen + ArmorPenPl;
    }

    private void upMagicPen()
    {
        MagicPen = MagicPen + MagicPenPl;
    }
}
