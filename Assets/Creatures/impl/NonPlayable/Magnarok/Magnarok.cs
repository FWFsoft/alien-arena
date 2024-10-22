using Creatures.Api;

using UnityEngine;

namespace Creatures.impl
{
    public abstract class Magnarok : NonPlayable, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Magnarok;
    }
}
