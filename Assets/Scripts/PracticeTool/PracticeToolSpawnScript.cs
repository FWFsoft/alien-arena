using System.Collections;
using System.Collections.Generic;

using Creatures.Api;

using Spawner.Api;
using Spawner.impl;

using UnityEngine;

namespace PracticeTool
{
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

        GameObject SpawnDummyEnemyAtPoint(GameObject spawnPoint, int health = 100, int size = 1)
        {
            Creature enemyInstance = new SpawnChain(enemyToSpawn.GetComponent<Creature>(), spawnPoint)
                .with(new HealthSpawnAction(health))
                .with(new SizeSpawnAction(size))
                .with(new ImmobileSpawnAction())
                .spawn();

            // Assuming EffectsManagerScript's SetupInstance() should still be called
            var managerScript = enemyInstance.GetComponent<EffectsManagerScript>();
            managerScript.SetupInstance();

            return enemyInstance.gameObject;
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
            foreach (var enemy in units)
            {
                Destroy(enemy);
            }
            foreach (var spawnPoint in smallEnemySpawnPoints)
            {
                units.Add(SpawnDummyEnemyAtPoint(spawnPoint));
            }
            units.Add(SpawnDummyEnemyAtPoint(individualSpawnPoint));
            units.Add(SpawnDummyEnemyAtPoint(bossSpawnPoint, 1000, 3));
            units.Add(SpawnAllyAtPoint(alliedSpawnPoint));
        }

    }
}
