using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int bouncesToBreak;
    [SerializeField] GameObject shieldBreak;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip shieldBlock;
    bool updraft;

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
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if(other.gameObject.name == "Vertical Explosion(Clone)" && GetComponentInParent<PlayerMovement>() != null)
        {
            updraft = true;
            StartCoroutine(ApplyForce(GetComponentInParent<PlayerMovement>()));
        }
    }

    IEnumerator ApplyForce(PlayerMovement player)
    {
        while(updraft) //problem is that this will autoclose when shield is disabled
        {
            player.Updraft();
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Exit Updraft");
    }

    IEnumerator RemoveUpdraft()
    {
        yield return new WaitForSeconds(3);
        updraft = false;
    }
}
