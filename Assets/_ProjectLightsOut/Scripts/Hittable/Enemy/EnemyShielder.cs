using System.Collections;
using System.Collections.Generic;
using ProjectLightsOut.DevUtils;
using UnityEngine;

public class EnemyShielder : EnemyHealer
{
    protected override IEnumerator Buffing()
    {
        while (true)
        {
            chantEffectAnimator.SetTrigger("Buff");

            EventManager.Broadcast(new OnBossBuff(BuffType.Shield));

            yield return new WaitForSeconds(1f);
        }
    }
}
