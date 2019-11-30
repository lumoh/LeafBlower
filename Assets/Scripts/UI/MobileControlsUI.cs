using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class MobileControlsUI : MonoBehaviour
{
    public Canvas ControlsCanvas;
    public RectTransform ControlsCanvasRT;
    public RectTransform JoyStickParent;
    public RectTransform OuterJoyStick;
    public RectTransform InnerJoyStick;

    [NonSerialized] public Vector3 JoyVector;

    public static MobileControlsUI instance;

    private float maxJoyMagnitude;
    private bool active;

    // Start is called before the first frame update
    void Awake()
    {
        GlobalEvents.StartLevel.AddListener(setActive);
		GlobalEvents.LoseLevel.AddListener(setInactive);        
		GlobalEvents.WinLevel.AddListener(setInactive);

		instance = this;
        maxJoyMagnitude = (OuterJoyStick.sizeDelta.x - InnerJoyStick.sizeDelta.x) / 2f;
    }

    private void setActive()
    {
        active = true;
        OuterJoyStick.gameObject.SetActive(true);
    }

    private void setInactive()
    {
        active = false;
        OuterJoyStick.gameObject.SetActive(false);

        InnerJoyStick.anchoredPosition = Vector3.zero;
        JoyVector = Vector3.zero;
    }

    private Vector2 mouseToAnchorPosition(Vector3 mousePos)
    {
        Vector2 anchorPos = new Vector2((mousePos.x / Screen.width) * ControlsCanvasRT.sizeDelta.x, (mousePos.y / Screen.height) * ControlsCanvasRT.sizeDelta.y);
        return anchorPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OuterJoyStick.gameObject.SetActive(true);
                Vector3 mousePos = GetTouchPos();
                JoyStickParent.anchoredPosition = mouseToAnchorPosition(mousePos);
            }

            if (GetTouch())
            {
                Vector3 touchPos = GetTouchPos();
                Vector2 touchAnchor = mouseToAnchorPosition(touchPos);

                // Get 0-1 magnitude vector for movement
                Vector3 rawDirection = touchAnchor - JoyStickParent.anchoredPosition;
                rawDirection /= maxJoyMagnitude;
                if (rawDirection.magnitude > 1f)
                {
                    rawDirection.Normalize();
                }
                JoyVector = rawDirection;

                // Position the inner joystick
                Vector3 joyPos = (JoyVector * maxJoyMagnitude);
                joyPos.z = 0;
                InnerJoyStick.anchoredPosition = joyPos;
            }
            else
            {
                JoyStickParent.anchoredPosition = new Vector2(400, 300);
                InnerJoyStick.anchoredPosition = Vector3.zero;
                JoyVector = Vector3.zero;
            }
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
