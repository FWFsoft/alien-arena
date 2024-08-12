using System.Collections;
using System.Collections.Generic;
using Creatures.Api;
using Spawner.Api;
using UnityEngine;

namespace Spawner.Api
{
    public class SpawnChain
    {
        private GameObject spawnPoint;
        private Stack<CreatureSpawnAction> spawnActions;
        private Creature creatureInstance;

        public SpawnChain(Creature creature, GameObject spawnPoint) {
            this.spawnActions = new Stack<CreatureSpawnAction>();
            this.creatureInstance = Object.Instantiate(creature, spawnPoint.transform.position, Quaternion.identity);
            this.spawnPoint = spawnPoint;            
        }

        public SpawnChain with(CreatureSpawnAction spawner) {
            spawnActions.Push(spawner); 
            return this;
        }

        public Creature spawn()
        {
            foreach (CreatureSpawnAction spawnAction in spawnActions)
            {
                creatureInstance = spawnAction.apply(creatureInstance);
            }
            return creatureInstance;
        }
    }
}
