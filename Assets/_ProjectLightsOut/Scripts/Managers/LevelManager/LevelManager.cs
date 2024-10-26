using System.Collections;
using System.Collections.Generic;
using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Hittable;
using UnityEngine;

namespace ProjectLightsOut.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDataSO levelData;
        private List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> Enemies { get => enemies; }
        private List<Enemy> deadEnemies = new List<Enemy>();
        public List<Enemy> DeadEnemies { get => deadEnemies; }
        private int activeProjectiles = 0;
        private bool isLevelComplete = false;

        private void OnEnable()
        {
            if (levelData == null)
            {
                Debug.LogError("LevelManager: LevelDataSO is not set.");
            }
            
            EventManager.AddListener<OnEnemyRegister>(OnEnemyRegister);
            EventManager.AddListener<OnEnemyDead>(OnEnemyDead);
            EventManager.AddListener<OnProjectileShoot>(OnProjectileShoot);
            EventManager.AddListener<OnProjectileDestroy>(OnProjectileDestroy);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<OnEnemyRegister>(OnEnemyRegister);
            EventManager.RemoveListener<OnEnemyDead>(OnEnemyDead);
            EventManager.RemoveListener<OnProjectileShoot>(OnProjectileShoot);
            EventManager.RemoveListener<OnProjectileDestroy>(OnProjectileDestroy);
        }

        private void OnEnemyRegister(OnEnemyRegister evt)
        {
            enemies.Add(evt.Enemy);
            Debug.Log($"LevelManager: Enemy registered. Total enemies remaining: {enemies.Count}");
        }

        private void OnEnemyDead(OnEnemyDead evt)
        {
            enemies.Remove(evt.Enemy);
            deadEnemies.Add(evt.Enemy);
            
            Debug.Log($"LevelManager: Enemy dead. Total enemies remaining: {enemies.Count}");
            CheckAllEnemiesDead();
        }

        private void OnProjectileShoot(OnProjectileShoot evt)
        {
            activeProjectiles++;
            Debug.Log($"LevelManager: Projectile shot. Total active projectiles: {activeProjectiles}");
        }

        private void OnProjectileDestroy(OnProjectileDestroy evt)
        {
            activeProjectiles--;
            Debug.Log($"LevelManager: Projectile destroyed. Total active projectiles: {activeProjectiles}");

            if (activeProjectiles == 0 && isLevelComplete)
            {
                EventManager.Broadcast(new OnLevelComplete());
            }
        }

        private void CheckAllEnemiesDead()
        {
            if (enemies.Count == 0)
            {
                if (levelData.Waves.Count > 0)
                {
                    print("LevelManager: All enemies dead. Loading next wave.");
                }
                else
                {
                    EventManager.Broadcast(new OnSlowTime(0.1f, 1.2f));
                    isLevelComplete = true;
                }
            }
        }
    }
}