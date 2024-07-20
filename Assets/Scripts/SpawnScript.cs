using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public InputHandler inputHandler;
    public float timeBetweenSpawn = 5f;
    [SerializeField]
    public GameObject enemyToSpawn;
    [SerializeField]
    public GameObject allyToSpawn;

    public List<GameObject> smallEnemySpawnPoints;

    public GameObject alliedSpawnPoint;
    public GameObject individualSpawnPoint;
    public GameObject bossSpawnPoint;

    private List<GameObject> units;
    // Start is called before the first frame update
    void Start()
    {
        units = new List<GameObject>();
        ResetEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.shouldReset)
        {
            ResetEnemies();
        }
    }

    GameObject SpawnEnemyAtPoint(GameObject spawnPoint, int health = 100, int size = 1)
    {

        var enemy = Instantiate(enemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        var managerScript = enemy.GetComponent<EffectsManagerScript>();
        var healthScript = enemy.GetComponent<HealthScript>();
        healthScript.health = health;

        var scale = enemy.transform.localScale;
        enemy.transform.localScale = new Vector3(scale.x * size, scale.y * size, scale.z * size);

        enemy.GetComponent<AlienScript>().speed = 0; // Dummies don't move

        managerScript.SetupInstance();
        return enemy;

    }


    GameObject SpawnAllyAtPoint(GameObject spawnPoint)
    {

        var ally = Instantiate(allyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        //var managerScript = ally.GetComponent<EffectsManagerScript>();
        //var healthScript = ally.GetComponent<HealthScript>();

        //managerScript.SetupInstance();
        return ally;

    }


    void ResetEnemies()
    {
        foreach(var enemy in units)
        {
            Destroy(enemy);
        }
        foreach(var spawnPoint in smallEnemySpawnPoints)
        {
            units.Add(SpawnEnemyAtPoint(spawnPoint));
        }
        units.Add(SpawnEnemyAtPoint(individualSpawnPoint));
        units.Add(SpawnEnemyAtPoint(bossSpawnPoint, 1000, 3));
        units.Add(SpawnAllyAtPoint(alliedSpawnPoint));
        // TODO: Spawn ally? Define ally?
    }

}
