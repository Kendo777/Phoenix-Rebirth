using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class LogText : MonoBehaviour
{
    #region Public
    public PlayerTeams player;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        TMPro.TMP_Text text = GetComponent<TMPro.TMP_Text>();
        text.text = Logger.Player(player);
    }
    #endregion
}
