using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region Private
    private Vector3 _vel = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField]
    private float _bulletSpeed = 10.0f;
    #endregion

    #region Public
    public GameObject explosion;

    #endregion

    #region MonoBehaviour
    void Update()
    {
        float dt = Time.deltaTime;
        _vel = transform.forward * _bulletSpeed;
        transform.position += _vel * dt;

    }
    #endregion

    #region Methods

    public void Movement(Quaternion rot, float spreadAngle)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, spreadAngle);
    }

    #endregion
}
