using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public int health;
    int maxHealth;

    // On each hit, set the last player to damage so we can work out who 'owns' the spawn area
    PlayerAbilities lastPlayerToDamage;

    // Set the original spawn area so we know player damage for area
    MobSpawnArea spawnArea;

    // When an enemy is damaged, must call spawnarea.AddDamageToTeam to track who's dealt the most damage
    public void Damage(PlayerAbilities player, int damage)
    {
        Player p = player.GetComponent<Player>();
        spawnArea.AddDamageToTeam(p.side, damage);
        lastPlayerToDamage = player;
        spawnArea.lastTeam = p.side;

        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Should be dying");
            spawnArea.RemoveEnemy(this);
            spawnArea.lastTeam = p.side;
                Destroy(gameObject);
        }
    }

    public void AddSpawnArea(MobSpawnArea _spawnArea)
    {
        spawnArea = _spawnArea;
    }
}
