using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the ceiling because if we don't it annoys the collider of the ships.
/// </summary>
public class Ceiling : MonoBehaviour
{
    #region Public
    public GameManager gameManager;
    #endregion

    #region MonoBehaviour
    private void Update()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (gameManager && gameManager.CurrentPlayer && gameManager.CurrentPlayer.State == PLAYERSTATE.SELECTING_SHIP)
        {
            box.enabled = false;
        }
        else
        {
            box.enabled = true;
        }
    }
    #endregion
}
