using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    #region Public
    public float rotationSpeed = 10.0f;
    public GameObject model;
    public float verticalSpd = 3f;
    public float distance = 10f;
    #endregion

    #region Private
    private float _timerSine = 0.0f;
    private float _randomTimer = 3f;
    private bool _canMove = false;
    private Pos _currentPosition = Pos.SECOND;
    private Vector3[] _positions;
    #endregion

    #region Enums 
    public enum Pos
    {
        FIRST = 0,
        SECOND
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        _randomTimer = Random.Range(0, 4f);
        Vector3 pos = model.transform.localPosition;
        pos.y += distance;
        _positions = new Vector3[] {
            pos,
            model.transform.localPosition
        };
        StartCoroutine(StartCanMove());
    }
    void Update()
    {
        if (_canMove)
        {
            RotatePlanet();
            MoveLerp();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Rotates the planet.
    /// </summary>
    private void RotatePlanet()
    {
        float dt = Time.deltaTime;
        Vector3 euler = transform.eulerAngles;
        euler.y += rotationSpeed * dt;
        transform.eulerAngles = euler;
    }

    /// <summary>
    /// Moves the planet with a Lerp function.
    /// </summary>
    private void MoveLerp()
    {
        _timerSine += Time.deltaTime * verticalSpd;
        if (_timerSine > 1.0f)
        {
            _timerSine = 0.0f;
            _currentPosition = GetOther();
            return;
        }
        Vector3 _ = model.transform.localPosition;
        model.transform.localPosition = Vector3.Lerp(_positions[(int)_currentPosition], _positions[(int)GetOther()], _timerSine);
    }

    private Pos GetOther()
    {
        return _currentPosition == Pos.FIRST ? Pos.SECOND : Pos.FIRST;
    }

    private IEnumerator StartCanMove()
    {
        yield return new WaitForSeconds(_randomTimer);
        _canMove = true;
    }
    #endregion

}
