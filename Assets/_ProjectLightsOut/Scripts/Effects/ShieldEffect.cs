using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D col2D;
    private bool isShieldActive = false;
    private float shieldDuration = 1f;
    private Coroutine shieldCoroutine;

    private void Awake()
    {
        spriteRenderer.enabled = false;
        animator.enabled = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        col2D.enabled = false;
    }

    private void Update()
    {
        if (isShieldActive)
        {
            shieldDuration -= Time.deltaTime;
            if (shieldDuration <= 0)
            {
                DeactivateShield();
            }
        }
    }

    public void ActivateShield()
    {
        shieldDuration = 1.5f;
        if (isShieldActive) return;
        isShieldActive = true;
        spriteRenderer.enabled = true;
        animator.enabled = true;
        col2D.enabled = true;

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldUpAnimation());
    }

    public void DeactivateShield()
    {
        if (!isShieldActive) return;
        isShieldActive = false;
        col2D.enabled = false;
        
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldDownAnimation());
    }

    private IEnumerator ShieldUpAnimation()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator ShieldDownAnimation()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.enabled = false;
        animator.enabled = false;
    }
}
