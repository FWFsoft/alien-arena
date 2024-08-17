using UnityEngine;

namespace Creatures.Api
{
    public abstract class Creature : MonoBehaviour
    {
        public float Speed { get; set; } = 0.5f;
        public bool SpeedEnabled { get; set; } = true;
    }
}
