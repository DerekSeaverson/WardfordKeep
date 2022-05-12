using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{

    public Animator animator;
    public AudioSource takeHitAudio;
    public AudioSource onDeathAudio;
    public EnemyHealth enemyHealth;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 100;
        enemyHealth.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int hitDamage)
    {
        if (takeHitAudio != null)
        {
            takeHitAudio.Play(0);
        }
        currentHealth -= hitDamage;
        enemyHealth.SetEnemyHealth(currentHealth);
        animator.SetTrigger("takeHit");
        if (currentHealth <= 0)
        {
            onDeath();
        }
    }

    void onDeath()
    {
        if (onDeathAudio != null)
        {
            onDeathAudio.Play(0);
        }
        animator.SetTrigger("isDead");
        Debug.Log("Enemy Died");

        //stops collision for enemy
        this.enabled = false;
    }
}
