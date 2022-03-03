using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Zenital Camera Movement.
/// </summary>
public class ZenitalCameraMovement : MonoBehaviour
{
    #region Private
    private float xSpeed = 0.0f;
    private float zSpeed = 0.0f;
    private GameObject cameraFollow;
    #endregion

    #region Public 
    public float cameraSpeed = 3f;
    public AxisMobile axis;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        
    }
    #endregion

    #region Methods
    

    /// <summary>
    /// Moves the camera.
    /// </summary>
    public void Move()
    {
        float dt = Time.deltaTime;
        float xInput = axis.GetAxis("Horizontal");
        float yInput = axis.GetAxis("Forward");

        xSpeed = cameraSpeed * xInput * dt;
        zSpeed = cameraSpeed * yInput * dt;

        transform.position += new Vector3(xSpeed, 0.0f, zSpeed);       
    }

    /// <summary>
    /// Sets what the camera should follow. [[[DEPRECATED]]]
    /// </summary>
    /// <param name="camera"></param>
    public void SetCameraFollow(GameObject camera)
    {
        cameraFollow = camera;
    }

    /// <summary>
    /// Deprecated function
    /// </summary>
    public void Move_________deprecated()
    {
        if (cameraFollow != null)
        {
            if (System.Math.Abs(cameraFollow.transform.position.z - transform.position.z) < 0.1f)
            {
                float xInput = Input.GetAxisRaw("Horizontal");
                float yInput = Input.GetAxis("Forward");

                xSpeed = cameraSpeed * xInput;
                zSpeed = cameraSpeed * yInput;

                transform.position += new Vector3(xSpeed, 0.0f, zSpeed);
            }
            return;
        }
        Debug.LogError("No objective assigned to ZenitalCameraMovement.cs, please assign an objective with SetCameraFollow before calling this function.");
    }
    #endregion
}
