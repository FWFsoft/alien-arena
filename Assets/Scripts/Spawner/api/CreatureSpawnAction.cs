using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.Api;

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
