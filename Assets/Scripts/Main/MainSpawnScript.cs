using System.Collections;
using System.Collections.Generic;

using Creatures.Api;

using Spawner.Api;
using Spawner.impl;

using UnityEngine;

namespace Main
{
    public class SpawnScript : MonoBehaviour
    {
        public InputHandler inputHandler;
        [SerializeField]
        public GameObject enemyToSpawn;
        [SerializeField]

        public List<GameObject> smallEnemySpawnPoints;

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
            Creature enemyInstance = new SpawnChain(enemyToSpawn.GetComponent<Creature>(), spawnPoint)
                .with(new HealthSpawnAction(health))
                .with(new SizeSpawnAction(size))
                .spawn();

            // Assuming EffectsManagerScript's SetupInstance() should still be called
            var managerScript = enemyInstance.GetComponent<EffectsManagerScript>();
            managerScript.SetupInstance();

            return enemyInstance.gameObject;

        }


        void ResetEnemies()
        {
            foreach (var spawnPoint in smallEnemySpawnPoints)
            {
                units.Add(SpawnEnemyAtPoint(spawnPoint));
            }
        }

    }
}
