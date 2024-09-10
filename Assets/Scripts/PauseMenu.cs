using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] InputHandler inputHandler;
    private bool isPaused = false;
    private void Update()
    {
        if (inputHandler.togglePause)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
            isPaused = !isPaused;
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        print("wooo " + sceneID);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
