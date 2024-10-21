using UnityEngine;

public enum CharacterState
{
    Idle,      // 空闲
    Moving,    // 移动
    Jumping,   // 跳跃
    Attacking, // 攻击
}

public class PlayerController : MonoBehaviour
{
    public CharacterState currentState = CharacterState.Idle;  // 当前角色状态
    public float moveSpeed = 5f;          // 移动速度
    public float jumpForce = 7f;          // 跳跃力
    public float attackDuration = 0.3f;   // 攻击时长

    private Rigidbody2D rb;               // 刚体组件
    private bool isGrounded;              // 是否在地面上
    private float attackTimeCounter;      // 攻击计时器
    private float moveInput;              // 移动输入

    // 地面检测相关
    public Transform groundCheck;         // 地面检测的位置
    public float groundCheckRadius = 0.2f; // 地面检测半径
    public LayerMask groundLayer;         // 表示地面的图层    
    public Animator Anima;                // 动画控制器

    private bool IsMoving;                // 是否正在移动
    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [Header("Attack info")]
    [SerializeField] private bool IsAttacking;
    [SerializeField] private int ComboCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anima = GetComponentInChildren<Animator>(); // 获取角色的动画
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck Transform is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        HandleInput();     // 处理输入
        HandleState();     // 处理状态切换
        CheckGround();     // 检测角色是否在地面
        UpdateAnimator();  // 更新动画状态
        Dash();            // 处理冲刺逻辑
    }

    public void AttackOver()  // 动画结束后调用，用于重置攻击状态
    {
        IsAttacking = false;  // 结束攻击状态
    }

    // 处理用户输入
    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");  // 获取水平移动输入

        if (dashTime > 0)
        {
            rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // 检查是否按下攻击键 (J键)
        if (Input.GetKeyDown(KeyCode.J) && !IsAttacking)  // 如果未在攻击状态
        {
            IsAttacking = true;  // 设置攻击状态
            currentState = CharacterState.Attacking;
            attackTimeCounter = attackDuration;  // 重置攻击计时器
            ComboCounter++;  // 连击计数递增
            if(ComboCounter == 4)
            {
                ComboCounter = 0;   
            }
        }

        // 检查是否按下跳跃键 (Space)，并且角色必须在地面上
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState != CharacterState.Jumping)
        {
            Jump();  // 进行跳跃
        }
    }

    // 处理角色状态切换
    void HandleState()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
            case CharacterState.Moving:
                if (currentState != CharacterState.Attacking)  // 当处于攻击状态时，不切换到其他状态
                {
                    // 检查是否在移动或空闲
                    currentState = moveInput != 0 ? CharacterState.Moving : CharacterState.Idle;
                }
                break;

            case CharacterState.Attacking:
                attackTimeCounter -= Time.deltaTime;  // 攻击计时
                if (attackTimeCounter <= 0)  // 攻击结束
                {
                    currentState = CharacterState.Idle;  // 攻击完成后回到空闲状态
                    AttackOver();  // 调用结束攻击函数
                }
                break;

            case CharacterState.Jumping:
                if (isGrounded)  // 角色着地后回到空闲或移动状态
                {
                    currentState = moveInput != 0 ? CharacterState.Moving : CharacterState.Idle;
                }
                break;
        }
    }

    // 检测角色是否在地面
    void CheckGround()
    {
        // 使用 Raycast 检测地面，并更新 isGrounded 状态
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    // 处理跳跃逻辑
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // 赋予跳跃力
        isGrounded = false;  // 设置为离开地面
        currentState = CharacterState.Jumping;  // 切换到跳跃状态
    }

    // 更新动画状态
    void UpdateAnimator()
    {
        // 更新移动动画
        IsMoving = Mathf.Abs(moveInput) > 0;
        Anima.SetBool("IsMoving", IsMoving);

        // 更新跳跃动画
        Anima.SetBool("IsGrounded", isGrounded);
        Anima.SetBool("IsDashing", dashTime > 0);
        Anima.SetBool("IsAttacking", IsAttacking);  // 触发攻击动画
        Anima.SetInteger("ComboCounter", ComboCounter);
    }

    // 用于显示地面检测的可视化帮助（方便调试）
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    // 用于显示地面检测的可视化帮助（方便调试）
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);  // 显示地面检测范围
        }
    }

    // 冲刺逻辑
    void Dash()
    {
        dashTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = dashDuration;
        }
    }
}
