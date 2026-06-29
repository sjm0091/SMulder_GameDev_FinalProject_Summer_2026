
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    public Vector2 rotateInput;
    public Vector2 jumpCrouchInput;
    public Transform raycastStart;
    private CharacterController characterController;
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float rotationSpeed = 0.5f;

    float gravity = -9.8f;

    bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 rotation = new Vector3(0f, (rotateInput.x + rotateInput.y) / 2, 0f).normalized;
        transform.rotation = Quaternion.Euler(transform.eulerAngles + (rotation * rotationSpeed));
        rb.freezeRotation = false;
        rb.rotation = transform.rotation;
        rb.freezeRotation = true;


        Vector3 movement = (((transform.forward * moveInput.y) + (transform.right * moveInput.x)).normalized * moveSpeed) + new Vector3(0f, rb.linearVelocity.y, 0f);
        
        // Debug.Log("toMove: " + movement);
        rb.linearVelocity = movement;

        if (jumpCrouchInput.y != 0)
        {
            // Debug.Log("jump activated");
            if (jumpCrouchInput.y > 0)
            {
                Jump();
            }
            if (jumpCrouchInput.y < 0)
            {
                Crouch();
            }
        }

        
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        rotateInput = value.Get<Vector2>();
    }

    public void OnJumpCrouch(InputValue value)
    {
        jumpCrouchInput = value.Get<Vector2>();
    }

    public void CheckIfGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.5f))
        {
            isGrounded = true;
        }
    }

    public void Jump()
    {
        CheckIfGrounded();
        Debug.Log("isGrounded: "+isGrounded);
        if (isGrounded)
        {
            Vector3 jump = new Vector3(0f, jumpForce, 0f);
            rb.AddForce(jump, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void Crouch()
    {
        // To implement once I have an animated player
        return;
    }
}
