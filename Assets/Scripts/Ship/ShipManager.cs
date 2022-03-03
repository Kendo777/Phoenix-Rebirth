using UnityEngine;

[RequireComponent(typeof(ShipShoot))]
[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(ShipClickManager))]
public class ShipManager : MonoBehaviour
{
    #region Public Variables 
    public SHIPSTATE starterState = SHIPSTATE.MOVEMENT;
    public SHIPSTATE currentState = SHIPSTATE.MOVEMENT;
    public GameObject cameraMove;
    public GameObject cameraBattle;
    public string shipName = "Special Ship";
    // Number of shots for the ship
    public int shootNum;
    public float fireRate;
    #endregion

    #region Private variables
    private float nextFire = 0.0f;
    private GameObject _currentCamera;
    private ShipClickManager _scm;
    private ShipMovement _sm;
    private PlayerManager _pm;
    private ShipShoot _ss;
    private int _idPlayer;
    private bool _currentlyShooting = false;
    private bool _alreadyShot = false;
    #endregion

    #region Properties
    public int PlayerID
    {
        get
        {
            return _idPlayer;
        }
        set
        {
            _idPlayer = value;
        }
    }

    public PlayerManager PlayerParent => _pm;

    public SHIPSTATE CurrentState => currentState;
    public GameObject CurrentCamera => _currentCamera;
    #endregion

    #region Monobehaviour

    private void Start()
    {
        currentState = starterState;
        _sm = GetComponent<ShipMovement>();
        _scm = GetComponent<ShipClickManager>();
        _scm.SetManager(this);
        _ss = GetComponent<ShipShoot>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case SHIPSTATE.WAIT:
                _alreadyShot = false;
                if (Mathf.Abs(_sm.Velocity.magnitude) > 0.01f)
                {
                    _sm.Decceleration();
                }
                _sm.WaitMovement();
                _currentCamera = cameraMove;
                FreezeRigidbody(true);
                break;
            case SHIPSTATE.ATTACK:
                if (!_alreadyShot)
                {
                    _currentCamera = cameraBattle;
                    if (_sm.HorizontalSpeed == 0.0f && _sm.ForwardSpeed == 0.0f)
                    {
                        if (_ss.CurrentShot >= shootNum)
                        {
                            // Resets.
                            _ss.CurrentShot = 0;
                            // Pass state wait.
                            currentState = SHIPSTATE.MOVEMENT;
                            _currentlyShooting = false;
                            _alreadyShot = true;
                            return;
                        }

                        if (Input.GetAxis("Fire1") != 0.0f || _currentlyShooting)
                        {

                            if (!_currentlyShooting)
                            {
                                _currentlyShooting = true;
                            }
                            _ss.Shoot();
                        }
                        else
                        {
                            _sm.Rotate();
                        }

                    }
                    else
                    {
                        _sm.Decceleration();
                    }
                }
                break;
            case SHIPSTATE.MOVEMENT:
                FreezeRigidbody(false);
                _currentCamera = cameraMove;
                _sm.MovementWithAcceleration();
                if (Input.GetKey(KeyCode.T) && !_alreadyShot)
                {
                    SetState(SHIPSTATE.ATTACK);
                }
                break;
            case SHIPSTATE.DESTROYED:
                _sm.MoveDestroy();
                Destroy(gameObject, 4f);
                break;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the next state.
    /// </summary>
    /// <param name="next">Next.</param>
    public void SetState(SHIPSTATE next)
    {
        currentState = next;
    }

    /// <summary>
    ///  Sets the player parent to the ship movement so it can substract the time left of movement.
    /// </summary>
    /// <param name="plyr">Plyr.</param>
    public void SetPlayer(PlayerManager plyr)
    {
        _pm = plyr;
        GetComponent<ShipMovement>().SetPlayerParent(plyr);
    }

    /// <summary>
    ///  Clicked.
    /// </summary>
    public void Clicked()
    {
        _pm.SetShipInstance(gameObject);
    }

    /// <summary>
    /// Calls player manager to destroy this ship.
    /// </summary>
    public void DestroyFromManager()
    {
        _pm.DeleteShip(gameObject.GetInstanceID());
    }

    /// <summary>
    /// Freezes or unfreezes the rigidbody. Never unfreezes the rotation.
    /// </summary>
    /// <param name="freeze">If set to <c>true</c> freeze else unfreeze except the rotation.</param>
    public void FreezeRigidbody(bool freeze = true)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (freeze)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            // always freeze rotation.
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    #endregion
}
