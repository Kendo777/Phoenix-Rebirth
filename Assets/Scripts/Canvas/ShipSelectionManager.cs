using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectionManager : MonoBehaviour
{
    #region Enums
    public enum ShipType
    {
        Normal, Gorrion
    }

    public enum Team
    {
        Blue, Red
    }

    #endregion

    #region Public
    [Header("Inputs")]
    public GameObject inputPlayerRed;
    public GameObject inputPlayerBlue;
    [Header("Buttons")]
    public GameObject buttonAddNormalRed;
    public GameObject buttonAddNormalBlue;
    public GameObject buttonSubstractNormalBlue;
    public GameObject buttonSubstractNormalRed;
    public GameObject buttonAddGorrionRed;
    public GameObject buttonAddGorrionBlue;
    public GameObject buttonSubstractGorrionRed;
    public GameObject buttonSubstractGorrionBlue;
    [Header("Text")]
    public TMPro.TMP_Text currentGorrionRedText;
    public TMPro.TMP_Text currentNormalRedText;
    public TMPro.TMP_Text currentGorrionBlueText;
    public TMPro.TMP_Text currentNormalBlueText;
    [Header("Logic")]
    public int maximumShips;
    #endregion

    #region Private
    private int _currentNormalRed = 0;
    private int _currentNormalBlue = 0;
    private int _currentGorrionRed = 0;
    private int _currentGorrionBlue = 0;
    #endregion

    #region Properties 
    public int CurrentNormalRed => _currentNormalRed;
    public int CurrentNormalBlue => _currentNormalBlue;
    public int CurrentGorrionRed => _currentGorrionRed;
    public int CurrentGorrionBlue => _currentGorrionBlue;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        UpdateText();
        CheckButtons();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Adds a red ship.
    /// </summary>
    /// <param name="whatShip"></param>
    public void AddShipRed(string whatShip)
    {
        ShipType WhatShip = whatShip == "gorrion" ? ShipType.Gorrion : ShipType.Normal;
        if (_currentGorrionRed + _currentNormalRed < maximumShips)
        {
            if (WhatShip == ShipType.Normal)
                _currentNormalRed++;
            else
                _currentGorrionRed++;
        }
        
        UpdateText();
        CheckButtons();
    }

    /// <summary>
    /// Adds a blue ship
    /// </summary>
    /// <param name="whatShip"></param>
    public void AddShipBlue(string whatShip)
    {
        ShipType WhatShip = whatShip == "gorrion" ? ShipType.Gorrion : ShipType.Normal;
        if (_currentGorrionBlue + _currentNormalBlue < maximumShips)
        {
            if (WhatShip == ShipType.Normal)
                _currentNormalBlue++;
            else
                _currentGorrionBlue++;
        }
        UpdateText();
        CheckButtons();
    }

    /// <summary>
    /// Substracts a red ship.
    /// </summary>
    /// <param name="whatShip"></param>
    public void SubstractShipRed(string whatShip)
    {
        ShipType WhatShip = whatShip == "gorrion" ? ShipType.Gorrion : ShipType.Normal;
        if (WhatShip == ShipType.Normal)
            _currentNormalRed--;
        else
            _currentGorrionRed--;
        _currentGorrionRed = _currentGorrionRed < 0 ? 0 : _currentGorrionRed;
        _currentNormalRed = _currentNormalRed < 0 ? 0 : _currentNormalRed;
        UpdateText();
        CheckButtons();
    }

    /// <summary>
    /// Substracts a blue ship.
    /// </summary>
    /// <param name="whatShip"></param>
    public void SubstractShipBlue(string whatShip)
    {
        ShipType WhatShip = whatShip == "gorrion" ? ShipType.Gorrion : ShipType.Normal;
        if (WhatShip == ShipType.Normal)
            _currentNormalBlue--;
        else
            _currentGorrionBlue--;
        _currentGorrionBlue = _currentGorrionBlue < 0 ? 0 : _currentGorrionBlue;
        _currentNormalBlue = _currentNormalBlue < 0 ? 0 : _currentNormalBlue;
        UpdateText();
        CheckButtons();
    }


    /// <summary>
    /// Update text.
    /// </summary>
    private void UpdateText()
    {
        currentGorrionBlueText.text = _currentGorrionBlue.ToString();
        currentGorrionRedText.text  = _currentGorrionRed.ToString();
        currentNormalBlueText.text  = _currentNormalBlue.ToString();
        currentNormalRedText.text   = _currentNormalRed.ToString();
    }

    /// <summary>
    /// Checks buttons so we can disable them.
    /// </summary>
    private void CheckButtons()
    {
        if (_currentGorrionRed + _currentNormalRed >= maximumShips)
        {
            buttonAddGorrionRed.SetActive(false);
            buttonAddNormalRed.SetActive(false);
        }
        else
        {
            buttonAddGorrionRed.SetActive(true);
            buttonAddNormalRed.SetActive(true);
        }

        if (_currentGorrionRed + _currentNormalRed <= 0)
        {
            buttonSubstractNormalRed.SetActive(false);
            buttonSubstractGorrionRed.SetActive(false);
        }
        else
        {
            buttonSubstractNormalRed.SetActive(true);
            buttonSubstractGorrionRed.SetActive(true);
        }

        if (_currentGorrionBlue + _currentNormalBlue >= maximumShips)
        {
            buttonAddGorrionBlue.SetActive(false);
            buttonAddNormalBlue.SetActive(false);
        }
        else
        {
            buttonAddGorrionBlue.SetActive(true);
            buttonAddNormalBlue.SetActive(true);
        }

        if (_currentGorrionBlue + _currentNormalBlue <= 0)
        {
            buttonSubstractNormalBlue.SetActive(false);
            buttonSubstractGorrionBlue.SetActive(false);
        }
        else
        {
            buttonSubstractNormalBlue.SetActive(true);
            buttonSubstractGorrionBlue.SetActive(true);
        }

    }
    #endregion

}
