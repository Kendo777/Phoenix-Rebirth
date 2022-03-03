using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameManager))]
public class CanvasManager : MonoBehaviour
{
    #region Public
    [SerializeField]
    private TMPro.TextMeshProUGUI _textEndScreenWinner;
    [SerializeField]
    private GameObject _cartelEnd;
    #endregion

    #region Private
    private GameManager _gm;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _gm = GetComponent<GameManager>();
        _textEndScreenWinner.gameObject.SetActive(false);
        _cartelEnd.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShowWinnerFinalScreen();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Show win screen if game has finished.
    /// </summary>
    void ShowWinnerFinalScreen()
    {
        if (_gm.State == GAMESTATE.LOSE && _gm.Winner != null)
        {
            
            _textEndScreenWinner.text = string.Format("The winner is {0}!", _gm.Winner.Name);
            _textEndScreenWinner.gameObject.SetActive(true);
            _cartelEnd.SetActive(true);
        }
    }
    #endregion

}
