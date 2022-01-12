using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

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
        if(isAlive) {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else {
            myRigidBody.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isAlive) {
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
        if(enemyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Attacks")))
        {
            Die();
        }
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnParticleCollision(GameObject other) {
        if(other.gameObject.tag == "Explosion") {
            Die();
        }
    }
    public void Die() 
    {
        if(isAlive) 
        {
            isAlive = false;
            myRigidBody.isKinematic = true;
            enemyAnimator.SetTrigger("death");
            enemyCollider.enabled = false;
            particles.Play();
        }
        
    }
}
