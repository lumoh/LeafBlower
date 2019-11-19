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
    bool isDead;

    void Awake()
    {
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.RetryLevel.AddListener(handleRetry);
        GlobalEvents.WinLevel.AddListener(handleWinLevel);
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(9, 10, true);
    }

    private void handleWinLevel()
    {
        isDead = true;
    }

    private void handleLoseLevel()
    {
        isDead = true;
    }

    private void handleRetry()
    {
        transform.position = new Vector3(0, 5, 0);
        StartCoroutine(respawn());      
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(0.25f);
        isDead = false;
    }

    void Update()
    {
        if (!isDead && transform.position.y < 0f)
        {
            isDead = true;
            GlobalEvents.LoseLevel.Invoke();
        }

        if(!isDead)
        {
            if (characterController != null && characterController.isGrounded)
            {
                moveDirection = getMoveVector();

                handleRotation(moveDirection);

                moveDirection *= speed;
                if (moveDirection.magnitude > speed)
                {
                    moveDirection = moveDirection.normalized * speed;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
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
}
