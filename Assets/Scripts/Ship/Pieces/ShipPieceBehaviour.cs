using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set this component to every piece that you want set free when destroyed.
/// Then set it in the root's script called ShipPieceDestroy in the array.
/// </summary>
public class ShipPieceBehaviour : MonoBehaviour
{
    #region Public
    public bool destroyed = false;
    public Vector3 axis = new Vector3(0f, 0f, 1);
    public float speed = 3f;
    public float angularSpeed = 0.3f;
    public float timeUntilDestroy = 4f;
    #endregion

    #region Private
    private float currentAngle = 0f;
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        axis = Vector3.ClampMagnitude(axis, 1.0f);
    }

    private void Update()
    {
        if (destroyed)
        {
            transform.parent = null;
            Move();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Move this instance when destroyed
    /// </summary>
    private void Move()
    {
        float dt = Time.deltaTime;
        transform.position += axis * speed * dt;
        //transform.eulerAngles += new Vector3(0f, 0f, angularSpeed);
        transform.Rotate(Vector3.forward, currentAngle);
        currentAngle += angularSpeed * dt;
        Destroy(gameObject, timeUntilDestroy);
        if (currentAngle > 360f || currentAngle < -360f) currentAngle = 0f;
    }

    #endregion
}
