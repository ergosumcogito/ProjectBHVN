using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Core.Enemy_Logic;

public class AutoAim : MonoBehaviour
{
    
    private PlayerRuntimeStats stats;

    private float AttackRange => stats != null ? stats.FinalAttackRange : 5f;
    
    private void Awake()
    {
        stats = GetComponentInParent<PlayerRuntimeStats>();
    }
    
    public Transform GetClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            AttackRange
        );

        EnemyAbstract closestEnemy = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            EnemyAbstract enemy = hit.GetComponent<EnemyAbstract>();
            if (enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy?.transform;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}