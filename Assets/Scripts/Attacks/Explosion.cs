using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionSound;
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    public virtual IEnumerator DestroySelf() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
