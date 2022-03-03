using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    #region Public
    public PlayerTeams player;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        TMPro.TMP_Text element = GetComponent<TMPro.TMP_Text>();
        element.text = player == PlayerTeams.RED ? DataTransmission.playerName0 : DataTransmission.playerName1;
    }
    #endregion

}
