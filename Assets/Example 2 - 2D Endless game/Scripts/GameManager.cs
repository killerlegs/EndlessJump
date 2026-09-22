using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tracks whether the game is playing or over, increases score over time,
// and restarts the level a short delay after the player collides.
public class GameManager : MonoBehaviour
{
    enum GameState { Playing, GameOver }

    [SerializeField] float scorePerSecond = 1f;
    [SerializeField] float restartDelay = 2f;

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

        // Score is not saved anywhere yet, hisghscores don't exist yet -> TODO
        score += scorePerSecond * Time.deltaTime;
    }

    public void OnPlayerCollidedWithObstacle()
    {
        if (gameState == GameState.GameOver) return;

        gameState = GameState.GameOver;
        Debug.Log($"Game Over. Score: {score:F0}"); // F0 formats the output string by ignoring the remainders after the comma (4.35 -> 4)

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
