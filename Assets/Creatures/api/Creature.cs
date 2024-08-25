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

        [SerializeField]
        private List<Transform> patrolPoints = new List<Transform>();

        private int currentPatrolIndex = 0;

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

        public List<Transform> PatrolPoints
        {
            get => patrolPoints;
            set => patrolPoints = value;
        }

        protected virtual void Update()
        {
           
            if (HealthScript.isDead())
            {
                return;
            }
            var target = findPlayerInRange();
            if(target == null)
            {
                Animator.SetBool("isMoving", false);
                // Add patrol in here
            } else
            {
                // Move towards player
                // TODO: Add an attack when a certain distance away
                Animator.SetBool("isMoving", true);
                var isLeft = target.transform.position.x < transform.position.x;

                this.transform.rotation = Quaternion.Euler(new Vector3(0f, isLeft ? 180f : 0f, 0f));
                var step = Speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            }
        }

        protected virtual void Patrol()
        {
            if (PatrolPoints == null || PatrolPoints.Count == 0)
            {
                return; // No patrol points defined
            }

            // Get the current patrol point
            Transform patrolTarget = PatrolPoints[currentPatrolIndex];

            // Move towards the patrol point
            MoveTowardsTarget(patrolTarget.gameObject);

            // Check if the creature has reached the patrol point
            if (Vector3.Distance(transform.position, patrolTarget.position) < 0.1f)
            {
                // Go to the next patrol point, looping back to the start if necessary
                currentPatrolIndex = (currentPatrolIndex + 1) % PatrolPoints.Count;
            }
        }

        protected virtual void MoveTowardsTarget(GameObject target)
        {
            var isLeft = target.transform.position.x < transform.position.x;

            this.transform.rotation = Quaternion.Euler(new Vector3(0f, isLeft ? 180f : 0f, 0f));
            var step = Speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        protected virtual GameObject FindTargetInRange()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            GameObject closestPlayer = null;
            float closestDistance = MaximumAttackDistance;
            foreach (GameObject player in players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPlayer = player;
                }
            }
            return closestPlayer;
        }
        private GameObject findPlayerInRange()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            GameObject closestPlayer = null;
            float closestDistance = MaximumAttackDistance;
            foreach (GameObject player in players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPlayer = player;
                }
            }
            return closestPlayer;
        }
    }
}
