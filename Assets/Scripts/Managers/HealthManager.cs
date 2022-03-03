using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class HealthManager : MonoBehaviour
{
    #region Private
    [SerializeField]
    private PlayerManager[] _players;
    private PlayerHealth _ph;
    private PlayerManager _current;
    [SerializeField]
    private GameManager _gm;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _ph = GetComponent<PlayerHealth>();
        _ph.Visible = false;
    }

    void Update()
    {
        ManageHealth();
        CheckCurrentState();
    }
    #endregion

    #region Methods

    private void CheckCurrentState()
    {
        if (_current != null && _current.State != PLAYERSTATE.BATTLE && _current.State != PLAYERSTATE.MOVE)
        {
            _ph.Visible = false;
            _current = null;
        }
    }

    /// <summary>
    /// Manages the health bar with the current stamina of the ship.
    /// </summary>
    private void ManageHealth()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            PlayerManager pm = _players[i];
            if (pm.State== PLAYERSTATE.MOVE || pm.State== PLAYERSTATE.BATTLE)
            {
                ShipHealth sh = pm.CurrentShip.GetComponent<ShipHealth>();
                _current = pm;
                _ph.Visible = true;
                _ph.Max = (int)sh.MaxHealth;
                _ph.SetHealth((int)pm.CurrentShip.GetComponent<ShipHealth>().Health);
            }
        }
    }
    #endregion 

}
