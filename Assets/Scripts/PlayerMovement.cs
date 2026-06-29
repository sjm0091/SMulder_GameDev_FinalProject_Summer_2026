
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    public Vector2 rotateInput;
    private CharacterController characterController;
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 0.5f;

    float gravity = -9.8f;
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
        
        Debug.Log("toMove: " + movement);
        rb.linearVelocity = movement;

        
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        rotateInput = value.Get<Vector2>();
    }
}
