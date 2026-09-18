using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Player player;
    [SerializeField] float levelCompleteDelay = 1.5f;

    private void Start()
    {
        player.OnRespawn(startPoint.position);
    }

    public void HandlePlayerDeath()
    {
        player.OnRespawn(startPoint.position);
    }

    public void OnPlayerReachedFinish()
    {
        StartCoroutine(LoadNextLevelAfterDelay());
    }

    IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(levelCompleteDelay);

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

