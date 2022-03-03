﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public ButtonState buttonType = ButtonState.DownL;
    public GameObject state;
    public string typeOfDirection = "Horizontal";
    public bool testing = true;
    private AxisMobile axis;

    void Start()
    {
        axis = state.GetComponent<AxisMobile>();
        if (!Application.isMobilePlatform)
        {
            // DISABLE
            if (!testing) gameObject.SetActive(false);
        }
    }

    public void TriggerButtonMove()
    {
        axis.SetState(buttonType, typeOfDirection);
    }

    public void Leave()
    {
        axis.SetState(ButtonState.None, typeOfDirection);
    }

}
