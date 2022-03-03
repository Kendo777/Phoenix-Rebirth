using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenuButton : MonoBehaviour
{
    #region Methods
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuTestingScene");
        Time.timeScale = 1f;
    }
    #endregion

}
