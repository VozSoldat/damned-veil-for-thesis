using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Managers;
using UnityEngine;

namespace ProjectLightsOut.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Vector2 direction;
        private int ricochetCount;
        private float destroyTimer = 10f;
        [SerializeField] private int maxRicochetCount = 3;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            EventManager.Broadcast(new OnProjectileShoot());
        }

        private void Update()
        {
            SelfDestruct();
        }

        private void FixedUpdate()
        {
            rb.velocity = direction;
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckTargetCollision(collision);
        }

        private void CheckTargetCollision(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ricochet"))
            {
                destroyTimer = 10f;
                
                if (ricochetCount < maxRicochetCount)
                {
                    ricochetCount++;
                    direction = Vector2.Reflect(direction, collision.GetContact(0).normal);
                    transform.up = direction;
                }

                else
                {
                    DestroyProjectile();
                }

                EventManager.Broadcast(new CameraShakeEvent(0.05f, 0.05f));
            }

            else if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<IHittable>().OnHit(ricochetCount);
                EventManager.Broadcast(new CameraShakeEvent(0.05f, 0.05f));
                EventManager.Broadcast(new OnSlowTime(0.1f, 0.2f));
            }
        }

        private void SelfDestruct()
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= 0)
            {
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            EventManager.Broadcast(new OnProjectileDestroy());
            Destroy(gameObject);
        }
    }
}