using UnityEngine;

public class MonsterCombat : MonoBehaviour
{
    public LayerMask EnemyyLayer; // ָ�����˵�ͼ��
    public Transform attackPoint; // �����ж�����ʼλ��
    public float attackRange = 0.5f; // ������Χ�뾶
    public int damageAmount = 10; // ÿ�ι������˺�ֵ

    void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not set!"); // ������Ϣ
            return;
        }

        // ��鹥����Χ�ڵĵ���
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyyLayer);

        foreach (Collider2D Enemyy in hitEnemies)
        {
            if (Enemyy != null) // ȷ�����˲�Ϊ null
            {
                Enemyy EnemyyComponent = Enemyy.GetComponent<Enemyy>();
                if (EnemyyComponent != null) // ����Ƿ��� Enemyy ���
                {
                    EnemyyComponent.TakeDamage(damageAmount); // ��ÿ�����еĵ�������˺�
                    Debug.Log("Hit " + Enemyy.name);
                }
                else
                {
                    Debug.LogWarning("Hit object does not have an Enemyy component: " + Enemyy.name);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage();
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
