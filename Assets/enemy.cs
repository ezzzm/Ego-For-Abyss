using UnityEngine;

public class Enemyy : MonoBehaviour
{
    public int health = 100; // 主角的生命值

    // 当主角与触发器碰撞时
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) // 确保碰撞的对象是怪物
        {
            TakeDamage(10); // 对主角造成伤害，例如10点
        }
    }

    // 处理受伤逻辑
    public void TakeDamage(int damage)
    {
        health -= damage; // 减少生命值
        Debug.Log("Player took damage! Current health: " + health);

        if (health <= 0)
        {
            Debug.Log("Monster is dead!");
            Destroy(gameObject);
        }
    }
}
