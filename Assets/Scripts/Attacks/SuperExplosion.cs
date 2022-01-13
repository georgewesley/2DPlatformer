using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperExplosion : Explosion
{
    void Start()
    {
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
