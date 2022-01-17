using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] AudioClip explosionSound;
    Vector3 explosionPosition;
    Vector3 playerPosition;
    void Start()
    {
        explosionPosition = GetComponent<Transform>().position;
        StartCoroutine(DestroySelf());
        playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
        int relativePosition = RelativePlayerPosition();
        AudioSource.PlayClipAtPoint(explosionSound, new Vector3(Camera.main.transform.position.x + relativePosition, explosionPosition.y, Camera.main.transform.position.z - 1));
    }

    public virtual IEnumerator DestroySelf() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private int RelativePlayerPosition()
    {
        if (playerPosition.x > explosionPosition.x) //Player to the right of attack
        {
            return -1;
        }
        else if (playerPosition.x < explosionPosition.x) //Player to the left of attack
        {
            return 1;
        }
        return 0; //player right at the attack
    }
}
