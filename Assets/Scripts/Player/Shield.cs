using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int bouncesToBreak;
    [SerializeField] GameObject shieldBreak;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip shieldBlock;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            Attack attack = collision.gameObject.GetComponent<Attack>();
            Debug.Log(attack.bounces);
            if (attack.bounces >= bouncesToBreak)
            {
                Instantiate(shieldBreak, gameObject.transform.position, transform.rotation);
                ParticleSystem.MainModule settings = explosion.GetComponentInChildren<ParticleSystem>().main;
                settings.startColor = attack.lastColor; // sets color for explosion to be the same as the current color of the attack object
                collision.gameObject.GetComponent<Attack>().InstantiateExplosion(explosion);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                attack.PlaySpatialSound(shieldBlock);
            }
        }
        
    }
}
