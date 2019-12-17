using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [NonSerialized] CharacterController characterController;

    public float speed = 6.0f;
    public float gravity = 20.0f;
    public float turnSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private bool _isDead;
    private int _deadzoneMask;
    private int _groundMask;

    void Awake()
    {
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.WinLevel.AddListener(handleWinLevel);

        _deadzoneMask = 1 << Layers.DEADZONE;
        _groundMask = 1 << Layers.GROUND;
        _isDead = false;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();        
    }

    private void handleWinLevel()
    {
        _isDead = true;
    }

    private void handleLoseLevel()
    {
        _isDead = true;
    }

    private void handleStartLevel()
    {
        StartCoroutine(respawn());      
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(0.25f);
        _isDead = false;
    }

    void Update()
    {
        setPlatform();

        if (!_isDead)
        {            
            _isDead = Physics.Raycast(transform.position, Vector3.down, 0.25f, _deadzoneMask);
            if (_isDead)
            {
                GlobalEvents.LoseLevel.Invoke();
            }
        }

        if(!_isDead)
        {
            if (characterController.isGrounded)
            {
                moveDirection = getMoveVector();

                handleRotation(moveDirection);

                moveDirection *= speed;
                if (moveDirection.magnitude > speed)
                {
                    moveDirection = moveDirection.normalized * speed;
                }
            }
            else
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (moveDirection.sqrMagnitude > Mathf.Epsilon)
            {
                characterController.Move(moveDirection * Time.deltaTime);
            }
        }
    }

    void handleRotation(Vector3 dir)
    {
        if (MobileControlsUI.instance != null && MobileControlsUI.instance.GetTouch() && dir != Vector3.zero)
        { 
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }

    Vector3 getMoveVector()
    {
        Vector3 moveVector = Vector3.zero;
        if(MobileControlsUI.instance != null)
        {
            moveVector = new Vector3(MobileControlsUI.instance.JoyVector.x, 0f, MobileControlsUI.instance.JoyVector.y);

            // account for isometric view
            moveVector = Quaternion.AngleAxis(-135, Vector3.up) * moveVector;
        }
        else
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        }

        return moveVector;
    }

    private void setPlatform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, _groundMask))
        {
            if (transform.parent != hit.transform.parent)
            {
                transform.parent = hit.transform.parent;
            }
        }
    }
}
