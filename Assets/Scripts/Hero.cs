using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 3f; 
    public float jumpForce = 10f; 
    public float jump; 
    private float jumpsCount; 
    private bool isFacingRight = true;

    public Transform groundCheck; 
    public LayerMask whatIsGraund; 
    public float checkRadius; 
    private bool isGraunded;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        jumpsCount = jump;
    }

    private void Run()
    {
        if (isGraunded) State = States.run;
        //Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Horizontal");
        
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        //transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        if (move > 0 && !isFacingRight)
            
            Flip();
        
        else if (move < 0 && isFacingRight)
            Flip();

        //sprite.flipX = dir.x < 0.0f;
    }

    private void Flip()
    {
        
        isFacingRight = !isFacingRight;
        
        Vector3 theScale = transform.localScale;
        
        theScale.x *= -1;
        
        transform.localScale = theScale;
    }

    private void Jump()
    {
        jumpsCount--;
        
        if(jumpsCount > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (jumpsCount == 0 && isGraunded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        
    }

    private void FixedUpdate()
    {
        //Run();
        if (!isGraunded) State = States.jump;
        isGraunded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGraund);       
    }
    
    private void Update()
    {
        if (isGraunded) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (Input.GetButtonDown("Jump"))
            Jump();

        if (isGraunded == true)
        {
            jumpsCount = jump;
        }
         //Input.GetKeyDown(KeyCode.Space)
    }
}

public enum States
{
    idle,
    run,
    jump
}
