using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManArrowHandler : MonoBehaviour
{

    public BowmanMovement bowMan;
    public float travelSpeed;

    public bool facingRight = true;
    bool canMove = true; 
    // Start is called before the first frame update
    void Awake()
    {
        if (!facingRight)
        {
            transform.localScale *= -1;
            travelSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position = new Vector2(transform.position.x + (travelSpeed * Time.deltaTime), transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //stops movement or destroyus object on certain collisions
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().takeDamage(5);
            Destroy(gameObject);
        } else if (other.gameObject.name == "Platform Tilemap")
        {
            canMove = false;
            Invoke("destroyObjectDelay", 1.5f);
        }
    }

    void destroyObjectDelay()
    {
        Destroy(gameObject);
    }
}
