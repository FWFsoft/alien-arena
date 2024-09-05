using UnityEngine;
using System.Collections.Generic;

namespace Creatures.Api
{
    public abstract class Creature : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float maximumAttackDistance = 100f;

        [SerializeField]
        private float health = 100f;

        [SerializeField]
        private float speed = 0.5f;

        [SerializeField]
        private bool speedEnabled = true;

        [SerializeField]
        private bool isDead = false;

        [SerializeField]
        private HealthScript healthScript;
        

        // Public properties to encapsulate the fields
        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public float MaximumAttackDistance
        {
            get => maximumAttackDistance;
            set => maximumAttackDistance = value;
        }

        public float Health
        {
            get => health;
            set => health = value;
        }

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public bool SpeedEnabled
        {
            get => speedEnabled;
            set => speedEnabled = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        public HealthScript HealthScript
        {
            get => healthScript;
            set => healthScript = value;
        }
    }
}
