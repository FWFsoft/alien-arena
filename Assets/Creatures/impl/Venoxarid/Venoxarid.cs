using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Venoxarid : Creature, Genera
    {
        public string GeneraType { get; } = "Venoxarid";
    }
}
