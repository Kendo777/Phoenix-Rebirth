using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region Public
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameManager gameManager;
    #endregion

    #region Private
    private GAMESTATE _before;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
                gameManager.State = _before;
            }
            else
            {
                Pause();
                _before = gameManager.State;
                gameManager.State = GAMESTATE.PAUSE;
            }
        }
    }
    #endregion

    #region Methods
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        gameIsPaused = true;
    }
    #endregion
}
