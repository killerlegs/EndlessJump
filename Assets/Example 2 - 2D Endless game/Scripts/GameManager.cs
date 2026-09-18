using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Central game state, matching the lecture's "Manager vs Controller" and
// FlappyGameManager examples: the PlayerController informs this manager on
// collision instead of handling game-over logic itself.
public class GameManager : MonoBehaviour
{
    enum GameState { Playing, GameOver }

    [SerializeField] float scorePerSecond = 1f;
    [SerializeField] float restartDelay = 2f;

    // In-memory only (not PlayerPrefs): survives a Restart() within the same
    // play session since it's static, but resets when the game is relaunched.
    static float highscore;

    GameState gameState;
    float score;

    public bool IsGameOver => gameState == GameState.GameOver;

    void Start()
    {
        score = 0f;
        gameState = GameState.Playing;
    }

    void Update()
    {
        if (gameState != GameState.Playing) return;

        // Increase score by 1 every second the player survives
        score += scorePerSecond * Time.deltaTime;
    }

    // Optional hook for extensions that award score for other events,
    // e.g. passing through a pipe gap (see README suggested extensions)
    public void AddScore(float amount)
    {
        if (gameState != GameState.Playing) return;
        score += amount;
    }

    // Called by the player controller on collision
    public void OnPlayerCollided()
    {
        if (gameState == GameState.GameOver) return;

        gameState = GameState.GameOver;

        bool isNewHighscore = score > highscore;
        if (isNewHighscore) highscore = score;

        // TODO (student exercise, see lecture Section 8 - UI):
        // uiManager.ShowGameOverScreen(isNewHighscore, highscore);
        Debug.Log(isNewHighscore ? $"New highscore: {highscore:F0}" : $"Game Over. Score: {score:F0}");

        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        Restart();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
