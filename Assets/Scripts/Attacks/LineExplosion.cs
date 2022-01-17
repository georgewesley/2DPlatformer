using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineExplosion : Explosion
{
    public override IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
