using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    public override void OnTriggerExit2D(Collider2D other) {
        if (isAlive)
        {
            moveSpeed = -moveSpeed;
        }
    }
}
