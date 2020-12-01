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
    int jumpPressedTimes;

    // Constants
    const int INITIAL_JUMP_VALUE = 1;
    const int MAX_JUMP_VALUE = 2;

    //Cached references
    Rigidbody2D playerRigidBody;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        Jump();
        FlipSprite();
        Die();
    }

    public void Run()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
        Vector2 playerVelocity = new Vector2(deltaX, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
    }

    public void Jump()
    {
        if (!IsGrounded() && jumpPressedTimes >= MAX_JUMP_VALUE) { return; }
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            playerRigidBody.velocity += jumpVelocityToAdd;
            jumpPressedTimes++;
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
        if (playerHasHorizontalSpeed && IsGrounded())
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
            jumpPressedTimes = INITIAL_JUMP_VALUE;
        }
        else if (!IsGrounded())
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);

            if (jumpPressedTimes == MAX_JUMP_VALUE)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isDoubleJumping", true);
            }
        }
        else
        {
            jumpPressedTimes = INITIAL_JUMP_VALUE;
            animator.SetBool("isDoubleJumping", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
    }

    private bool IsGrounded()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Die");
        }

    }
}