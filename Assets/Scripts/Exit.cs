using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            if(SceneManager.sceneCountInBuildSettings != SceneManager.GetActiveScene().buildIndex+1) {
                StartCoroutine(SceneDelay());
            }
        }
        
    }

    private IEnumerator SceneDelay() {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
