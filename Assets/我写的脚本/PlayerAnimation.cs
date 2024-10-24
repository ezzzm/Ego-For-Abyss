using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void AnimationTrigger()
    {
        player.AttackOver(); // 结束攻击时调用
    }
}
