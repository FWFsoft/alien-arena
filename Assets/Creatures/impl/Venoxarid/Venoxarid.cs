using Creatures.Api;

using UnityEngine;

namespace Creatures.impl
{
    public abstract class Venoxarid : NonPlayable, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Venoxarid;
    }
}
