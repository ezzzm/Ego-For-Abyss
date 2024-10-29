using Unity.VisualScripting;
using UnityEngine;

public enum CharacterState
{
    Idle,      // ����
    Moving,    // �ƶ�
    Jumping,   // ��Ծ
    Attacking, // ����
}

public class PlayerController : Entity
{
    public CharacterState currentState = CharacterState.Idle;  // ��ǰ��ɫ״̬
    public float moveSpeed = 5f;          // �ƶ��ٶ�
    public float jumpForce = 7f;          // ��Ծ��
    public float attackDuration = 0.3f;   // ����ʱ��
    private float attackTimeCounter;      // ������ʱ��
    private float moveInput;              // �ƶ�����

    // ���������
    public Transform groundCheck;         // �������λ��
    public float groundCheckRadius = 0.2f; // ������뾶
    public LayerMask groundLayer;         // ��ʾ�����ͼ��    

    private bool IsMoving;                // �Ƿ������ƶ�

    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [Header("Attack info")]
    [SerializeField] private bool IsAttacking;
    [SerializeField] private int ComboCounter;
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
    }

    protected override void Start()
    {
       base.Start();
    }

    protected override void Update()
    {
        base.Update();
        HandleInput();     // ��������
        HandleState();     // ����״̬�л�
        UpdateAnimator();  // ���¶���״̬
        Dash();            // �������߼�      
        FlipController();
    }

    public void AttackOver()  // ������������ã��������ù���״̬
    {
        IsAttacking = false;  // ��������״̬
    }

    // �����û�����
    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");  // ��ȡˮƽ�ƶ�����

        if (dashTime > 0 )
        {
            rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // ����Ƿ��¹����� (J��)
        if (Input.GetKeyDown(KeyCode.J) && !IsAttacking)  // ���δ�ڹ���״̬
        {
            IsAttacking = true;  // ���ù���״̬
            currentState = CharacterState.Attacking;
            attackTimeCounter = attackDuration;  // ���ù�����ʱ��
            ComboCounter++;  // ������������
            if(ComboCounter == 4)
            {
                ComboCounter = 1;   
            }
        }

        // ����Ƿ�����Ծ�� (Space)�����ҽ�ɫ�����ڵ�����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState != CharacterState.Jumping)
        {
            Jump();  // ������Ծ
        }
    }

    // �����ɫ״̬�л�
    void HandleState()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
            case CharacterState.Moving:
                if (currentState != CharacterState.Attacking)  // �����ڹ���״̬ʱ�����л�������״̬
                {
                    // ����Ƿ����ƶ������
                    currentState = moveInput != 0 ? CharacterState.Moving : CharacterState.Idle;
                }
                break;

            case CharacterState.Attacking:
                attackTimeCounter -= Time.deltaTime;  // ������ʱ
                if (attackTimeCounter <= 0)  // ��������
                {
                    currentState = CharacterState.Idle;  // ������ɺ�ص�����״̬
                    AttackOver();  // ���ý�����������
                }
                break;

            case CharacterState.Jumping:
                if (isGrounded)  // ��ɫ�ŵغ�ص����л��ƶ�״̬
                {
                    currentState = moveInput != 0 ? CharacterState.Moving : CharacterState.Idle;
                }
                break;
        }
    }

    // ������Ծ�߼�
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // ������Ծ��
        isGrounded = false;  // ����Ϊ�뿪����
        currentState = CharacterState.Jumping;  // �л�����Ծ״̬
    }

    // ���¶���״̬
    void UpdateAnimator()
    {
        // �����ƶ�����
        IsMoving = Mathf.Abs(moveInput) > 0;
        Anima.SetBool("IsMoving", IsMoving);

        // ������Ծ����
        Anima.SetBool("IsGrounded", isGrounded);
        Anima.SetBool("IsDashing", dashTime > 0);
        Anima.SetBool("IsAttacking", IsAttacking);  // ������������
        Anima.SetInteger("ComboCounter", ComboCounter);
    }

    // ������ʾ������Ŀ��ӻ�������������ԣ�
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);  // ��ʾ�����ⷶΧ
        }
    }

    // ����߼�
    void Dash()
    {
        dashTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift)&&dashTime<3)
        {
            dashTime = dashDuration;
        }
    }
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip(); 
    }
}
