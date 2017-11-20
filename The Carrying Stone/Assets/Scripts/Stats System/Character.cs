using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character: MonoBehaviour
{
    /*Responsible for managing the base stats of characters, contains the base stats and stats per level.
     * Loaded from a JSON file on start internally updates itself upon level up
     * accessed through {get; set;}
     */

    [SerializeField]
    private BaseAttributes baseStats;

    //Stat file to load into the base stats.
    [SerializeField]
    private string StatsFile = "CHARACTER_BASESTATS";

    [SerializeField]
    private int currentExp;
    [SerializeField]
    private int currentLevel;

    [SerializeField]
    public CharacterStats Stats;//Combination of Stats from: base stats + items + abilities and modifiers 
    
    void Start ()
    {
        //Load base stats based on the stat file selected
        baseStats = JsonStatLoader.DataLoader(StatsFile);
        currentLevel = 1;

        Stats.reloadCharacterStats(); //calculate CharacterStats for the first time
    }

    void Update ()
    {
        ChecklevelUp(); //Checks to see if we have leveled up
    }

    public void gainExp(int exp)
    {
        currentExp = currentExp + exp;
    }

    public void ChecklevelUp()
    {
        if (currentLevel != baseStats.MaxLevel) {
            if (currentExp >= baseStats.ExpTable[currentLevel - 1])
            {
                currentExp = currentExp - baseStats.ExpTable[currentLevel - 1];
                currentLevel++;
                baseStats.increaseBaseStats();
                Stats.reloadCharacterStats();
            }
        }
    }
}
