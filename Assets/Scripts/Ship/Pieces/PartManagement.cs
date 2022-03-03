using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartManagement : MonoBehaviour
{
    #region Public
    // Start is called before the first frame update
    public GameObject ParentShip;
    // 0, 2, 4... RIGHT. 1, 3, 5... LEFT.
    public Transform[] Wings;
    public float maxRotation = 15f;
    public float rotationSpeed = 2f;
    public bool rotXAxis = true;
    public bool rotYAxis = false;
    public bool rotZAxis = false;
    public bool moveXAxis = false;
    public bool moveYAxis = false;
    public bool moveZAxis = false;
    #endregion

    #region Private
    private ShipMovement parentShipMovement;
    #endregion

    #region MonoBehaviour
    // Begins before Start
    void Awake()
    {
        parentShipMovement = ParentShip.GetComponent<ShipMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ParentShip.GetComponent<ShipManager>().CurrentState != SHIPSTATE.DESTROYED)
        {
            float spdVertical = parentShipMovement.VerticalSpeed;
            for (int i = 0; i < Wings.Length; i++)
            {
                spdVertical = Mathf.Clamp(spdVertical, -maxRotation, maxRotation);
                float rot = spdVertical * rotationSpeed;
                Wings[i].transform.localRotation = Quaternion.Euler(
                    rot * MUtil.BoolF(rotXAxis),
                    rot * MUtil.BoolF(rotYAxis),
                    rot * MUtil.BoolF(rotZAxis)
                );
                spdVertical *= -1;
            }
        }
    }
    #endregion

    void RotateWings()
    {

    }

}
