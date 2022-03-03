using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Data transmition from scene to scene in-game class.
/// </summary>
static public class DataTransmission
{
    #region Public Static Variables
    static public List<GameObject> shipsRed = new List<GameObject>();
    static public List<GameObject> shipsBlue = new List<GameObject>();
    static public string playerName0 = "Player 1";
    static public string playerName1 = "Player 2";
    static public AudioClip chosenAudioClip;
    #endregion

    #region Methods
    /// <summary>
    /// Adds a red ship prefab.
    /// </summary>
    /// <param name="ship"></param>
    /// <returns></returns>
    static public bool AddShipRed(GameObject ship)
    {
        try
        {
            if (!CheckShip(ship))
            {
                throw new IOException("Error adding a ship because it's missing a component");
            }
            shipsRed.Add(ship);
            return true;
        } catch(IOException _)
        {
            Debug.LogError("Error adding the ship. Reason: " + _.Message);
            return false;
        }
    }

    /// <summary>
    /// Adds a blue ship prefab
    /// </summary>
    /// <param name="ship"></param>
    /// <returns></returns>
    static public bool AddShipBlue(GameObject ship)
    {
        try
        {
            if (!CheckShip(ship))
            {
                throw new IOException("Error adding a ship because it's missing a component");
            }
            shipsBlue.Add(ship);
            return true;
        }
        catch (IOException _)
        {
            Debug.LogError("Error adding the ship. Reason: " + _.Message);
            return false;
        }
    }

    /// <summary>
    /// Checks if the ship follows the script component rules.
    /// </summary>
    /// <param name="ship"></param>
    /// <returns></returns>
    static public bool CheckShip(GameObject ship)
    {
        if 
        (
            ship.GetComponent<ShipClickManager>() == null || ship.GetComponent<ShipManager>() == null || ship.GetComponent<ShipMovement>() == null ||
            ship.GetComponent<ShipHealth>() == null || ship.GetComponent<ShipPieceDestroy>() == null || ship.GetComponent<ShipShoot>() == null
        )
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Empties the lists. If there is an error adding or there is a mistake use this method.
    /// </summary>
    static public void EmptyShips()
    {
        shipsBlue = new List<GameObject>();
        shipsRed = new List<GameObject>();
    }
    #endregion
}
