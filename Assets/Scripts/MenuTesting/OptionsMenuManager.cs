using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuManager : MonoBehaviour
{
    #region MonoBehaviour
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }
    #endregion

    #region Methods
    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuTestingScene");
    }
    #endregion

}
