using System.Collections;
using System.Collections.Generic;

using Creatures.Api;

using UnityEngine;

namespace Spawner.Api
{
    /*
     * Base class for spawning creatures, distinguished this as 
     * creature spawner in order to leave room for spawners that
     * can spawn things like treasure, objectives, events, etc.
     */
    public abstract class CreatureSpawnAction
    {
        public abstract Creature apply(Creature creature);
    }
}
