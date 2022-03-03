using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not used.
/// </summary>
public class CameraHookes : MonoBehaviour
{
    #region Public
    // Public
    public float theta = 30.0f;
    public float phi = 270.0f;
    public float distanceToTarget = 10.0f;
    public float hookesCoeficient = 1.0f;
    public float dampingCoeficient = 1.0f;
    public Transform target;
    public Vector3 targetOffset;
    public Vector3 cameraCentre;

    // Private
    private Vector3 _speed;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        // Time differential
        float dt = Time.deltaTime;

        // Set the resting position of the camera
        Vector3 restPosition = CameraRestPosition();
        Vector3 offsetCameraPosition = restPosition - transform.position;

        // Calculate the parameters for the hookes and damping forces
        Vector3 springForce = hookesCoeficient * offsetCameraPosition;
        Vector3 dampingForce = -dampingCoeficient * _speed;

        // Now set the acceleration of out camera
        Vector3 acceleration = springForce + dampingForce;
        _speed += acceleration * dt;
        transform.position += _speed * dt;

        // Make the camera look at the target
        Vector3 globalTargetOffsetPosition = target.TransformPoint(targetOffset);
        transform.LookAt(globalTargetOffsetPosition, Vector3.up);
    }
    #endregion

    #region Methods

    /// <summary>
    /// Calculate the rest position.
    /// </summary>
    public Vector3 CameraRestPosition()
    {
        Vector3 localRestingPosition = new Vector3();
        Vector3 globalRestingPosition = new Vector3();
        Vector3 globalCameraCentre = new Vector3();

        // Set local position of the camera
        localRestingPosition = SphericalCoordinates();
        localRestingPosition *= distanceToTarget;

        // Transform local positions to global positions
        globalCameraCentre = target.TransformPoint(cameraCentre);

        // Set global positions of the camera
        globalRestingPosition = localRestingPosition.x * target.right + localRestingPosition.y * target.up + localRestingPosition.z * target.forward;
        globalRestingPosition += globalCameraCentre;

        return globalRestingPosition;
    }

    Vector3 SphericalCoordinates()
    {
        float sinTheta = Mathf.Sin(Mathf.Deg2Rad * theta);
        float cosTheta = Mathf.Cos(Mathf.Deg2Rad * theta);
        float sinPhi = Mathf.Sin(Mathf.Deg2Rad * phi);
        float cosPhi = Mathf.Cos(Mathf.Deg2Rad * phi);

        Vector3 sphericalCoordinates = new Vector3(
            sinTheta * cosPhi,
            cosTheta,
            sinTheta * sinPhi
            );

        return sphericalCoordinates;
    }
    #endregion
}