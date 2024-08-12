using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner.Api;
using Creatures.Api;

namespace Spawner.impl {
    public class HealthSpawnAction : CreatureSpawnAction
    {
        private readonly int health;

        public HealthSpawnAction(int health)
        {
            this.health = health;
        }

        public override Creature apply(Creature creature)
        {
            var healthScript = creature.GetComponent<HealthScript>();
            healthScript.health = health;
            return creature;
        }
    }
}
