using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
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
        // ²¥·ÅËÀÍö¶¯»­¡¢Ïú»ÙµÐÈËµÈÂß¼­
        Destroy(gameObject);
    }
}

