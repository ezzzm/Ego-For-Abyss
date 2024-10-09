using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private float InputX;
    public float MoveSpeed;
    public Animator Anima;
    private bool IsMoving;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anima = GetComponentInChildren<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(InputX * MoveSpeed, rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, 10);
        }
        IsMoving = rb.velocity.x !=0;
        Anima.SetBool("IsMoving",IsMoving);
    }
}
