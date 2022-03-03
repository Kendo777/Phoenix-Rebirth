using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    #region Public
    
    public GameObject crossHairGorrionBlue;
    public GameObject crossHairGorrionRed;
    public GameObject crossHairNormalRed;
    public GameObject crossHairNormalBlue;

    #endregion

    #region Private
    const string _falcon = "Falcon";
    const string _gorrion = "Gorrion";
    private PlayerManager _currentPlayer;
    private GameManager _gameManager;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        Desactivate();
        _gameManager = GetComponent<GameManager>();
    }
    private void Update()
    {
        if (_gameManager)
        {
            if (!_currentPlayer && _gameManager.CurrentPlayer && _gameManager.CurrentPlayer.State == PLAYERSTATE.BATTLE)
            {
                Desactivate();
                _currentPlayer = _gameManager.CurrentPlayer;
            }
            else if (_currentPlayer && _currentPlayer.State != PLAYERSTATE.BATTLE)
            {
                Desactivate();
                _currentPlayer = null;
            }

            if (_currentPlayer)
            {
                SetCrosshair(_currentPlayer.team, _currentPlayer.CurrentShip.tag);
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Sets the crosshair for the specified team
    /// </summary>
    /// <param name="team"></param>
    /// <param name="tag"></param>
    private void SetCrosshair(PlayerTeams team, string tag)
    {
        if (team == PlayerTeams.BLUE)
        {
            if (tag == _falcon)
            {
                crossHairNormalBlue.SetActive(true);
            }
            else
            {
                crossHairGorrionBlue.SetActive(true);
            }
        }
        else
        {
            if (tag == _falcon)
            {
                crossHairNormalRed.SetActive(true);
            }
            else
            {
                crossHairGorrionRed.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Desactivates all the crosshairs
    /// </summary>
    private void Desactivate()
    {
        crossHairGorrionBlue.SetActive(false);
        crossHairGorrionRed.SetActive(false);
        crossHairNormalBlue.SetActive(false);
        crossHairNormalRed.SetActive(false);
    }
    #endregion

}
