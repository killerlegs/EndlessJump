using UnityEngine;
using UnityEngine.SceneManagement;

// Central point for level flow. Manages Player Restart 
// and has methods that are called by the Finish Level zone
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Player player;
    [SerializeField] float levelCompleteDelay = 1.5f;

    private void Start()
    {
        player.OnRespawn(startPoint.position);
    }

    // Called by Player when it touches a KillZone.
    public void HandlePlayerDeath()
    {
        player.OnRespawn(startPoint.position);
    }

    // Called by FinishZone when the player reaches the end of the level.
    public void OnPlayerReachedFinish()
    {
        // Invoke calls a method by name after a delay - a simpler alternative
        // to a coroutine for a single delayed call.
        Invoke(nameof(LoadNextLevel), levelCompleteDelay);
    }

    void LoadNextLevel()
    {
        // Scenes only have a valid build index if they're added to
        // File > Build Profiles > Scene List.
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels - all missions complete!");
        }
    }
}

