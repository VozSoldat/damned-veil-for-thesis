using System;
using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Managers;
using UnityEngine;

namespace ProjectLightsOut.Hittable
{
    public class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] private int health = 1;
        public int Health { get => health; }
        public Action<int> OnDamaged;

        public void OnHit(int damage)
        {
            health--;
            OnDamaged?.Invoke(damage);
            
            if (health <= 0)
            {
                EventManager.Broadcast(new OnEnemyDead(this));
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            EventManager.Broadcast(new OnEnemyRegister(this));
        }
    }
}