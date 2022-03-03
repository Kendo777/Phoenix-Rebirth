using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  Probably this will be just 
///  the gameManager ingame, not the general manager.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Public variables
    public GameObject[] players; // Maybe Max 2?
    public GameObject currentCamera;
    public GameObject ZenitalCamera;
    #endregion

    #region Private variables
    private GAMESTATE _currentGameState = GAMESTATE.TURN1;
    private ZenitalCameraMovement _cameraMovementZenital;
    private CameraMovement _cameraMove;
    private int _currentTurn = 0;
    private int maxTurn;
    private PlayerManager _won;
    #endregion

    #region Properties
    /// <summary>
    /// The current state of the game (in-game)
    /// </summary>
    public GAMESTATE State
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
        }
    }
    /// <summary>
    /// The winner of the game, it's null if no one is a winner yet.
    /// </summary>
    public PlayerManager Winner => _won;
    /// <summary>
    /// Gets the current player. Returns null if we are on pause mode so if you are using this on update make sure it
    /// doesn't return null.
    /// </summary>
    /// <value>The current player.</value>
    public PlayerManager CurrentPlayer
    {
        get
        {
            if (State == GAMESTATE.PAUSE)
                return null;
            if (players.Length == 1)
                return players[0].GetComponent<PlayerManager>();
            return players[(int)State].GetComponent<PlayerManager>();
        }
    }
    /// <summary>
    /// Gets the current ship.
    /// </summary>
    /// <value>The current ship.</value>
    public GameObject CurrentShip => CurrentPlayer.CurrentShip;
    /// <summary>
    /// Gets the current ship manager.
    /// </summary>
    /// <value>The current ship manager.</value>
    public ShipManager CurrentShipManager => CurrentShip.GetComponent<ShipManager>();
    #endregion

    #region MonoBehaviour
    void Start()
    {
        int redTeam = (int)PlayerTeams.RED;
        int blueTeam = (int)PlayerTeams.BLUE;
        maxTurn = players.Length;
        _cameraMove = currentCamera.GetComponent<CameraMovement>();
        _cameraMovementZenital = ZenitalCamera.GetComponent<ZenitalCameraMovement>();
        _cameraMovementZenital.SetCameraFollow(currentCamera);
        players[redTeam].GetComponent<PlayerManager>().SetState(PLAYERSTATE.SELECTING_SHIP);
        players[redTeam].GetComponent<PlayerManager>().Name = DataTransmission.playerName0;
        players[blueTeam].GetComponent<PlayerManager>().SetState(PLAYERSTATE.WAIT);
        players[blueTeam].GetComponent<PlayerManager>().Name = DataTransmission.playerName1;
    }
    
    void Update()
    {
        StateMachine();
        CheckLoseCondition();
    }
    #endregion MonoBehaviour

    #region Methods
    /// <summary>
    /// Handles the state machine.
    /// </summary>
    private void StateMachine()
    {
        switch (_currentGameState)
        {
            case GAMESTATE.TURN1:
                ActionPlayer(_currentGameState);
                break;
            case GAMESTATE.TURN2:
                #region Commented
                /*PlayerManager managerPlayer2 = players[1].GetComponent<PlayerManager>();
                if (managerPlayer2.State== PLAYERSTATE.WAIT)
                {
                    managerPlayer2.ChangeState(PLAYERSTATE.SELECTING_SHIP);
                }
                if (managerPlayer2.State== PLAYERSTATE.PASS_TURN)
                {
                    managerPlayer2.ChangeState(PLAYERSTATE.WAIT);
                    _currentGameState = GAMESTATE.TURN1;
                }
                if(managerPlayer2.State== PLAYERSTATE.SELECTING_SHIP)
                {
                    // MOVE 
                    Debug.LogWarning("queee");
                    _cameraMovementZenital.Move();
                }
                if (_cameraMove.GetObjective().GetInstanceID() != managerPlayer2.CurrentCamera.GetInstanceID())
                {
                    _cameraMove.SetObjectiveInstance(managerPlayer2.CurrentCamera
                }*/
                #endregion
                ActionPlayer(_currentGameState);
                break;

            case GAMESTATE.PAUSE:
                break;
            case GAMESTATE.LOSE:
                StartCoroutine(LostCoroutine());
                break;
        }
    }

    /// <summary>
    /// Checks lose condition in a fast way.
    /// </summary>
    private void CheckLoseCondition()
    {
        // Do not check if we are not on a turn.
        if (_currentGameState == GAMESTATE.TURN1 || _currentGameState == GAMESTATE.TURN2)
        {
            PlayerManager pm = players[(int)_currentGameState].GetComponent<PlayerManager>();
            GameObject[] ships = pm.Ships;
            // Check current turn ships.
            if (ships.Length <= 0 || pm.lostMothership)
            {
                _won = GetNotPlayingPlayer();
                _currentGameState = GAMESTATE.LOSE;
                return;
            }
            
            // Check not playing turn ships.
            pm = GetNotPlayingPlayer();
            ships = pm.Ships;
            if (ships.Length <= 0 || pm.lostMothership)
            {
                _won = players[(int)_currentGameState].GetComponent<PlayerManager>();
                _currentGameState = GAMESTATE.LOSE;
                return;
            }
            
        }
    }

    private IEnumerator LostCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MenuTestingScene");
    }

    /// <summary>
    /// Returns the player that is not playing. Probably deprecate this method if we are expanding to a 2 < N player game
    /// </summary>
    /// <returns></returns>
    private PlayerManager GetNotPlayingPlayer()
    {
        if (_currentGameState == GAMESTATE.PAUSE || _currentGameState == GAMESTATE.LOSE)
        {
            return null;
        }
        return _currentGameState == GAMESTATE.TURN1 ? players[1].GetComponent<PlayerManager>() : players[0].GetComponent<PlayerManager>();
    }

    /// <summary>
    /// Handles a turn.
    /// </summary>
    /// <param name="currentTurn"></param>
    private void ActionPlayer(GAMESTATE currentTurn)
    {
        // Takes the current player.
        PlayerManager managerPlayer = players[(int)currentTurn].GetComponent<PlayerManager>();
        if (managerPlayer.State == PLAYERSTATE.WAIT)
        {
            Logger.ChangeTurn(managerPlayer);
            managerPlayer.ChangeState(PLAYERSTATE.SELECTING_SHIP);
        }
        // Always set the current camera instance
        if (!_cameraMove.GetObjective())
            _cameraMove.SetObjectiveInstance(managerPlayer.CurrentCamera);
        if (_cameraMove.GetObjective().GetInstanceID() != managerPlayer.CurrentCamera.GetInstanceID())
        {
            _cameraMove.SetObjectiveInstance(managerPlayer.CurrentCamera);
        }
        if (managerPlayer.State == PLAYERSTATE.PASS_TURN)
        {
            managerPlayer.ChangeState(PLAYERSTATE.WAIT);
            _currentGameState = PassTurn();
        }
        if (managerPlayer.State == PLAYERSTATE.SELECTING_SHIP)
        {
            // MOVE 
            _cameraMovementZenital.Move();
        }
        _cameraMove.SetObjectiveInstance(managerPlayer.CurrentCamera);
    }

  
    private GAMESTATE PassTurn()
    {
        if (_currentGameState == GAMESTATE.TURN1)
            return GAMESTATE.TURN2;
        return GAMESTATE.TURN1;
    }
    #endregion 
}
