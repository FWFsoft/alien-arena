using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Frigidariant : NonPlayable, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Frigidariant;
    }
}
