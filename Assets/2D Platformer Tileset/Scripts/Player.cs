using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpSpeed;

    Rigidbody2D rigidbody2D;
    Animator animator;

    CapsuleCollider2D myBodyCollider;
    
    bool isAlive = true;
    bool bJump = false;

    float jumpElapsedTime = 0;

    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        // This condition prevents from doing a double Jump;
        if(!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        //if (bJump && myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //{
        //    // We have landed on Ground after the jump so get to idle state;
        //    bJump = false;
        //    animator.SetBool("Jump", bJump);
        //}

        if (value.isPressed)
        {
            // Do stuff
            rigidbody2D.velocity += new Vector2(0, jumpSpeed);
            bJump = true;
            animator.SetBool("Jump", bJump);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        if (bJump) //&& myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            jumpElapsedTime += Time.deltaTime;
            
            if(jumpElapsedTime > 0.2f && myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                // We have landed on Ground after the jump so get to idle state;
                bJump = false;
                animator.SetBool("Jump", bJump); 
                jumpElapsedTime = 0f;
            }
            
        }

        //FlipSprite();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * movementSpeed, rigidbody2D.velocity.y);
        rigidbody2D.velocity = playerVelocity;

        // Mathf.Abs(moveInput.x) > 0f; or we can write Mathf.Abs(moveInput.x) > Mathf.Epsilon
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
        
        if (playerHasHorizontalSpeed)
        {
            // This will flip the sprite according to moveInput.
            // moveInput is always either +1/-1 based on key input.
            transform.localScale = new Vector2(moveInput.x, 1f);
        }
    }

    void FlipSprite()
    {
        // Mathf.epsilon means its almost equal to zero and good/better for comparison of float with zero. 
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;

        
    }
}
