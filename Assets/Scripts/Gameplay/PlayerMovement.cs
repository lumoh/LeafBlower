using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;

    public float speed = 6.0f;
    public float gravity = 20.0f;
    public float turnSpeed = 10f;

    private Vector3 _moveDirection = Vector3.zero;
    private bool _isDead;
    private bool _isGrounded;
    private int _deadzoneMask;
    private int _groundMask;
    private float _yVelocity;
    private MovingPlatform _movingPlatform;

    void Awake()
    {
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.WinLevel.AddListener(handleWinLevel);

        _deadzoneMask = 1 << Layers.DEADZONE;
        _groundMask = 1 << Layers.GROUND;
        _isDead = false;
    }

    private void handleWinLevel()
    {
        _isDead = true;
        _moveDirection = Vector3.zero;
        Taptic.Failure();
    }

    private void handleLoseLevel()
    {
        _isDead = true;
        _moveDirection = Vector3.zero;
        Taptic.Failure();
    }

    void Update()
    {
        setPlatform();
        setDead();
        move();        
    }

    void move()
    {
        if (!_isDead)
        {
            _moveDirection = getMoveVector();
            handleRotation(_moveDirection);
            _moveDirection *= speed;
            if (_moveDirection.magnitude > speed)
            {
                _moveDirection = _moveDirection.normalized * speed;
            }

            if (_isGrounded || characterController.isGrounded)
            {
                _yVelocity = 0;
            }
            else
            {
                _yVelocity -= gravity * Time.deltaTime;                
            }

            _moveDirection.y = _yVelocity;
        }
    }

    private void LateUpdate()
    {
        if (_moveDirection.sqrMagnitude > characterController.minMoveDistance)
        {
            if (_movingPlatform != null)
            {
                _moveDirection += _movingPlatform.GetVelocity();
            }

            characterController.Move(_moveDirection * Time.deltaTime);
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

    private void setDead()
    {
        if (!_isDead)
        {
            _isDead = Physics.Raycast(transform.position, Vector3.down, 0.25f, _deadzoneMask);
            if (_isDead)
            {
                _moveDirection = Vector3.zero;
                GameManager.instance.Analytics.LoseLevel(GameManager.instance.Level.Num, "Fall");
                Taptic.Failure();
                transform.parent = null;
                GlobalEvents.LoseLevel.Invoke();
            }
        }
    }

    private void setPlatform()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, _groundMask);
        if(_isGrounded)
        {
            if (transform.parent != hit.transform.parent)
            {
                transform.parent = hit.transform.parent;
                _movingPlatform = transform.parent.GetComponent<MovingPlatform>();
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 0.1f);
        if (transform.parent != null)
        {
            Handles.Label(transform.position, transform.parent.gameObject.name);
        }
    }
#endif
}
