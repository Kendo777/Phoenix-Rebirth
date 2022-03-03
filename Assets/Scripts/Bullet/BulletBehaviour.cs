using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    #region Private
    private float _damage = 10.0f;
    private int _playerWhoShotId = 0;
    private Color _color;
    #endregion

    #region Properties
    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    public int PlayerWhoShot 
    {
        get
        {
            return _playerWhoShotId;
        }
        set
        {
            _playerWhoShotId = value;
        }
    }

    public Color BulletColor
    {
        set
        {
            _color = value;
            Material material = GetComponent<Renderer>().materials[0];
            material.SetColor("_Color", _color);
        }
        get => _color;
    }

    #endregion

    #region Public
    public void Movement(Quaternion rot, float spreadAngle)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, spreadAngle);
    }
    #endregion

}
