using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Scropio : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private bool isAttacking;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (!isAttacking)
            {
               StartCoroutine(AttackCoroutine());
            }

            return;
        }
        else
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position, transform.position);
            if(distance > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCoolDown);
        }

        isAttacking = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerMask);
        foreach(Collider2D hit in  hitPlayer)
        {
            Debug.Log("Attacking the Player");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        speed = -speed;
        Flip();
    }

    void Flip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.linearVelocity.x)), 1f);
    }
}
