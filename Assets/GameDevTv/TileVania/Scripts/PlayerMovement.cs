using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TileVania
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 2f;
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float climbSpeed = 5f;
        [SerializeField] Vector2 deathKick = new Vector2(5f, 5f);

        [SerializeField] Transform gun;

        [Header("Prefabs")]
        [SerializeField] GameObject bulletPrefab;

        Rigidbody2D rb;

        float initialGravity;

        CapsuleCollider2D myBodyCollider;
        BoxCollider2D myFeetCollider;
        Animator animator;

        bool isAlive = true;

        Vector2 moveInput;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            myBodyCollider = GetComponent<CapsuleCollider2D>();
            myFeetCollider = GetComponent<BoxCollider2D>();
            initialGravity = rb.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isAlive)
            {
                return;
            }
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }

        void OnMove(InputValue value)
        {
            if (!isAlive)
            {
                return;
            }
            moveInput = value.Get<Vector2>();
            //Debug.Log(moveInput);
        }

        void OnJump(InputValue value)
        {
            if (!isAlive)
            {
                return;
            }

            if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                return;
            }

            if (value.isPressed)
            {
                // Do stuff
                rb.velocity += new Vector2(0, jumpSpeed);
            }
        }

        void OnFire(InputValue value)
        {
            if (!isAlive)
            {
                return;
            }

            GameObject go = Instantiate(bulletPrefab, gun.position, transform.rotation);

        }


        void Run()
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * movementSpeed, rb.velocity.y);
            rb.velocity = playerVelocity;

            bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
            animator.SetBool("isRunning", playerHasHorizontalSpeed);
        }

        void FlipSprite()
        {
            // Mathf.epsilon means its almost equal to zero and good/better for comparison of float with zero. 
            bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

            if (playerHasHorizontalSpeed)
            {
                transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
            }
        }

        void ClimbLadder()
        {
            if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
            {
                rb.gravityScale = initialGravity;
                animator.SetBool("isClimbing", false);
                return;
            }

            rb.gravityScale = 0f;
            Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            rb.velocity = climbVelocity;

            bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", playerHasVerticalSpeed);
        }

        void Die()
        {
            if (rb.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
            {
                isAlive = false;
                animator.SetTrigger("Dying");
                rb.velocity = deathKick;
                GameSession.instance.ProcessPlayerDeath();
            }
        }
    }
}

