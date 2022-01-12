using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D attackBody;
    SpriteRenderer sprite;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject lineExplosion;
    ParticleSystem.MainModule settings;
    PlayerMovement player;
    Transform attackScale;
    BoxCollider2D boxCollider;
    PolygonCollider2D polyCollider;

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
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        attackBody.velocity = new Vector2(xSpeed, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider == polyCollider) { //front
            if(collision.gameObject.tag == "ground" && bounces < 1)
            {
                sprite.color = new Color(0, 1, 1, 1);
                bounces += 1;
                attackScale.localScale = new Vector3(-attackScale.localScale.x, attackScale.localScale.y);
                xSpeed = attackScale.localScale.x * bulletSpeed/2;
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
        else if(collision.otherCollider == boxCollider) //back
        { 
            Debug.Log("BOX");
            if(collision.gameObject.tag == "Attack" && bounces == 1) 
            {
                transform.eulerAngles = new Vector3(0, 0, -90*Mathf.Sign(attackScale.localScale.x));
                Instantiate(lineExplosion, attackScale.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
