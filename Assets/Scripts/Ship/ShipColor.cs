using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipColor : MonoBehaviour
{
    #region Public
    #endregion

    #region Private
    [SerializeField]
    private Color _shipColor;
    #endregion

    #region Properties
    public Color ShipModelColor
    {
        get
        {
            return _shipColor;
        }

        set
        {
            _shipColor = value;
        }

    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        //Debug.Log(material.GetComponent<Renderer>().materials[0].GetColor("_Color"));
    }
    #endregion

    #region Methods

    #endregion


}
