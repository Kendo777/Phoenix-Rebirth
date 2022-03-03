using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data manager in-game.
/// </summary>
public class DataManagementManager : MonoBehaviour
{
    #region Private
    private GameManager _gm;
    private GameObject[] _players;
    [SerializeField]
    private Transform[] _spawnPositionsRed;
    [SerializeField]
    private Transform[] _spawnPositionsBlue;
    [Header("Motherships")]
    [SerializeField]
    private GameObject _mothershipBlue;
    [SerializeField]
    private GameObject _mothershipRed;
    [SerializeField]
    private AxisMobile _axis;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _gm = GetComponent<GameManager>();
        _players = _gm.players;
        FillData();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Fills every data necessary for gameplay.
    /// </summary>
    private void FillData()
    {
        // Players.
        PlayerManager[] players = { _players[0].GetComponent<PlayerManager>(), _players[1].GetComponent<PlayerManager>() };
        // Ship lists.
        List<GameObject> blueShips = new List<GameObject>();
        List<GameObject> redShips = new List<GameObject>();
        // for-loop i 
        int i = 0;

        // Add every ship.
        for (i = 0; i < DataTransmission.shipsRed.Count; i++)
        {
            GameObject ship = Instantiate(DataTransmission.shipsRed[i]);
            ship.transform.position = _spawnPositionsRed[i].position;
            ship.transform.rotation = _spawnPositionsRed[i].rotation;
            ship.GetComponent<ShipMovement>().axis = _axis;
            redShips.Add(ship);
        }
        // Add the mothership.
        GameObject motherShipR = Instantiate(_mothershipRed);
        motherShipR.GetComponent<ShipMovement>().axis = _axis;
        Transform lastPointRed = _spawnPositionsRed[_spawnPositionsRed.Length - 1];
        motherShipR.transform.position = lastPointRed.position;
        motherShipR.transform.rotation = lastPointRed.rotation;
        redShips.Add(motherShipR);
        players[0].Ships = redShips.ToArray();
        // Add blue ships.
        for (i = 0; i < DataTransmission.shipsBlue.Count; i++)
        {
            GameObject ship = Instantiate(DataTransmission.shipsBlue[i]);
            ship.transform.position = _spawnPositionsBlue[i].position;
            ship.transform.rotation = _spawnPositionsBlue[i].rotation;
            ship.GetComponent<ShipMovement>().axis = _axis;
            blueShips.Add(ship);
        }
        // Add the mothership blue.
        GameObject motherShipB = Instantiate(_mothershipBlue);
        motherShipB.GetComponent<ShipMovement>().axis = _axis;
        Transform lastPointBlue = _spawnPositionsBlue[_spawnPositionsRed.Length - 1];
        motherShipB.transform.position = lastPointBlue.position;
        motherShipB.transform.rotation = lastPointBlue.rotation;
        blueShips.Add(motherShipB);
        // Transform to blue ships.
        players[1].Ships = blueShips.ToArray();
    }
    #endregion

}
