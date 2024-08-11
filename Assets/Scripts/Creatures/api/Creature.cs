using UnityEngine;

namespace Creatures.Api
{
    public abstract class Creature : MonoBehaviour
    {
        public float Speed { get; set; } = 3.0f;
        public bool SpeedEnabled { get; set; } = true;
    }
}
