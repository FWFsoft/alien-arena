using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Cerebrian : Creature, Genera
    {
        public string GeneraType { get; } = "Cerebrian";
    }
}
