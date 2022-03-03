using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenBehaviour : MonoBehaviour
{
    #region Public
    public float counter = 1.0f;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            SceneManager.LoadScene("loadScene");
        }
    }
    #endregion
}
