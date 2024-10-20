using UnityEngine;

namespace ProjectLightsOut.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Vector2 direction;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.velocity = direction;
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }
    }
}