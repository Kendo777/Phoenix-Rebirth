using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectingShipsManager : MonoBehaviour
{
    #region Public

    [Header("Inputs")]
    public TMPro.TMP_InputField gorrionShipsInput_RED;
    public TMPro.TMP_InputField normalShipsInput_RED;

    public TMPro.TMP_InputField gorrionShipsInput_BLUE;
    public TMPro.TMP_InputField normalShipsInput_BLUE;

    [Header("Text")]
    public TMPro.TMP_Text errorMessage;
    [Header("1 RED NORMAL 2 RED GORRION 3 BLUE NORMAL 4 BLUE GORRION.")]
    public GameObject[] SHIP_PREFABS;
    #endregion

    #region Properties
    public enum TypeOfShip
    {
        RedNormal = 0,
        RedGorrion = 1,
        BlueNormal,
        BlueGorrion,
    }
    #endregion

    #region Private
    [SerializeField]
    private int _maximumShips = 9;
    private ShipSelectionManager _ssm;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        errorMessage.gameObject.SetActive(false);
        _ssm = GetComponentInParent<ShipSelectionManager>();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Sends DataTransmission all the ships that should be spawned. ----- This method is REALLY SLOW. -----
    /// </summary>
    public void Submit()
    {
        AddRedShips();
        AddBlueShips();

        if (DataTransmission.shipsBlue.Count <= 0 || DataTransmission.shipsRed.Count <= 0)
        {
            // Show user that he needs to put less ships...
            errorMessage.gameObject.SetActive(true);
            DataTransmission.shipsBlue = new List<GameObject>();
            DataTransmission.shipsRed = new List<GameObject>();
            return;
        }
        string playerName0 = _ssm.inputPlayerRed.GetComponent<TMPro.TMP_InputField>().text;
        string playerName1 = _ssm.inputPlayerBlue.GetComponent<TMPro.TMP_InputField>().text;
        DataTransmission.playerName0 = playerName0.Trim() == "" ? "Red Team" : playerName0;
        DataTransmission.playerName1 = playerName1.Trim() == "" ? "Blue Team" : playerName1;
        SceneManager.LoadScene("LoadingScreen");
    }


    /// <summary>
    /// Yes, maybe this is overkill and SLOW for transferring what ships should we 
    /// spawn but we have 0 TIME to refactor this to better code.
    /// </summary>

    private void AddBlueShips()
    {
        int normalShipsBlue = _ssm.CurrentNormalBlue;
        int gorrionShipsBlue = _ssm.CurrentGorrionBlue;
        if (CheckInputs(normalShipsBlue, gorrionShipsBlue))
        {
            // Add ships to the static method and change scene.
            for (int i = 0; i < normalShipsBlue; i++)
            {
                DataTransmission.AddShipBlue(SHIP_PREFABS[(int)TypeOfShip.BlueNormal]);
            }

            for (int i = 0; i < gorrionShipsBlue; i++)
            {
                DataTransmission.AddShipBlue(SHIP_PREFABS[(int)TypeOfShip.BlueGorrion]);
            }
        }
        else
        {
            DataTransmission.EmptyShips();
            errorMessage.text = "BLUE: Maximum 9 and minimum 1!";
            Debug.Log("Error pasing ints");
        }
    }

    /// <summary>
    /// Yes, maybe this is overkill and SLOW for transferring what ships should we 
    /// spawn but we have 0 TIME to refactor this to better code.
    /// </summary>
    private void AddRedShips()
    {
        int normalShipsRed = _ssm.CurrentNormalRed;
        int gorrionShipsRed = _ssm.CurrentGorrionRed;

        if (CheckInputs(normalShipsRed, gorrionShipsRed))
        {
            // Add ships to the static method and change scene.
            for (int i = 0; i < normalShipsRed; i++)
            {
                DataTransmission.AddShipRed(SHIP_PREFABS[(int)TypeOfShip.RedNormal]);
            }

            for (int i = 0; i < gorrionShipsRed; i++)
            {
                DataTransmission.AddShipRed(SHIP_PREFABS[(int)TypeOfShip.RedGorrion]);
            }
        }
        else
        {
            DataTransmission.EmptyShips();
            errorMessage.text = "RED: Maximum 9 and minimum 1!";
            Debug.Log("Error pasing ints");
        }
    }

    /// <summary>
    /// Checks 2 inputs so they are not more than the maximum.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    private bool CheckInputs(int one, int two)
    {
        int sum = one + two;
        if (sum > _maximumShips || sum < 0)
        {
            return false;
        }
        return true;
    }
    #endregion
}

