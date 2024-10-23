using UnityEngine;

public enum CharacterState
{
    Idle,      // ����
    Moving,    // �ƶ�
    Jumping,   // ��Ծ
    Attacking, // ����
}

public class PlayerController : MonoBehaviour
{
    public CharacterState currentState = CharacterState.Idle;  // ��ǰ��ɫ״̬
    public float moveSpeed = 5f;          // �ƶ��ٶ�
    public float jumpForce = 7f;          // ��Ծ��
    public float attackDuration = 0.3f;   // ����ʱ��

    private Rigidbody2D rb;               // �������
    private bool isGrounded;              // �Ƿ��ڵ�����
    private float attackTimeCounter;      // ������ʱ��
    private float moveInput;              // �ƶ�����

    // ���������
    public Transform groundCheck;         // �������λ��
    public float groundCheckRadius = 0.2f; // ������뾶
    public LayerMask groundLayer;         // ��ʾ�����ͼ��    
    public Animator Anima;                // ����������

    private bool IsMoving;                // �Ƿ������ƶ�
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
        Anima = GetComponentInChildren<Animator>(); // ��ȡ��ɫ�Ķ���
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck Transform is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        HandleInput();     // ��������
        HandleState();     // ����״̬�л�
        CheckGround();     // ����ɫ�Ƿ��ڵ���
        UpdateAnimator();  // ���¶���״̬
        Dash();            // �������߼�
    }

    public void AttackOver()  // ������������ã��������ù���״̬
    {
        IsAttacking = false;  // ��������״̬
    }

    // �����û�����
    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");  // ��ȡˮƽ�ƶ�����

        if (dashTime > 0)
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
                ComboCounter = 0;   
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

    // ����ɫ�Ƿ��ڵ���
    void CheckGround()
    {
        // ʹ�� Raycast �����棬������ isGrounded ״̬
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
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
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = dashDuration;
        }
    }
}
