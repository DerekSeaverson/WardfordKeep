using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowmanMovement : MonoBehaviour
{
    public Animator animator;
    public GameObject target;
    public float withinCloseRange;
    public float withinAttackRange;
    public float speed;
    public float attackDelay;
    public LayerMask playerLayer;
    public AudioSource releaseAudio;
    public AudioSource loadAudio;
    public GameObject arrowPrefab;

    bool isAttack = false;
    private bool canAttack = true;
    bool playerInCloseRange = false;
    bool playerInAttackRange = false;
    private Vector2 targetPosition;
    public bool facingRight = true;
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

                animator.SetTrigger("isAttacking");
                Invoke("launchArrow", 1f);

                //minor delay to attack method to match animation
                //Invoke("isAttacking", 0.35f);
            }
            Collider2D[] playerInOverallAttackRange = Physics2D.OverlapCircleAll(transform.position, withinAttackRange, playerLayer); //captures the player if in range of enemy attack
            Collider2D[] playerInOverallRange = Physics2D.OverlapCircleAll(transform.position, withinCloseRange, playerLayer); //captures the player if in range of enemy to start movement
            if (playerInOverallRange.Length > 0)
            {
                Debug.Log("Player detected in bowman Run Away Range");
                playerInCloseRange = true;
            }
            else
            {
                playerInCloseRange = false;
            }
            if (playerInOverallAttackRange.Length > 0)
            {
                Debug.Log("Player Detected in bowman attack range");
                playerInAttackRange = true;
                //call to moveAway method which is in progress of being changed
                //moveAway();
                if (targetPosition.x > transform.position.x && !facingRight)
                    flipDirection();
                if (targetPosition.x < transform.position.x && facingRight)
                    flipDirection();
            }
            else
            {
                playerInAttackRange = false;
            }
        }
    }

    //moveAway in progress of adjusting movement to move away from player after shot if too close before expires and bowman to shoot again
    /*void moveAway()
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
    }*/

    void resetAttack()
    {
        canAttack = true;
        loadAudio.Play(0);
    }

    void flipDirection()
    {

        // Multiply the skeleton's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingRight = !facingRight;
    }

    void launchArrow()
    {
        arrowPrefab.GetComponent<BowManArrowHandler>().facingRight = this.facingRight;
        Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity); //spawns arrow
        releaseAudio.Play(0);
    }
}
