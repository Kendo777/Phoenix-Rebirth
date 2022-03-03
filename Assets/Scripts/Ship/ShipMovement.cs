using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ship script that makes the movement.
/// </summary>
/// <additionalnotes>
/// We use a basic acceleration movement, then we set it to rb.velocity
/// The x and z rotation depends on the velocity but remember that
/// that function only works with values not with directions...
/// </additionalnotes>

public class ShipMovement : MonoBehaviour
{

    #region Private variables
    private Vector3 _velocity = new Vector3(0f, 0f, 0f);
    private Rigidbody _rb;
    private GameObject _myPlayer;
    private PlayerManager _sm;
    private float _modelRotation = 0f;
    private float _turnSpeed = 0f;
    private float _currentRotation = 0f;
    private float _timerSine = 0.0f;
    private float _timerCos = 0.0f;
    private float _angularVel = 0.0f;
    #endregion

    #region Public variables
    public AxisMobile axis;
    public int timeConsumingSpeed = 3;
    public GameObject model = null;
    public float maxSpeed;      
    public float accR = 3f;
    public float turnAcceleration    = 3f;
    public float turnMaxSpeed = 3f;
    public float acceleration = 5f;
    public float angularAcceleration = 90.0f;
    public float angularSpeed = 90.0f;
    public bool startHitByBullet = false;
    public float speedDestroyed = 3f;
    public float angularSpeedDestroyed = 3f;
    public Vector3 rotation;
    #endregion

    #region Properties 
    public Vector3 Velocity => _velocity;
    public float VerticalSpeed => _velocity.y;
    public float HorizontalSpeed => _velocity.x;
    public float ForwardSpeed => _velocity.z;
    #endregion 

    #region Monobehaviour

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _modelRotation = model.transform.localEulerAngles.y;
        _currentRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        ReactToBullet();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Reacts to a bullet hit. Inform this instance to make ReactToBullet by setting startHitByBullet = true;
    /// </summary>
    private void ReactToBullet()
    {
        if (startHitByBullet)
        {
            if (HitByBullet())
            {
                startHitByBullet = false;
            }
        }
    }

    /// <summary>
    /// Movement with acceleration.
    /// </summary>
    public void MovementWithAcceleration()
    {
        float dt = Time.deltaTime;
        // Declare local input variables
        float inputHorizontal = 0f;
        float inputForward = 0f;
        float inputVertical = 0f;
        float inputRotation = 0f;

        // Get user's input
        inputHorizontal = axis.GetAxis("Horizontal");
        inputForward    = axis.GetAxis("Forward");
        inputVertical   = axis.GetAxis("Vertical");
        inputRotation   = axis.GetAxis("Rotation");
        Debug.Log(inputHorizontal);
        // Substracting to the player the time left depending on the highest input...
        float inputHAbs = Mathf.Abs(inputHorizontal);
        float inputVAbs = Mathf.Abs(inputVertical);
        float inputFAbs = Mathf.Abs(inputForward);
        float max = Mathf.Max(inputHAbs, inputVAbs, inputFAbs);
        _sm.SubstractTimeMove(max * timeConsumingSpeed * dt);
        Acceleration(new Vector4(inputHorizontal, inputVertical, inputForward, inputRotation));
    }

    /// <summary>
    /// Deccelerate
    /// </summary>
    public void Decceleration ()
    {
        Acceleration(Vector4.zero);
    }

    /// <summary>
    /// Acceleration to the specified inputs.
    /// </summary>
    /// <param name="inputs">Inputs.</param>
    private void Acceleration (Vector4 inputs)
    {
        // Acceleration on movement
        Vector3 velTarget = new Vector3(maxSpeed * inputs.x, inputs.y * maxSpeed, maxSpeed * inputs.z);
        Vector3 velOffset = velTarget - _velocity;
        velOffset = Vector3.ClampMagnitude(velOffset, acceleration * Time.deltaTime);
        _velocity += velOffset;
        // Now we redirection the velocity to the actual orientation of the ship.
        Vector3 directionedVelocity = _velocity.z * transform.forward + transform.right * _velocity.x + _velocity.y * Vector3.up;
        // Now we set the directioned velocity to the Rigidbody.
        _rb.velocity = directionedVelocity;
        // Rotate the ship depending on the velocity ( No directioned velocity for better usage )
        RotationDependingOnVelocity(_velocity);
        // Rotate on Y axis depending on the inputRotation
        RotationOnYAxis(inputs.w);
    }

    /// <summary>
    /// Rotation depending on velocity.
    /// </summary>
    /// <param name="velocity__">Velocity.</param>
    void RotationDependingOnVelocity(Vector3 velocity__)
    {
        Vector3 Objective;
        Objective = new Vector3(_velocity.z, 0f, -velocity__.x);
        model.transform.localRotation = Quaternion.Euler(Objective);
    }

    /// <summary>
    /// Deprecated.
    /// </summary>
    public void Rotate()
    {
        float inputRotation = Input.GetAxis("Rotation");
        float targetRotationSpeed = angularSpeed * inputRotation;
        float angularVelOffset = targetRotationSpeed - _angularVel;
        angularVelOffset = Mathf.Clamp(angularVelOffset, -angularAcceleration * Time.deltaTime, angularAcceleration * Time.deltaTime);
        _angularVel += angularVelOffset;
        transform.eulerAngles += new Vector3(0f, _angularVel * Time.deltaTime, 0f);

    }

    /// <summary>
    /// Rotation on Y axis accelerated.
    /// </summary>
    /// <param name="input">Input.</param>
    private void RotationOnYAxis(float input)
    {
        float dt = Time.deltaTime;
        float velTarget = input * turnMaxSpeed;
        float velOffset = velTarget - _turnSpeed;
        velOffset = Mathf.Clamp(velOffset, -turnAcceleration * dt, turnAcceleration * dt);
        _turnSpeed += velOffset;
        _currentRotation += _turnSpeed;
        transform.localEulerAngles = new Vector3(0, _currentRotation, 0);
        if (Mathf.Abs(_currentRotation) >= 360)
            _currentRotation = 0;
    }

    /// <summary>
    /// on collision enter method.
    /// </summary>
    /// <param name="_">.</param>
    private void OnCollisionEnter(Collision _)
    {
        if (_rb != null) 
        {
            _rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Sets the player parent so we can substract the time left on PlayerManager.
    /// </summary>
    /// <param name="plr">Plr.</param>
    public void SetPlayerParent(PlayerManager plr)
    {
        _sm = plr;
    }

    /// <summary>
    /// Movement when the ship is waiting.
    /// </summary>
    public void WaitMovement()
    {
        _timerSine += Time.deltaTime;
        if (_timerSine > 2f)
        {
            _timerSine = 0.0f;
        }
        Vector3 _ = model.transform.localPosition;
        model.transform.localPosition = MUtil.SineAtY(0.25f, _.x, _.y, _.z, _timerSine, 1f);
    }

    /// <summary>
    /// Movement that reacts to a hit of a bullet
    /// </summary>
    /// <returns><c>true</c>, if move finished, <c>false</c> otherwise.</returns>
    public bool HitByBullet()
    {
        _timerCos += Time.deltaTime;
        if (_timerCos > 0.5f)
        {
            _timerCos = 0.0f;
            return true;
        }
        Vector3 _ = model.transform.localPosition;
        model.transform.localPosition = MUtil.CosAtX(0.3f, _.x, _.y, _.z, _timerCos, 4f);
        return false;
    }

    /// <summary>
    /// Moves this instance when destroyed
    /// </summary>
    public void MoveDestroy()
    {
        float dt = Time.deltaTime;
        transform.position += transform.forward.normalized * dt * speedDestroyed;
        transform.eulerAngles += transform.right.normalized * angularSpeedDestroyed + transform.forward.normalized * angularSpeedDestroyed;
        if (transform.eulerAngles.x > 360f || transform.eulerAngles.x < -360f)
        {
            Vector3 euler = transform.eulerAngles;
            euler.x = 0f;
            transform.eulerAngles = euler;
        }
    }
    #endregion Methods

    #region Thrash Code.

    /////
    /// /// <summary>
    /// Gets input, velocity vector and speed and returns/calculates next frame speed.
    /// </summary>

    //float Acceleration(float input,  float spd, float acc, bool decc = true, float maxSpeed_ = -1)
    //{
    //    if (maxSpeed_== -1f) 
    //        maxSpeed_ = maxSpeed;
    //    if (SHIPSTATE.MOVEMENT == currentState) 
    //        spd += input * Time.fixedDeltaTime * acc;

    //    if (System.Math.Abs(input) < 0.001f && decc)
    //    {
    //        if(spd > 0.2f)
    //        {
    //            spd -= Time.fixedDeltaTime * acc;
    //        }
    //        else if(spd < -0.2f)
    //        {
    //            spd += Time.fixedDeltaTime * acc;
    //        }
    //        else
    //        {
    //            spd = 0;
    //        }
    //    }
    //    #region 

    //    #endregion
    //    spd = Mathf.Clamp(spd, -maxSpeed_, maxSpeed_);
    //    return spd;
    //}

    /// <summary>
    /// The movement of the ship
    /// </summary>
    /// <params>
    /// void
    /// </params>
    /// <returns>
    /// void
    /// </returns>
    //void Movement()
    //{
    //    // Get Input Axis.
    //    float inputHorizontal = 0f;
    //    float inputForward = 0f;
    //    float inputVertical = 0f;
    //    float inputRotation = 0f;
    //    // Can't move if the current state is WAIT.
    //    if (SHIPSTATE.MOVEMENT == currentState)
    //    {
    //        inputHorizontal = Input.GetAxis("Horizontal");
    //        inputForward    = Input.GetAxis("Forward");
    //        inputVertical   = Input.GetAxis("Vertical");
    //        inputRotation = Input.GetAxis("Rotation");
    //    }

    //    Vector3 forwardVelocity = transform.forward * spdY;
    //    Vector3 rightVelocity = transform.right * spdX;
    //    Vector3 verticalVelocity = transform.up * spdVertical;
    //    body.velocity = forwardVelocity + rightVelocity + verticalVelocity;
    //    spdX            = Acceleration(inputHorizontal,  spdX, accH);
    //    spdY            = Acceleration(inputForward,     spdY, accH);
    //    spdVertical     = Acceleration(inputVertical,    spdVertical, accV);
    //    currentRotation = Acceleration(inputRotation,    currentRotation, accR, false, 360);
    //    if (Mathf.Abs(currentRotation) >= 360)
    //        currentRotation = 0;
    //    model.transform.localRotation= Quaternion.Euler(new Vector3(spdY, modelRotation, -spdX));
    //    transform.localRotation = Quaternion.Euler(new Vector3(0, currentRotation, 0));

    //}
    /// 
    /// 
    /// 
    ///                   ****** UNUSED FUNCTIONS *******
    ///         Code Snippets that we might use later (probably not)
    /// 
    /// <summary>
    /// Getting the horizontal degrees of the ship, it calculates the next degree that we need
    /// </summary>
    /// <params>
    /// float input
    /// </params>
    /// <returns>
    /// float nextDegree
    /// </returns>

    //float GetDegreesH(float input)
    //{    

    //    if (input > 0.001f)
    //    { 
    //        degreeH += 1 * currentTimeH;
    //        if (currentTimeH < 0)
    //            currentTimeH = 0f;
    //        currentTimeH += Time.fixedDeltaTime;

    //    }
    //    else if (input < -0.001f)
    //    {
    //        degreeH += 1 * currentTimeH;
    //        if (currentTimeH > 0)
    //            currentTimeH = 0f;
    //        currentTimeH -= Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        if (System.Math.Abs(degreeH) > 0.0001f)
    //        {
    //            currentTimeH = 0f;
    //            if (currentTimeH > 1f)
    //                currentTimeH -= Time.fixedDeltaTime * 0.5f;
    //            else if (currentTimeH < -0.01)
    //                currentTimeH += Time.fixedDeltaTime * 0.5f;
    //            if (degreeH > 1f)
    //                degreeH -= 1;
    //            else if (degreeH < 0f)
    //                degreeH += 1;
    //        }
    //    }
    //    degreeH = Mathf.Clamp(degreeH, -maxDegreesH, maxDegreesH);
    //    return degreeH;
    //}

    ///// <summary>
    /////  I'm trying to create here an easing rotating. Please don't touch the function or use it.
    ///// </summary>

    //float GetDegreesHBounceEaseOut(float time, float input)
    //{

    //    inp = Input.GetAxis("Horizontal");
    //    float dg = Mathf.Clamp(BounceEaseOut(currentTimeH, DecideDegree(degreeH), maxDegreesH * input, time), 0, maxDegreesH * input);
    //    degreeH = dg;
    //    print(dg);
    //    //print();
    //    if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.001f)
    //    {
    //        currentTimeH += Time.fixedDeltaTime;
    //    }
    //    else
    //    { 
    //        currentTimeH = 0;
    //    }

    //    return degreeH;
    //}

    ///// <summary>
    ///// Returns if it's ended or if it is in the beginning. If it's not in one of them returns 0.
    ///// </summary>

    //float DecideDegree(float degree)
    //{
    //    //if(System.Math.Abs(degree) < 0.1f || System.Math.Abs(maxDegreesH - Mathf.Abs(degree)) < 0.001f)
    //    //{
    //    //    return degree;
    //    //}
    //    return Mathf.Clamp(degree, 0, maxDegreesH * Input.GetAxis("Horizontal"));
    //}

    //// GABRIEL: This function can be made in one. But right now it's working
    //// and I don't wanna touch it anymore until next session
    //// of programming

    //float GetDegreesV(float input)
    //{

    //    if (input > 0.0001f)
    //    {
    //        degreeV +=  1 * currentTimeV;
    //        if (currentTimeV < 0)
    //            currentTimeV= 0f;
    //        currentTimeV += Time.fixedDeltaTime;

    //    }
    //    else if (input < -0.0001f)
    //    {
    //        degreeV += 1 * currentTimeV;
    //        if (currentTimeV > 0)
    //            currentTimeV = 0f;
    //        currentTimeV -= Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        if (System.Math.Abs(degreeV) > 0.001f)
    //        {
    //            if (degreeV > 1f)
    //                degreeV -= 1;
    //            else if (degreeV < 0f)
    //                degreeV += 1;
    //        }
    //    }
    //    degreeV = Mathf.Clamp(degreeV, -maxDegreesV, maxDegreesV);
    //    return degreeV;
    //}

    ///// <summary>
    ///// The Rotation of the ship depending on the mouseX and mouseY [ Under construction ]
    ///// </summary>
    ///// <params>
    ///// void
    ///// </params>
    ///// <returns>
    ///// void
    ///// </returns>

    //void Rotation()
    //{
    //    float inputMouseX = Input.GetAxis("Mouse X");
    //    float inputMouseY = Input.GetAxis("Mouse Y");

    //    _rotationY += inputMouseX * sensivityX;
    //    _rotationX += inputMouseY * sensivityY;
    //    //Debug.Log(_rotationY);

    //    transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0);
    //}

    //// We ain't gonna use this to be honest

    //public static float BounceEaseOut(float t, float b, float c, float d)
    //{
    //    if ((t /= d) < (1 / 2.75f))
    //    {
    //        return c * (7.5625f * t * t) + b;
    //    }
    //    else if (t < (2 / 2.75))
    //    {
    //        return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
    //    }
    //    else if (t < (2.5f / 2.75f))
    //    {
    //        return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
    //    }
    //    else
    //    {
    //        return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
    //    }
    //}

    //if(inputHorizontal > 0.01f)
    //{
    //    if(velocity.x < -0.001f)
    //    {
    //        spdX -= Time.fixedDeltaTime * acc;
    //    }
    //    else
    //    {
    //        spdX += Time.fixedDeltaTime * acc;
    //    }

    //}
    //else if(inputHorizontal < -0.01f)
    //{
    //    if(velocity.x > 0.001f)
    //    {
    //        spdX -= Time.fixedDeltaTime * acc;
    //    }
    //    else
    //    {
    //        spdX -= Time.fixedDeltaTime * acc;
    //    }
    //}
    //else
    //{
    //   if(spdX > 0) spdX -= Time.fixedDeltaTime * acc;
    //   else spdX += Time.fixedDeltaTime * acc;
    //}
    #endregion ThrashCode
}
