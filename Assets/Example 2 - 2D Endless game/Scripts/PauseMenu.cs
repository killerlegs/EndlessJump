using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] string mainMenuSceneName = "Main Menu";

    [SerializeField] AudioSource backgroundMusic;

    void Start()
    {
        resumeButton.onClick.AddListener(HidePauseMenu);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        backgroundMusic.Pause();
        Debug.Log("PAUSE triggered. TimeScale: " + Time.timeScale);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        backgroundMusic.UnPause();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}