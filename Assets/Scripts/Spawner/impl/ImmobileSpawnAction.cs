using System.Collections;
using System.Collections.Generic;

using Creatures.Api;

using Spawner.Api;

using UnityEngine;

namespace Spawner.impl
{
    public class ImmobileSpawnAction : CreatureSpawnAction
    {
        public override Creature apply(Creature creature)
        {
            creature.Speed = 0.0f;
            return creature;
        }
    }
}
