using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{

    public Animator animator;
    public GameObject target;
    public float withinRange;
    public float withinAttackRange;
    public float attackRange;
    public float speed;
    public float attackDelay;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public AudioSource attackAAudio;
    public AudioSource attackBAudio;

    bool isAttack = false;
    private bool canAttack = true;
    bool playerInRange = false;
    bool playerInAttackRange = false;
    private Vector2 targetPosition;
    private bool facingRight = true;
    private bool isDead = false;
    // Start is called before the first frame update
    void Awake()
    {
        //actions on object awake
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isDead"))
        {
            isDead = true;
        }
        targetPosition = new Vector2(target.transform.position.x, transform.position.y); //tracks player position
        if (!isDead)
        {
            //attack delay management
            if (canAttack && playerInAttackRange && !isDead)
            {
                isAttack = true;
                canAttack = false;
                Invoke("resetAttack", attackDelay);

                //randomely chooses what attack to perform
                int rndNum = Random.Range(1, 3);
                if (rndNum == 1)
                {
                    animator.SetTrigger("attackA");
                    attackAAudio.Play(0);
                }
                else
                {
                    animator.SetTrigger("attackB");
                    attackBAudio.Play(0);
                }
                //minor delay to attack method to match animation
                Invoke("isAttacking", 0.35f);
            }

            //detects when player is within attack range or within range for movement
            /*var distance = Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.y), new Vector2(transform.position.x, transform.position.y));
            if (distance < withinAttackRange)
            {
                Debug.Log("Player is attack range");
                playerInAttackRange = true;
            }
            else if (distance < withinRange)
            {
                Debug.Log("Player is close");
                playerInAttackRange = false;

                var delta = target.transform.position - transform.position;
                delta.Normalize();

                var moveSpeed = speed * Time.deltaTime;

                transform.position = transform.position + (delta * moveSpeed);
                animator.SetFloat("Speed", Mathf.Abs(moveSpeed));
            } else
            {
                animator.SetFloat("Speed", 0f);
                playerInAttackRange = false;
                playerInRange = false;
            }*/
            Collider2D[] playerInOverallAttackRange = Physics2D.OverlapCircleAll(transform.position, withinAttackRange, playerLayer); //captures the player if in range of enemy attack
            Collider2D[] playerInOverallRange = Physics2D.OverlapCircleAll(transform.position, withinRange, playerLayer); //captures the player if in range of enemy to start movement
            if (playerInOverallAttackRange.Length > 0)
            {
                //Debug.Log("Player detected in Attack Range");
                playerInAttackRange = true;
                /*if (target.transform.position.x > transform.position.x && !facingRight)
                    flipDirection();
                if (target.transform.position.x < transform.position.x && facingRight)
                    flipDirection();*/
            }
            else
            {
                playerInAttackRange = false;
            }
            if (playerInOverallRange.Length > 0)
            {

                //Debug.Log("Player Detected in overall range");
                playerInRange = true;
                moveTowards();
                if (targetPosition.x > transform.position.x && !facingRight)
                    flipDirection();
                if (targetPosition.x < transform.position.x && facingRight)
                    flipDirection();
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    void moveTowards()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attackA") && !animator.GetCurrentAnimatorStateInfo(0).IsName("attackB") && !playerInAttackRange && playerInRange)
        {
            //move towards player
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }


    void isAttacking()
    {
        //attack management
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer); //captures the player if in radius of attack
        foreach (Collider2D player in hitPlayer)
        {
            //detects status of enemy before dealing damage
            if (!player.GetComponent<Animator>().GetBool("isDead"))
            {
                player.GetComponent<PlayerMovement>().takeDamage(10);
            }
        }
        isAttack = false;
    }

    void resetAttack()
    {
        canAttack = true;
    }

    void flipDirection()
    {

        // Multiply the skeleton's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingRight = !facingRight;
    }
}
