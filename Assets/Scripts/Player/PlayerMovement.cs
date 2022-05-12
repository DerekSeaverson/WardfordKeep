using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public AudioSource attackAudio;
    public AudioSource deathAudio;
    public AudioSource takeHitAudio;
    public AudioSource shieldAudio;
    public PlayerHealth playerHealthBar;
    public Transform playerTrans;
    public Rigidbody2D playerRigid;

    public LayerMask enemiesLayer;

    public float lastAttack = 0f;
    float attackDelay = 0.9f;
    public int swordDamage = 35;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    public bool jump = false;
    public bool attack = false;
    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //key management handler
        playerTrans.position = playerRigid.position;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Run") && Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Dash");
            runSpeed = 70f;
            Invoke("resetSpeed", 0.75f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //takeDamage(10);
        }


        if (Time.time > lastAttack + attackDelay)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                lastAttack = Time.time;
                attackAudio.Play(0);
                animator.SetTrigger("isAttacking");
                //minor delay to attack method to match animation
                Invoke("isAttacking", 0.35f);
            }
        }
    }

    private void resetSpeed()
    {
        runSpeed = 40f;
    }

    private void isAttacking()
    {
        //combat management
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            //detects status of enemy before dealing damage
            if (!enemy.GetComponent<Animator>().GetBool("isDead"))
            {
                //detection of shield skeleton in case skeleton is blocking attack
                if (enemy.name == "Shield_Skeleton")
                {
                    if (!enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Shield_Block"))
                    {
                        //Debug.Log("Shield Test working");
                        enemy.GetComponent<EnemyManagement>().takeDamage(35);
                    }  else
                    {
                        shieldAudio.Play(0);
                    }
                } 
                else
                {
                    enemy.GetComponent<EnemyManagement>().takeDamage(35);
                }
            }
        }
    }

    public void OnLanding()
    {
        //jump landing handler
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        //Character movement
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void takeDamage(int damage)
    {
        if (takeHitAudio != null)
        {
            takeHitAudio.Play(0);
        }
        currentHealth -= damage;
        animator.SetTrigger("takeHit");
        playerHealthBar.SetPlayerHealth(currentHealth);
        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            if (deathAudio != null)
            {
                deathAudio.Play(0);
                Invoke("GoToLoseScreen", 2f); 
            }
        }
    }

    private void GoToLoseScreen()
    {
        SceneManager.LoadScene("Lose Screen");
    }
}
