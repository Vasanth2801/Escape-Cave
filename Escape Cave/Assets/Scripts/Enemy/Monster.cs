using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private bool isAttacking;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player  = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceFromPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if(!isAttacking)
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
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, checkRadius, playerLayer);
        foreach(Collider2D hit in hitPlayer)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if(ph != null)
            {
                ph.TakeDamage(5);
            }
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