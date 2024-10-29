using UnityEngine;

public class Enemyy : MonoBehaviour
{
    public int health = 100; // ���ǵ�����ֵ

    // �������봥������ײʱ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) // ȷ����ײ�Ķ����ǹ���
        {
            TakeDamage(10); // ����������˺�������10��
        }
    }

    // ���������߼�
    public void TakeDamage(int damage)
    {
        health -= damage; // ��������ֵ
        Debug.Log("Player took damage! Current health: " + health);

        if (health <= 0)
        {
            Debug.Log("Monster is dead!");
            Destroy(gameObject);
        }
    }
}
