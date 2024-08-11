using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.Api;
using Spawner.Api;

namespace Spawner.impl {
    public class ImmobileSpawnAction : CreatureSpawnAction
    { 
        public override Creature apply(Creature creature) {
            creature.Speed = 0.0f;
            return creature;
        }
    }
}