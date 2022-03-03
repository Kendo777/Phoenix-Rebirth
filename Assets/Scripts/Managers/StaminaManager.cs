using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class StaminaManager : MonoBehaviour
{
    #region Public

    #endregion

    #region Private
    [SerializeField]
    private PlayerManager[] _players;
    [SerializeField]
    private int maxHealth = 100;
    private PlayerHealth _ph;
    private PlayerManager _current;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _ph = GetComponent<PlayerHealth>();
        _ph.Visible = false;
    }

    void Update()
    {
        ManageStamina();
        CheckCurrentState();
    }
    #endregion

    #region Methods
    private void CheckCurrentState()
    {
        if (_current != null && _current.State!= PLAYERSTATE.BATTLE && _current.State!= PLAYERSTATE.MOVE)
        {
            _ph.Visible = false;
            _current = null;
        }
    }

    /// <summary>
    /// Manages the stamina bar with the current stamina of the ship.
    /// </summary>
    private void ManageStamina()
    {
        if (!_ph.Visible && _ph.Health != maxHealth)
            _ph.SetHealth(maxHealth);
        for (int i = 0; i < _players.Length; i++)
        {
            PlayerManager pm = _players[i];
            if (pm.State == PLAYERSTATE.MOVE || pm.State== PLAYERSTATE.BATTLE)
            {
                maxHealth = (int)pm.durationOfTurns;
                _ph.Max = maxHealth;
                _current = pm;
                _ph.Visible = true;
                _ph.SetHealth((int)pm.TimeLeft);
            }
        }
    }
    #endregion 

}
