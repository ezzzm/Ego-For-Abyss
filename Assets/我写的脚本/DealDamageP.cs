using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer; // ָ�����˵�ͼ��
    public Transform attackPoint; // �����ж�����ʼλ��
    public float attackRange = 0.5f; // ������Χ�뾶
    public int damageAmount = 10; // ÿ�ι������˺�ֵ

    void DealDamage()
    {
        // ��鹥����Χ�ڵĵ���
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount); // ��ÿ�����еĵ�������˺�
            Debug.Log("Hit " + enemy.name);
        }
    }

    // ���ӻ�������Χ
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

