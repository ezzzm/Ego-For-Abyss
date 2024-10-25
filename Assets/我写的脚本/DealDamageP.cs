using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer; // 指定敌人的图层
    public Transform attackPoint; // 攻击判定的起始位置
    public float attackRange = 0.5f; // 攻击范围半径
    public int damageAmount = 10; // 每次攻击的伤害值

    void DealDamage()
    {
        // 检查攻击范围内的敌人
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount); // 对每个命中的敌人造成伤害
            Debug.Log("Hit " + enemy.name);
        }
    }

    // 可视化攻击范围
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

