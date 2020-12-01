using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] float moveSpeed;

    // Cached Reference
    Rigidbody2D enemyRigidbody;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        FlipSprite();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        animator.SetBool("isWalking", true);
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        moveSpeed *= -1;
        FlipSprite();
    }


    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed), 1f);
    }
}
