using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI enemyText;
    public int numEnemy;
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChange;
    }

    private void OnDisable()
    {
 
        SceneManager.sceneLoaded -= SceneChange;
    }

    private void Start()
    {
        livesText.text = "Lives: " + playerLives.ToString();
        scoreText.text = "Score: 0";
        UpdateEnemy();
    }
    public void ProcessPlayerDeath() {
        if (playerLives > 1) {
            playerLives = playerLives - 1;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.gameObject.transform.position = player.initialPosition;
            livesText.text = "Lives: " + playerLives.ToString();
            scoreText.text = "Score: 0";
        }
        else {
            Debug.Log("Reset Game");
            SceneManager.LoadScene(0);
            Destroy(gameObject); // so a new one with new lives can be created
        }
    }
    public void UpdateScore(int score)
    {
        string text = scoreText.text.Remove(0, 6);
        scoreText.text = "Score: " + (int.Parse(text) + score).ToString();
    }

    public void KillEnemy()
    {
        numEnemy--;
        enemyText.text = "Enemies: " + numEnemy.ToString();
    }
    public void UpdateEnemy()
    {
        if(FindObjectsOfType<Enemy>() != null)
        {
            numEnemy = FindObjectsOfType<Enemy>().Length;
            Debug.Log("Enemy num: " + numEnemy);
        }
        enemyText.text = "Enemies: " + numEnemy;
    }

    public void SceneChange(Scene s, LoadSceneMode l)
    {
        Debug.Log("We loaded a scene");
        UpdateEnemy();
    }
}
