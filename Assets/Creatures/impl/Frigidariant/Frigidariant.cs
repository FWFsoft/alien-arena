using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Frigidariant : Creature, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Frigidariant;
    }
}
