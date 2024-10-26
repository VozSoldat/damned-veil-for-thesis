using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLightsOut.Effects
{
    public class Effect : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        private void Start()
        {
            StartCoroutine(DestroyWhenAnimationEnds());
        }

        private IEnumerator DestroyWhenAnimationEnds()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }
}
