using System;
using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Managers;
using UnityEngine;

namespace ProjectLightsOut.Hittable
{
    public class Enemy : MonoBehaviour, IHittable
    {
        public bool IsHittable { get; private set; } = true;
        [SerializeField] private int health = 1;
        public int Health { get => health; }
        public Action<int> OnDamaged;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            EventManager.Broadcast(new OnEnemyRegister(this));
        }

        public void OnHit(int damage)
        {
            if (!IsHittable) return;

            health--;
            OnDamaged?.Invoke(damage);
            
            if (health <= 0)
            {
                IsHittable = false;
                EventManager.Broadcast(new OnEnemyDead(this));
                animator.SetTrigger("dead");
            }
        }
    }
}