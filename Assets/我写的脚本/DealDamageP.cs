using UnityEngine;

public class MonsterCombat : MonoBehaviour
{
    public LayerMask EnemyyLayer; // 指定敌人的图层
    public Transform attackPoint; // 攻击判定的起始位置
    public float attackRange = 0.5f; // 攻击范围半径
    public int damageAmount = 10; // 每次攻击的伤害值

    void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not set!"); // 调试信息
            return;
        }

        // 检查攻击范围内的敌人
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyyLayer);

        foreach (Collider2D Enemyy in hitEnemies)
        {
            if (Enemyy != null) // 确保敌人不为 null
            {
                Enemyy EnemyyComponent = Enemyy.GetComponent<Enemyy>();
                if (EnemyyComponent != null) // 检查是否有 Enemyy 组件
                {
                    EnemyyComponent.TakeDamage(damageAmount); // 对每个命中的敌人造成伤害
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

    // 可视化攻击范围
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
