using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls_Manager : MonoBehaviour
{

    public Animator animator;

    private bool facingRight = true; //variable to hold character's facing direction (starts right)
    float horizontalMove = 0f;
    float runSpeed = 40f;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("isJumping");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Run") && Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("isDash");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("isAttacking");
        }
        if (horizontalMove * Time.deltaTime > 0 && !facingRight)
        {
            Flip();
        } else if (horizontalMove * Time.deltaTime < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
