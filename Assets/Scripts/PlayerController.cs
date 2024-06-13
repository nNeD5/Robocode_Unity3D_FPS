using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playearCamera;
    [SerializeField] float speed = 1;
    [SerializeField] float mouseSensivity = 25;
    [SerializeField] private float gravityForce = 9.8f;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Item[] items;

    private int itemIndex = 0;
    private CharacterController cc;
    private Vector3 dir;
    private float verticalVelocity;
    private float horizontalRotation;
    private float mouseVerticalInput;
    private float currentVerticalRotaion;
    private bool shouldJump;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Jump();
        Move();
        Look();
        Use();
        GetInput();
    }

    private void GetInput()
    {
        dir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );
        horizontalRotation = Input.GetAxisRaw("Mouse X");
        mouseVerticalInput = Input.GetAxisRaw("Mouse Y");
        shouldJump = Input.GetButton("Jump");
    }

    private void Look()
    {
        // left right 
        transform.Rotate(Vector3.up * horizontalRotation * mouseSensivity * Time.fixedDeltaTime);

        // up down
        mouseVerticalInput *= mouseSensivity * Time.fixedDeltaTime;
        currentVerticalRotaion = Mathf.Clamp(currentVerticalRotaion + mouseVerticalInput, -50f, 50f);
        playearCamera.transform.localEulerAngles = Vector3.left * currentVerticalRotaion;
    }

    private void Move()
    {
        dir = dir.normalized;
        dir = transform.TransformDirection(dir);
        dir *= Time.fixedDeltaTime * speed;

        verticalVelocity -= gravityForce * Time.fixedDeltaTime;
        dir.y += verticalVelocity * Time.fixedDeltaTime;

        cc.Move(dir);
    }

    private void Jump()
    {
        if (shouldJump && cc.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void Use()
    {
        if (Input.GetButton("Use"))
        {
            items[itemIndex].Use();
        }
    }
}
