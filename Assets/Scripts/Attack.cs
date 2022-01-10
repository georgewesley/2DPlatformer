using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D attackBody;
    [SerializeField] float bulletSpeed;
    PlayerMovement player;
    Transform attackScale;
    int bounces = 0;
    float xSpeed;
    void Start()
    {
        attackBody = GetComponent<Rigidbody2D>();
        attackScale = GetComponent<Transform>();
        player = FindObjectOfType<PlayerMovement>();
        float playerScale = player.transform.localScale.x;
        xSpeed = playerScale * bulletSpeed;
        attackScale.localScale = new Vector3 (playerScale, 1f);
    }

    
    void Update()
    {
        attackBody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground" && bounces < 1)
        {
            bounces += 1;
            attackScale.localScale = new Vector3(-attackScale.localScale.x, attackScale.localScale.y);
            xSpeed = attackScale.localScale.x * bulletSpeed;
        }
        else if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
