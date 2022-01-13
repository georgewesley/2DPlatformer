using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineExplosion : Explosion
{
    private void Start()
    {
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
