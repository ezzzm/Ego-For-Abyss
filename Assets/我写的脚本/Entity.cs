using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator Anima;
    protected Rigidbody2D rb;
    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheckPoint;

    protected bool isGrounded;

    protected int facingDir = 1;
    protected bool facingRight = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anima = GetComponentInChildren<Animator>();   
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckGround();
    }
    protected virtual void CheckGround()
    {
        // 使用 Raycast 检测地面，并更新 isGrounded 状态
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    protected virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;

        // 翻转角色的x轴来改变朝向
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(groundCheckPoint.position, new Vector3(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckDistance));
    }
}
