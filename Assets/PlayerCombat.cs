using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("��������")]
    public LayerMask targetLayersp;        // ָ�����Թ�����Ŀ��ͼ�㣨�� Player �� Enemy��
    public Transform attackPointp;         // �����ж�����ʼλ��
    public float attackRangep = 0.5f;      // ������Χ�뾶
    public int damageAmountp = 10;
    public int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage.");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // �����������������ٵ��˵��߼�
        Destroy(gameObject);
    }// ÿ�ι������˺�ֵ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����һ���˽������Ĺ�����Χʱ�����˺�
        if (((1 << collision.gameObject.layer) & targetLayersp) != 0)
        {
            DealDamagepToTarget(collision);
        }
    }

    public void DealDamagep()  // ��������ʱ����
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPointp.position, attackRangep, targetLayersp);

        foreach (Collider2D target in hitTargets)
        {
            DealDamagepToTarget(target);
        }
    }

    private void DealDamagepToTarget(Collider2D target)
    {
        // ���Զ��������˺�
        PlayerCombat player = target.GetComponent<PlayerCombat>();
        if (player != null)
        {
            player.TakeDamage(damageAmountp);
            Debug.Log("�����������˺�: " + damageAmountp);
            return;
        }

        // ���ԶԵ�������˺�
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmountp);
            Debug.Log("�Ե���������˺�: " + damageAmountp);
        }
    }

    // �� Scene ��ͼ�п��ӻ�������Χ���������
    private void OnDrawGizmosSelected()
    {
        if (attackPointp == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPointp.position, attackRangep);
    }
}
