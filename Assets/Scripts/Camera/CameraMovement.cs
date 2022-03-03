using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The god-camera. It follows whatever instance we set it. When you set an instance
/// to follow it will make a smooth transition to that instance.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    #region Public variables

    public float movementDuration = 2.0f;
    public GameObject testing;
    public float cameraSpeed = 5.0f;
    #endregion

    #region Private variables
    private GameObject objective;
    private bool finalPosition = false;
    private float rotPlayer;
    private float timer = 0.0f;
    private Vector3 iniPos;
    private Quaternion iniRot;
   
    #endregion

    #region Monobehaviour

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        timer += dt;
        if (!finalPosition)
        {
            PositionTheCamera();
        }  
    }

    private void Update()
    {
        if (finalPosition)
        {
            transform.rotation = objective.transform.rotation;
            transform.position = objective.transform.position;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Positions the camera by a Lerp function
    /// </summary>
    private void PositionTheCamera()
    {
        if (objective)
        {
            float t = Mathf.Clamp01(timer / movementDuration);
            Vector3 pos = Vector3.Lerp(iniPos, objective.transform.position, t);
            Quaternion rot = Quaternion.Slerp(iniRot, objective.transform.rotation, t);
            transform.position = pos;
            transform.rotation = rot;

            finalPosition = (timer >= movementDuration);
        }
    }

    /// <summary>
    /// Sets the new ObjectiveInstance so the camera can move to.
    /// </summary>
    /// <param name="obj"></param>
    public void SetObjectiveInstance(GameObject obj)
    {
        if (objective != obj)
        {
            iniPos = transform.position;
            iniRot = transform.rotation;
            objective = obj;
            timer = 0.0f;
            finalPosition = false;
            rotPlayer = objective.transform.rotation.x;
        }
    }

    /// <summary>
    /// Returns the current objective.
    /// </summary>
    /// <returns></returns>
    public GameObject GetObjective()
    {
        return objective;
    }
    #endregion 
}