using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsLeft : MonoBehaviour
{
    #region Public
    public GameManager gameManager;
    public PlayerTeams player;
    #endregion

    #region MonoBehaviour
    private void Update()
    {
        TMPro.TMP_Text element = GetComponent<TMPro.TMP_Text>();
        element.text = "Ships Left: " + gameManager.players[(int)player].GetComponent<PlayerManager>().Ships.Length.ToString();
    }
    #endregion

}
