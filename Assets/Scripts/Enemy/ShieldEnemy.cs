using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    public override void OnTriggerExit2D(Collider2D other) {
        if (isAlive&&other.tag == "ground")
        {
            moveSpeed = -moveSpeed;
        }
    }
    public override void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "Shield")
        {
            Die();
        }
    }
}
