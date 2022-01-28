using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerBody;
    [SerializeField] float speedMultiplier;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float inAirReduction = .5f;
    [SerializeField] float ladderSpeed = 10f;
    [SerializeField] float ladderDecentSpeed = 10f;
    [SerializeField] GameObject attack;
    [SerializeField] Transform sword;
    [SerializeField] GameObject shield;
    [SerializeField] float coolDown;
    [SerializeField] float shieldCoolDown;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip deathSound;

    AudioSource playerSound;
    GameSession gameSession;
    Animator playerAnimation;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;
    public Vector3 initialPosition;
    bool isGrounded = false;
    bool isAlive = true;
    bool isOnCoolDown = false;
    bool shieldOnCoolDown = false;
    float gravity;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        playerBody = GetComponent<Rigidbody2D>();
        gravity = playerBody.gravityScale;
        playerAnimation = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        initialPosition = GetComponent<Transform>().position;
        playerSound = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (isAlive)
        {
            Run();
            ClimbLadder();
            FlipSprite();
            playerAnimation.SetFloat("Y Speed", playerBody.velocity.y + Mathf.Epsilon);
            isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed&&isAlive)
        {
            if (isGrounded)
            {
                playerBody.velocity += new Vector2(0f, jumpSpeed);
                playerAnimation.SetTrigger("jump");
            }
        }
    }

    void OnFire(InputValue value)
    {
        if(isAlive&&!isOnCoolDown&&!sword.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        //last condition prevents from spawning inside walls
        {
            isOnCoolDown = true;
            playerAnimation.SetTrigger("attack");
            playerSound.PlayOneShot(attackSound);
            StartCoroutine(InstantiateAttack());
        }
    }

    void OnBlock(InputValue value)
    {
        Debug.Log("Try to block");
        if(isAlive&&!shieldOnCoolDown)
        {
            Debug.Log("Actual Block");
            playerAnimation.SetTrigger("block");
            shield.SetActive(true);
            shieldOnCoolDown = true;
            playerCollider.enabled = false;
            StartCoroutine(ShieldCoolDown());
        }
    }

    IEnumerator InstantiateAttack()
    {
        yield return new WaitForSeconds(0.34f);
        Instantiate(attack, sword.position, transform.rotation);
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSecondsRealtime(coolDown); //note that the cool down will also essentially include .34 seconds from above InstantiateAttack 
        isOnCoolDown = false;
    }

    IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSecondsRealtime(.5f);
        playerCollider.enabled = true;
        shield.SetActive(false);
        yield return new WaitForSecondsRealtime(shieldCoolDown);
        shieldOnCoolDown = false;
    }

    void Run()
    {
        Vector2 playerVelocity;
        if (isGrounded)
        {
            playerVelocity = new Vector2(moveInput.x * speedMultiplier, playerBody.velocity.y);
            playerAnimation.SetBool("isRunning", Mathf.Abs(moveInput.x) > Mathf.Epsilon);
        }
        else
        {
            playerVelocity = new Vector2(moveInput.x * speedMultiplier * inAirReduction, playerBody.velocity.y);
        }
        playerBody.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHorizontalSpeed = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;
        if(playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerBody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        playerAnimation.SetBool("isClimbing", false);
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerBody.gravityScale = gravity;
            return;
        }

        playerBody.gravityScale = 0;
        if(moveInput.y>0)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, moveInput.y * ladderSpeed);
        }
        else if(moveInput.y<0) {
            playerBody.velocity = new Vector2(playerBody.velocity.x, moveInput.y * ladderDecentSpeed);
            playerAnimation.SetBool("isClimbing", true);
        }
        else
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Attacks"))&&isAlive)
        {
            Die();
        }
        else if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) && collision.gameObject.GetComponent<Enemy>() != null && isAlive) {
            //not sure why but line below is still called without the second bool above. I would think that a collision would not even occur since it is disabled.
            if(collision.gameObject.GetComponent<Enemy>().isAlive) {
                Die();
            }
        }
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Background" && isAlive && shield.activeSelf == false|| (other.gameObject.tag == "Explosion" && isAlive)) 
        //if no isAlive here, then this will trigger second death if a collider is turned off
        {
            if (other.gameObject.name != "Vertical Explosion(Clone)")
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Explosion" && isAlive) {
            Die();
        }
    }

    private void OnParticleCollision(GameObject other) {
        if(other.gameObject.tag == "Explosion"&&isAlive) {
            Die();
        }
    }
    public void Die() {
        playerSound.PlayOneShot(deathSound);
        isAlive = false;
        playerCollider.enabled = false;
        playerBody.gravityScale = gravity;
        playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
        playerAnimation.SetTrigger("die");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Player"), true);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSecondsRealtime(3);
        gameSession.ProcessPlayerDeath();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Player"), false);
        isAlive = true;
        playerCollider.enabled = true;
        playerAnimation.SetTrigger("alive");
        isOnCoolDown = false;
        shieldOnCoolDown = false;
    }

    public void Updraft()
    {
        playerBody.AddForce(new Vector2(0f, .2f));
        playerAnimation.SetTrigger("jump");
    }
}
