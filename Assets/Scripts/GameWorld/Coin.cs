using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickup;
    GameSession game;

    private void Start()
    {
        game = FindObjectOfType<GameSession>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.UpdateScore(100);
            AudioSource.PlayClipAtPoint(coinPickup, Camera.main.transform.position + new Vector3(0, 0, -1));
            Destroy(gameObject);
        }
    }
}
