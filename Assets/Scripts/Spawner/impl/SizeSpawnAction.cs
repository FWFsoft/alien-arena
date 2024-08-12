using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner.Api;
using Creatures.Api;

namespace Spawner.impl {
    public class SizeSpawnAction : CreatureSpawnAction
    {
        private readonly float sizeMultiplier;

        public SizeSpawnAction(float sizeMultiplier)
        {
            this.sizeMultiplier = sizeMultiplier;
        }

        public override Creature apply(Creature creature)
        {
            var currentScale = creature.transform.localScale;
            creature.transform.localScale = new Vector3(currentScale.x * sizeMultiplier, currentScale.y * sizeMultiplier, currentScale.z * sizeMultiplier);
            return creature;
        }
    }
}
