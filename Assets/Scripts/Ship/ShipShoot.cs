using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipColor))]
public class ShipShoot : MonoBehaviour
{
    #region Private
    [SerializeField]
    private GameObject _preBullet;
    public Transform _firePosition;
    public AudioClip beamSound;
    private AudioSource audioSource;
    [SerializeField]
    private float _spreadAngle;
    private bool _canShoot = true;
    private int _currentShoot = 0;
    private ShipManager _sm;
    [SerializeField]
    private float _timeUntilBulletDestroys = 2f;
    [SerializeField]
    private float _damage = 30.0f;
    [SerializeField]
    private float _rangeBetweenShotsS = 1f;
    #endregion

    #region Properties 
    public int CurrentShot
    {
        get
        {
            return _currentShoot;
        }

        set
        {
            _currentShoot = value;
        }
    }

    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            if (_damage <= 10.0f) _damage = 10.0f;
            _damage = value;
        }
    }

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        _sm = GetComponent<ShipManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = beamSound;
    }
    #endregion 

    #region Methods
    /// <summary>
    /// Shoots
    /// </summary>
    public void Shoot()
    {
        if (_canShoot)
        {
            audioSource.Play();
            ShipColor sc = GetComponent<ShipColor>();
            Logger.ShootAction(_sm.PlayerParent, _sm);
            if (sc == null)
            {
                Debug.LogError("Error getting the ship color component.");
                return;
            }
            Quaternion rot = Random.rotation;
            GameObject bullet = Instantiate(_preBullet, _firePosition.position, _firePosition.transform.rotation);
            bullet.GetComponent<BulletBehaviour>().Movement(rot, _spreadAngle);
            BulletBehaviour bh = bullet.GetComponent<BulletBehaviour>();
            // bh.BulletColor = sc.ShipModelColor;
            bh.Damage = _damage;
            bh.PlayerWhoShot = _sm.PlayerID;
            Destroy(bullet, _timeUntilBulletDestroys);
            _canShoot = false;
        }
        else
        {
            StartCoroutine("ShootWaitTime");
        }
    }

    /// <summary>
    /// Resets _canShoot
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootWaitTime()
    {
        yield return new WaitForSeconds(_rangeBetweenShotsS);
        _canShoot = true;
        _currentShoot++;
        StopAllCoroutines();
    }
    #endregion

}
