using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Tracks whether the game is playing or over, increases score over time,
// and restarts the level a short delay after the player collides.
public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    enum GameState { Playing, GameOver }

    //[SerializeField] float scorePerSecond = 1f;
    [SerializeField] float restartDelay = 2f;

    //UI References
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    GameState gameState;
    float score;
    float highScore;
    [SerializeField] Transform player;
    float startingHeight;

    public bool IsGameOver => gameState == GameState.GameOver;

    void Start()
    {
        score = 0f;
        gameState = GameState.Playing;

        highScore = PlayerPrefs.GetFloat("Highscore", 0f);
        
        scoreText.text = "Score: " + score.ToString("F0");
        highScoreText.text = "Best:" + highScore.ToString("F0");

        startingHeight = player.position.y;

        Debug.Log($"Saved Highscore: {highScore:F0}");
    }

    void Update()
    {
        if (gameState != GameState.Playing) return;

        /*Score is not saved anywhere yet, hisghscores don't exist yet -> TODO
        score += scorePerSecond * Time.deltaTime;
        */
        
        float currentHeight = player.position.y - startingHeight;
        score = Mathf.Max(score, currentHeight);
        
        //Update score UI
        scoreText.text = "Score: " + score.ToString("F0");
    }

    public void OnPlayerCollidedWithObstacle()
    {
        if (gameState == GameState.GameOver) return;

        gameState = GameState.GameOver;
        Debug.Log($"Game Over. Score: {score:F0}"); // F0 formats the output string by ignoring the remainders after the comma (4.35 -> 4)

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("Highscore", highScore);
            PlayerPrefs.Save();
            Debug.Log($"New Highscore: {highScore:F0}");
        }

        //Update the high score UI
        highScoreText.text = "Best:" + highScore.ToString("F0");

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
