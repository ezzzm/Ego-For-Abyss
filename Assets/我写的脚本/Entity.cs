using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    protected Animator Anima;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anima = GetComponent<Animator>();   
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
