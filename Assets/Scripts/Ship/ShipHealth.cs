using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipManager))]
public class ShipHealth : MonoBehaviour
{
    #region Public

    #endregion


    #region Private
    [SerializeField]
    private float _maxHealth;
    private float _currentHealth;
    private ShipManager _sm;
    private ShipMovement _sM;
    private ShipPieceDestroy _spd;
    [SerializeField]
    private GameObject _particleOnDestroy;
    #endregion

    #region Properties

    public float Health
    {
        get
        {
            return _currentHealth;
        }
    }

    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        _sm = GetComponent<ShipManager>();
        _sM = GetComponent<ShipMovement>();
        _spd = GetComponent<ShipPieceDestroy>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        CheckDeath();   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletBehaviour bulletHit = collision.gameObject.GetComponent<BulletBehaviour>();
            if (_sm.PlayerID != bulletHit.PlayerWhoShot) {
                // Receive damage.
                _currentHealth -= bulletHit.Damage;
                Logger.ReceiveDamage(_sm.PlayerParent, _sm, (int)bulletHit.Damage, (int)_currentHealth);
                // Make ship react to the bullet
                _sM.startHitByBullet = true;
                // Destroy bullet.
                //GameObject expfx = collision.gameObject.GetComponent<BulletMovement>().explosion;
                //GameObject self = Instantiate(expfx, transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                //Destroy(self, 1.0f);
            }
        }
    }


    #endregion

    #region Methods
    /// <summary>
    /// Checks if the ship is dead. If it is, activate all the destroyed behaviours.
    /// </summary>
    private void CheckDeath()
    {
        if (_currentHealth <= 0f && _sm.CurrentState != SHIPSTATE.DESTROYED)
        {
            Logger.Destroyed(_sm.PlayerParent, _sm);
            if (gameObject.CompareTag("MotherShip"))
            {
                _sm.PlayerParent.lostMothership = true;
            }
            _sm.DestroyFromManager();
            _spd.Destroy();
            _sm.SetState(SHIPSTATE.DESTROYED);
            Destroy(GetComponent<Rigidbody>());
        }
    }

    /// <summary>
    /// OnDestroy.
    /// </summary>
    //private void OnDestroy()
    //{
    //    GameObject expfx = _particleOnDestroy;
    //    GameObject self = Instantiate(expfx, transform.position, Quaternion.identity);
    //    Destroy(self, 1.0f);
    //}
    #endregion

}
