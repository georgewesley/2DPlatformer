using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
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
    [SerializeField] float coolDown;
    Animator playerAnimation;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;
    bool isGrounded = false;
    bool isAlive = true;
    bool isOnCoolDown = false;
    float gravity;

    void Start()
    {
        
        playerBody = GetComponent<Rigidbody2D>();
        gravity = playerBody.gravityScale;
        playerAnimation = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if (isAlive)
        {
            Run();
            ClimbLadder();
            FlipSprite();
            playerAnimation.SetFloat("Y Speed", playerBody.velocity.y + Mathf.Epsilon);
            isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("ground"));
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
        if(isAlive&&!isOnCoolDown)
        {
            isOnCoolDown = true;
            playerAnimation.SetTrigger("attack");
            StartCoroutine(InstantiateAttack());
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
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("climbing")))
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
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards", "Attacks")))
        {
            isAlive = false;
            playerAnimation.SetTrigger("die");
            playerBody.bodyType = RigidbodyType2D.Static;
        }
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject);
        }
    }
}
