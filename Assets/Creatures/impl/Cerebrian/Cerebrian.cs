using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public abstract class Cerebrian : NonPlayable, Genera
    {
        public GeneraType GeneraType { get; } = GeneraType.Cerebrian;
    }
}
