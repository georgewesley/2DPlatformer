using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : Enemy
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject); //since we do not call die we are uneffected
        }
    }
}
