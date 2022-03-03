using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;

public class PlayerManager : MonoBehaviour
{
    #region Private variables
    private PLAYERSTATE _currentState = PLAYERSTATE.SELECTING_SHIP;
    private GameObject _currentShip = null;
    private float _time = 30;
    [SerializeField]
    private string _playerName = "Player";
    [SerializeField]
    private GameObject[] _ships;
    #endregion

    #region Public variables
    [ReadOnly]
    public float durationOfTurns = 100f;
    public GameObject zenitalCameraMovement;
    public PlayerTeams team = PlayerTeams.RED;
    public bool lostMothership = false;
    #endregion

    #region Properties
    /// <summary>
    /// The current ships of the player.
    /// </summary>
    /// <value>The ships.</value>
    public GameObject[] Ships {
        set => _ships = value; get => _ships;
    }
    /// <summary>
    /// Player's name
    /// </summary>
    public string Name { get => _playerName; set => _playerName = value; }
    /// <summary>
    /// The current state of the player.
    /// </summary>
    public PLAYERSTATE State => _currentState;

    // We are giving 2 methods for changing the state because refactoring is hard. (Joking, we are just lazy bones :P)
    /// <summary>
    /// Changes the current state to the param that it's given
    /// </summary>
    /// <param name="state"></param>
    public void SetState(PLAYERSTATE state) => _currentState = state;
    /// <summary>
    /// Changes the current state to the param that it's given
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(PLAYERSTATE next) => _currentState = next;
    /// <summary>
    /// The current ship. If you consider modifying this variable, use SetShipInstance() instead.
    /// </summary>
    public GameObject CurrentShip
    {
        get
        {
            return _currentShip;
        }
    }

    /// <summary>
    /// Time left of the current turn of the player.
    /// </summary>
    public float TimeLeft {
        get
        {
            return _time;
        }
    }

    /// <summary>
    /// The current camera that returns this player manager to the GameManager so it sets to the main camera.
    /// </summary>
    public GameObject CurrentCamera => _currentShip ? _currentShip.GetComponent<ShipManager>().CurrentCamera : zenitalCameraMovement;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        durationOfTurns++;
        _time = durationOfTurns;
        for (int i = 0; i < _ships.Length; i++)
        {
            _ships[i].GetComponent<ShipManager>().SetPlayer(this);
        }
        AssignShipsParent();
    }

    private void Update()
    {
        StateMachine();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Process the player state machine.
    /// </summary>
    private void StateMachine()
    {
        switch (_currentState)
        {
            case PLAYERSTATE.SELECTING_SHIP:
                if (_currentShip)
                {
                    ChangeState(PLAYERSTATE.MOVE);
                    Logger.Controlling(this, _currentShip.GetComponent<ShipManager>());
                }
                break;

            case PLAYERSTATE.MOVE:
                if (_currentShip)
                {
                    ShipManager manager = _currentShip.GetComponent<ShipManager>();

                    if (manager.CurrentState == SHIPSTATE.WAIT)
                    {
                        manager.SetState(SHIPSTATE.MOVEMENT);
                    }

                    if (manager.CurrentState == SHIPSTATE.ATTACK)
                    {
                        _currentState = PLAYERSTATE.BATTLE;
                    }
                }
                break;
            case PLAYERSTATE.BATTLE:
                if (_currentShip)
                {
                    ShipManager manager = _currentShip.GetComponent<ShipManager>();

                    if (manager.CurrentState == SHIPSTATE.WAIT)
                    {
                        _currentState = PLAYERSTATE.PASS_TURN;
                        _currentShip = null;
                        _time = durationOfTurns;
                    }
                    
                    if (manager.CurrentState == SHIPSTATE.MOVEMENT)
                    {
                        _currentState = PLAYERSTATE.MOVE;
                    }
                }
                break;

            case PLAYERSTATE.WAIT:
                if (_currentShip)
                    _currentShip = null;
                break;

            case PLAYERSTATE.PASS_TURN:
                PassTurn();
                break;

        }
    }

    /// <summary>
    /// Substracts to the current time left.
    /// </summary>
    /// <param name="timeToSubstract">Time to substract.</param>
    public void SubstractTimeMove(float timeToSubstract)
    {
        _time -= timeToSubstract;
        if (_time <= 0)
        {
            PassTurn();
        }
    }

    /// <summary>
    /// Handles all the turn passing. It's all up to you if you decide to use this function outside
    /// of this class.
    /// </summary>
    public void PassTurn()
    {
        ChangeState(PLAYERSTATE.PASS_TURN);
        if (_currentShip != null)
            _currentShip.GetComponent<ShipManager>().SetState(SHIPSTATE.WAIT);
        _time = durationOfTurns;
        _currentShip = null;
    }

    /// <summary>
    /// Sets the current ship instance.
    /// </summary>
    /// <param name="ship_">Ship.</param>
    public void SetShipInstance(GameObject ship_)
    {
       if (_currentState == PLAYERSTATE.SELECTING_SHIP)
       {
            int index = IndexShipInstance(ship_.GetInstanceID());
            _currentShip = _ships[index];
       }
    }

    /// <summary>
    /// Returns the Ship Index of the id.
    /// </summary>
    /// <returns>The ship instance.</returns>
    /// <param name="ID">Identifier.</param>
    private int IndexShipInstance(int ID)
    {
        for(int i = 0; i < _ships.Length; i++)
        {
            if(_ships[i].GetInstanceID() == ID)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Deletes the ship of specified <paramref name="id"/>.
    /// Please, pass gameObject.GetInstance(), not this.GetInstance() or it
    /// will bug the entire program.
    /// </summary>
    /// <param name="id">Identifier.</param>
    public void DeleteShip(int id, bool isCurrentShip = false)
    {
        // Probably we won't let the player destroy its ship in his turn but just in-case.
        if (isCurrentShip || (_currentShip != null && id == _currentShip.GetInstanceID()))
        {
            _currentShip = null;
            if (_currentState != PLAYERSTATE.WAIT)
            {
                _currentState = PLAYERSTATE.PASS_TURN;
            }
        }

        if (_ships.Length > 0)
        {
            ResizeShipsArray(id);
        }  
        
    }

    /// <summary>
    /// Deletes the ship of the passed <paramref name="shipParam"/>.
    /// </summary>
    /// <param name="shipParam">Ship gameobject.</param>
    public void DeleteShip(GameObject shipParam)
    {
        ResizeShipsArray(shipParam.GetInstanceID());
    }

    /// <summary>
    /// Resizes the ships array, deleting the ship that is correspondent to the <paramref name="id"/>.
    /// If you got an OutOfBounceException or Not Found error: make sure that the id that you pass to this function is gameObject.GetInstanceID()
    /// NOT GETINSTANCEID() BY ITSELF because it will pass the instance id of the component.
    /// </summary>
    /// <param name="id">Identifier.</param>
    private void ResizeShipsArray(int id)
    {
        GameObject[] newShips = new GameObject[_ships.Length - 1];
        bool found = false;
        for (int i = 0, j = 0; i < _ships.Length; i++)
        {
            int currentId = _ships[i].GetInstanceID();
            
            if (currentId != id)
            {
                newShips[j] = _ships[i];
                j++;
            }
            else if (currentId == id)
            {
                found = true;
            }

            if (_ships.Length == 1 && currentId == id)
            {
                // Right here the player lost.
                _ships = newShips;
                return;
            }

            if (i == _ships.Length - 1 && !found)
            {
                Debug.LogError("Error finding the ship!");
                return;
            }
        }

        _ships = newShips;
    }

    /// <summary>
    /// Assign every ship this player.
    /// </summary>
    private void AssignShipsParent()
    {
        for (int i = 0; i < _ships.Length; i++)
        {
            ShipManager _sm = _ships[i].GetComponent<ShipManager>();
            _sm.PlayerID = gameObject.GetInstanceID();
        }
    }

    #endregion

    #region Debug
    private void ShowInstancesId()
    {
        for (int i = 0; i < _ships.Length; i++)
        {
            Debug.Log(_ships[i].GetInstanceID());
        }
    }
    #endregion
}
