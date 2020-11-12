using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;


    // Player State
    bool isAlive = true;
    bool isGrounded = true;

    //Cached references
    Rigidbody2D playerRigidBody;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
    }

    public void Run()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
        Vector2 playerVelocity = new Vector2(deltaX, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            playerRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
        DetectAnimationState(playerHasHorizontalSpeed);
    }

    private void DetectAnimationState(bool playerHasHorizontalSpeed)
    {
        if (playerHasHorizontalSpeed && isGrounded)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", false);
        }
        else if (!isGrounded)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D otherCollision)
    {
        if (otherCollision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D otherCollision)
    {
        if (otherCollision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = false;
        }
    }
}