using Creatures.Api;

using UnityEngine;

namespace Creatures.impl
{
    public abstract class Frigidariant : NonPlayable, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Frigidariant;
    }
}
