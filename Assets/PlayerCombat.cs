using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("攻击设置")]
    public LayerMask targetLayersp;        // 指定可以攻击的目标图层（如 Player 和 Enemy）
    public Transform attackPointp;         // 攻击判定的起始位置
    public float attackRangep = 0.5f;      // 攻击范围半径
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
        // 播放死亡动画、销毁敌人等逻辑
        Destroy(gameObject);
    }// 每次攻击的伤害值

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当玩家或敌人进入怪物的攻击范围时触发伤害
        if (((1 << collision.gameObject.layer) & targetLayersp) != 0)
        {
            DealDamagepToTarget(collision);
        }
    }

    public void DealDamagep()  // 主动攻击时调用
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPointp.position, attackRangep, targetLayersp);

        foreach (Collider2D target in hitTargets)
        {
            DealDamagepToTarget(target);
        }
    }

    private void DealDamagepToTarget(Collider2D target)
    {
        // 尝试对玩家造成伤害
        PlayerCombat player = target.GetComponent<PlayerCombat>();
        if (player != null)
        {
            player.TakeDamage(damageAmountp);
            Debug.Log("对玩家造成了伤害: " + damageAmountp);
            return;
        }

        // 尝试对敌人造成伤害
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmountp);
            Debug.Log("对敌人造成了伤害: " + damageAmountp);
        }
    }

    // 在 Scene 视图中可视化攻击范围，方便调试
    private void OnDrawGizmosSelected()
    {
        if (attackPointp == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPointp.position, attackRangep);
    }
}
