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
    [SerializeField] GameObject verticalExplosion;
    [SerializeField] GameObject superExplosion;
    [SerializeField] AudioClip wallBounce;
    [SerializeField] AudioClip superChargedSound;
    [SerializeField] AudioClip enemyDeathSound;
    AudioClip explosionSound;
    AudioSource rightSide;
    AudioSource leftSide;
    ParticleSystem.MainModule settings;
    PlayerMovement player;
    Transform attackScale;
    BoxCollider2D boxCollider;
    PolygonCollider2D polyCollider;
    bool superCharged = false;
    public bool isPlayingAudio = false;
    int relativePosition;
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
        attackScale.localScale = new Vector3(playerScale, 1f);
        settings = explosion.GetComponentInChildren<ParticleSystem>().main;
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        rightSide = Camera.main.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        leftSide = Camera.main.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        attackBody.velocity = new Vector2(xSpeed, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == polyCollider) { //front
            if (!superCharged)
            {
                if (collision.gameObject.tag == "ground" && bounces < 1)
                {
                    PlaySpatialSound(wallBounce);
                    sprite.color = new Color(0, 1, 1, 1);
                    bounces += 1;
                    attackScale.localScale = new Vector3(-attackScale.localScale.x, attackScale.localScale.y);
                    xSpeed = attackScale.localScale.x * bulletSpeed / 2;
                }
                else if (collision.gameObject.tag == "Attack")
                {
                    if (bounces == 1)
                    {
                        isPlayingAudio = !collision.gameObject.GetComponent<Attack>().isPlayingAudio; // this is here because bounces < 1 implies no explosion
                        settings.startColor = new Color(0, 1, 1, 1);
                        if (collision.gameObject.GetComponent<Attack>().bounces == 1)
                        { // if both attacks are blue
                            InstantiateExplosion(verticalExplosion, !isPlayingAudio);
                        }
                        InstantiateExplosion(explosion);
                    }
                    Destroy(gameObject);
                }
                else if (collision.gameObject.tag == "Explosion") // does not seem to trigger, only the OnParticleCollion method does
                {
                    superCharged = true;
                    sprite.color = new Color(0, 0, 0, 1);
                    xSpeed = xSpeed * 2;
                }
                else
                {
                    if (bounces == 1)
                    {
                        settings.startColor = new Color(0, 1, 1, 1);
                        InstantiateExplosion(explosion);
                    }
                    else
                    {
                        PlaySpatialSound(enemyDeathSound);
                    }
                    Destroy(gameObject);
                }
            }
            else
            {
                if (collision.gameObject.tag != "Explosion")
                {
                    InstantiateExplosion(superExplosion);
                    Destroy(gameObject);
                }
            }
        }
        else if (collision.otherCollider == boxCollider) //back
        {
            if (collision.gameObject.tag == "Attack" && bounces == 1)
            {
                transform.eulerAngles = new Vector3(0, 0, -90 * Mathf.Sign(attackScale.localScale.x));
                InstantiateExplosion(lineExplosion);
                Destroy(gameObject);
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<VerticalExplosion>() != null && !superCharged)
        {
            superCharged = true;
            sprite.color = new Color(0, 0, 0, 1);
            xSpeed = xSpeed / 2f;
            PlaySpatialSound(superChargedSound);
        }
        else if (other.GetComponent<LineExplosion>() != null && !superCharged)
        {
            settings.startColor = new Color(1, 0, 0, 1);
            InstantiateExplosion(explosion);
            Destroy(gameObject);
        }

    }
    private void PlaySpatialSound(AudioClip clip) {
        if (player.transform.position.x > attackScale.position.x) //Player to the right of attack
        {
            leftSide.PlayOneShot(clip);
        }
        else if (player.transform.position.x < attackScale.position.x) //Player to the left of attack
        {
            rightSide.PlayOneShot(clip);
        }
    }

    private void InstantiateExplosion(GameObject newExplosion, bool playSound = true) {
        explosionSound = Instantiate(newExplosion, attackScale.position, transform.rotation).gameObject.GetComponent<Explosion>().explosionSound;
        if(playSound) {
            PlaySpatialSound(explosionSound);
        }
    }
}
