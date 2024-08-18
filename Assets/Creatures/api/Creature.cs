using UnityEngine;

namespace Creatures.Api
{
    public abstract class Creature : MonoBehaviour
    {
        public Animator Animator { get; set; }
        public float MaximumAttackDistance { get; set; } = 100f;
        public float Health { get; set; } = 100;
        public float Speed { get; set; } = 0.5f;
        public bool SpeedEnabled { get; set; } = true;
        public bool IsDead { get; set; } = false;
        public HealthScript HealthScript { get; set; }

        protected virtual void Update()
        {
            if (IsDead || HealthScript.isDead())
            {
                return;
            }

            var target = FindTargetInRange();
            if (target == null)
            {
                Animator.SetBool("isMoving", false);
                Patrol();
            }
            else
            {
                Animator.SetBool("isMoving", true);
                MoveTowardsTarget(target);
            }
        }

        protected abstract void Patrol(); // To be implemented by specific creatures

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
    }
}
