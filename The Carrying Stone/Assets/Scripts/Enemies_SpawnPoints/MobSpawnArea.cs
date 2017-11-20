using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MobSpawnArea : MonoBehaviour {

    public enum EWhoGetsBuff { MostDamage, LastHit }
	public enum EBuffType { SpawnAreaOnly, WholeTeam }
	public enum EHowToSpawnEnemies { OneRandomEnemyType, TrulyRandom, OneEnemyType }

    [Header("Enums")]
    public EWhoGetsBuff whoGetsBuff;
	public EBuffType buffType;
	public EHowToSpawnEnemies howToSpawnEnemies;

    [Header("Buff Attributes")]
    public PlayerAbilities.EBuffType eBuffType;
    public float buffAmount;
    public float buffDuration;

    [Header("Possible Enemies to Spawn")]
	public List<EnemyAI> enemyPrefabs = new List<EnemyAI>();
	
    [Header("Enemy Movement Variables")]
	public Vector3 minPoint;
    public Vector3 maxPoint;

    [Range(2,20)]
	public int minEnemiesToSpawn, maxEnemiesToSpawn;
    public int respawnSpeed;

    public TeamManager teamManager;
    [HideInInspector] public string lastTeam;
	
    /******** Private variables *********/
	List<EnemyAI> currentEnemies = new List<EnemyAI>();
	
	int enemiesLeft;
	int teamOneDamage, teamTwoDamage;

    WaitForSeconds spawnTime;
	
	void Start()
	{
		CheckRequiredComponents();
        Invoke("SpawnEnemies", 2f);
        spawnTime = new WaitForSeconds(respawnSpeed);
	}

    public void AddDamageToTeam(string teamname, int enemyHealth)
    {
        if (teamname == "TeamOne")
        {
            teamOneDamage += enemyHealth;
        }
        else if (teamname == "TeamTwo")
        {
            teamTwoDamage += enemyHealth;
        }
        else
        {
            Debug.LogWarning("Trying to add damage to mob area, but cannot assign to a team");
        }
    }

    public void ApplyBuffToTeam(string teamName)
    {
        Debug.Log("Spawn Area cleared.. Applying buff!");
        switch (buffType)
		{
			case EBuffType.SpawnAreaOnly:
			
			float radius = GetComponent<SphereCollider>().radius;
			Collider[] colls = Physics.OverlapSphere(transform.position, radius);
			
			foreach (Collider coll in colls)
			{
				if (coll.gameObject.GetComponent<Team>())
				{
                        if (coll.gameObject.GetComponent<Team>().team == teamName)
                        {
                            PlayerAbilities pa = coll.gameObject.GetComponent<PlayerAbilities>();
                            pa.StartCoroutine(pa.ApplyBuff(eBuffType, buffAmount, buffDuration));
                        }
				}
			}			
			break;
				
			case EBuffType.WholeTeam:
				if (teamName == "Team 1")
				{
					for (int i = 0; i < teamManager.teamOneArray.Count; i++)
					{
						// teamManager.teamOneArray[i].ApplyBuff();
					}
				}
				else if (teamName == "Team 2")
				{
					for (int i = 0; i < teamManager.teamTwoArray.Count; i++)
					{
						// teamManager.teamTwoArray[i].ApplyBuff();
					}
				}
				else
				{
					Debug.LogWarning("Trying to apply buff to the whole team.. but failed to find anyone!");
				}
			break;
		}
    }
	
	void CheckRequiredComponents()
	{
		if (gameObject.tag != "SpawnArea")
		{
			Debug.LogWarning("Spawn Area at " + transform.position + " does not have the required SpawnArea tag!");
		}
		if (!gameObject.GetComponent<SphereCollider>().isTrigger)
		{
			Debug.LogWarning("Spawn Area at " + transform.position + " does not have a triggerable collider!");
		}
	}
	
	void SpawnEnemies()
	{
		int enemies = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);
		enemiesLeft = enemies;
		
		switch (howToSpawnEnemies)
		{
			case EHowToSpawnEnemies.OneRandomEnemyType:

				int rand = Random.Range(0, enemyPrefabs.Count);
				for (int i = 0; i < enemies; i++)
				{
					GameObject go = Instantiate(enemyPrefabs[rand].gameObject, GenerateSpawnPoint(), Quaternion.identity);
					currentEnemies.Add(go.GetComponent<EnemyAI>());
					go.GetComponent<EnemyAI>().AddSpawnArea(this);
				}
				
				break;
				
			case EHowToSpawnEnemies.TrulyRandom:
				
				for (int i = 0; i < enemies; i++)
				{
					int r = Random.Range(0, enemyPrefabs.Count);
					GameObject go = Instantiate(enemyPrefabs[r].gameObject, GenerateSpawnPoint(), Quaternion.identity);
					currentEnemies.Add(go.GetComponent<EnemyAI>());
					go.GetComponent<EnemyAI>().AddSpawnArea(this);
				}
				break;

            case EHowToSpawnEnemies.OneEnemyType:

                for (int i = 0; i < enemies; i++)
                {
                    GameObject go = Instantiate(enemyPrefabs[0].gameObject, GenerateSpawnPoint(), Quaternion.identity);
                    currentEnemies.Add(go.GetComponent<EnemyAI>());
                    go.GetComponent<EnemyAI>().AddSpawnArea(this);
                }

                break;
		}		
	}
	
	public void RemoveEnemy(EnemyAI enemy)
	{
		currentEnemies.Remove(enemy);
        enemiesLeft--;

        if (enemiesLeft == 0)
        {
            ApplyBuffToTeam(lastTeam);
            Invoke("SpawnEnemies", respawnSpeed);
        }
	}
	
	Vector3 GenerateSpawnPoint()
	{
        // Add logic for overlapping enemies
		return new Vector3(Random.Range(minPoint.x, maxPoint.x), 0f, Random.Range(minPoint.z, maxPoint.z));
	}
}

