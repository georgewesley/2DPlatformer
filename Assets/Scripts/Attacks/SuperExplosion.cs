using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperExplosion : Explosion
{
    public override IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
