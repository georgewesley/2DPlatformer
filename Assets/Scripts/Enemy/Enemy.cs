using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1f;
    [SerializeField] ParticleSystem particles;
    Rigidbody2D myRigidBody;
    Transform currentTransform;
    Animator enemyAnimator;
    public bool isAlive = true;
    Collider2D enemyCollider;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        currentTransform = GetComponent<Transform>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(0, 0);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (isAlive&&collision.gameObject.tag=="ground")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-currentTransform.localScale.x, currentTransform.localScale.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Die();
            Destroy(collision.gameObject);
        }
    }
    public virtual void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Explosion")
        {
            Die();
        }
    }
    public void Die()
    {
        if (isAlive)
        {
            FindObjectOfType<GameSession>().GetComponent<GameSession>().KillEnemy();
            isAlive = false;
            myRigidBody.isKinematic = true;
            enemyAnimator.SetTrigger("death");
            enemyCollider.enabled = false;
            particles.Play();
            StartCoroutine(removeBody());
        }

    }

    IEnumerator removeBody()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
