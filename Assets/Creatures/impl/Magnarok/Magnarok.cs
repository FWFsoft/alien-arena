using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Magnarok : Creature, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Magnarok;
    }
}
