using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurn : MonoBehaviour
{
    #region Public
    public GameManager gameManager;
    #endregion

    #region Private

    #endregion

    #region Methods
    public void Pass()
    {
        gameManager.CurrentPlayer.SetState(PLAYERSTATE.PASS_TURN);
    }
    #endregion
}
