﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class MobileControlsUI : MonoBehaviour
{
    public Canvas ControlsCanvas;
    public RectTransform ControlsCanvasRT;
    public Camera ControlsCamera;
    public RectTransform JoyStickParent;
    public RectTransform OuterJoyStick;
    public RectTransform InnerJoyStick;

    [NonSerialized] public Vector3 JoyVector;
    [NonSerialized] public Vector3 JoyVectorNatural;
    [NonSerialized] public float JoyMagnitude;

    public static MobileControlsUI instance;

    private float maxJoyMagnitude;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        maxJoyMagnitude = (OuterJoyStick.sizeDelta.x - InnerJoyStick.sizeDelta.x) / 2f;
    }

    private Vector2 mouseToAnchorPosition(Vector3 mousePos)
    {
        Vector2 anchorPos = new Vector2((mousePos.x / Screen.width) * ControlsCanvasRT.sizeDelta.x, (mousePos.y / Screen.height) * ControlsCanvasRT.sizeDelta.y);
        return anchorPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OuterJoyStick.gameObject.SetActive(true);
            Vector3 mousePos = GetTouchPos();
            JoyStickParent.anchoredPosition = mouseToAnchorPosition(mousePos);
        }

        if(GetTouch())
        {
            Vector3 touchPos = GetTouchPos();
            Vector2 touchAnchorPos = mouseToAnchorPosition(touchPos);

            JoyVector = touchAnchorPos - JoyStickParent.anchoredPosition;
            JoyVectorNatural = JoyVector;
            if(JoyVectorNatural.magnitude > 1f)
            {
                JoyVectorNatural.Normalize();
            }

            JoyMagnitude = Mathf.Min(maxJoyMagnitude, JoyVector.magnitude);
            JoyVector.Normalize();

            Vector3 joyPos = (JoyVector * JoyMagnitude);
            joyPos.z = 0;
            InnerJoyStick.anchoredPosition = joyPos;
        }
        else
        {
            OuterJoyStick.gameObject.SetActive(false);

            InnerJoyStick.anchoredPosition = Vector3.zero;
            JoyVector = Vector3.zero;
            JoyMagnitude = 0f;
            JoyVectorNatural = Vector3.zero;
        }
    }

    public bool GetTouchDown()
    {
        bool touchDown;
        if(Input.mousePresent)
        {
            touchDown = Input.GetMouseButtonDown(0);
        }
        else
        {
            Touch touch = Input.GetTouch(0);
            touchDown = touch.phase == TouchPhase.Began;
        }

        return touchDown;
    }

    public bool GetTouch()
    {
        bool isTouching = Input.touchCount > 0;

        if(Input.mousePresent)
        {
            isTouching = Input.GetMouseButton(0);
        }

        return isTouching;
    }

    Vector3 GetTouchPos()
    {
        Vector3 pos = Vector3.zero;
        if(Input.mousePresent)
        {
            pos = Input.mousePosition;
        }
        else if(Input.touchCount > 0)
        {
            pos = Input.GetTouch(0).position;
        }
        return pos;
    }
}
