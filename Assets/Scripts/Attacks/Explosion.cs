using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionSound;
    [SerializeField] float selfDestructTime;
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    public virtual IEnumerator DestroySelf() {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }
}
