using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D attackBody;
    SpriteRenderer sprite;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject explosion;
    ParticleSystem.MainModule settings;
    PlayerMovement player;
    Transform attackScale;

    public int bounces = 0;
    float xSpeed;
    void Start()
    {
        attackBody = GetComponent<Rigidbody2D>();
        attackScale = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>();
        float playerScale = player.transform.localScale.x;
        xSpeed = playerScale * bulletSpeed;
        attackScale.localScale = new Vector3 (playerScale, 1f);
        settings = explosion.GetComponentInChildren<ParticleSystem>().main;
    }

    
    void Update()
    {
        attackBody.velocity = new Vector2(xSpeed, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground" && bounces < 1)
        {
            sprite.color = new Color(0, 1, 1, 1);
            Debug.Log(xSpeed);
            xSpeed = xSpeed/2;
            Debug.Log(xSpeed);
            bounces += 1;
            attackScale.localScale = new Vector3(-attackScale.localScale.x, attackScale.localScale.y);
            xSpeed = attackScale.localScale.x * bulletSpeed;
        }
        else if(collision.gameObject.tag == "Attack") {
            if(bounces == 1) {
                settings.startColor = new Color (0, 1, 1, 1);
            }
            else {
                settings.startColor = new Color(1,1,1,1);
            }
            Instantiate(explosion, attackScale.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            if(bounces == 1)
            {
                settings.startColor = new Color (0, 1, 1, 1);
                Instantiate(explosion, attackScale.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
