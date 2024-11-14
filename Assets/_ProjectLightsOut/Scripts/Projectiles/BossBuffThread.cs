using System;
using System.Collections;
using System.Collections.Generic;
using ProjectLightsOut.DevUtils;
using UnityEngine;

public enum BuffType
{
    Health,
    Shield
}

public class OnBossBuff : GameEvent
{
    public BuffType buffType;

    public OnBossBuff(BuffType buffType)
    {
        this.buffType = buffType;
    }
}

public class BossBuffThread : MonoBehaviour
{
    private Transform targetTransform;
    [SerializeField] private BuffType buffType;
    public Action OnThreadDestroyed;

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, 1f * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, targetTransform.position - transform.position);

        if (Vector2.Distance(transform.position, targetTransform.position) < 0.1f)
        {
            EventManager.Broadcast(new OnBossBuff(buffType));
            Destroy(gameObject);
        }
    }
}
