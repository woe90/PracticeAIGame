using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private float moveSpeed;
    private float turnSpeed;
    private Transform thingToPull; // null if nothing, else a link to some pullable crate

    private void Start() {
        moveSpeed = 10f;
        turnSpeed = 10f;
    }

    private void Update() {
        move();
    }

    private void move() {
        Vector3 inputVector = Vector3.zero;

        // Check for movement input
        if (Input.GetKey(KeyCode.W)) {
            inputVector += Vector3.forward;
        } else if (Input.GetKey(KeyCode.S)) {
            inputVector += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputVector += Vector3.left;
        } else if (Input.GetKey(KeyCode.D)) {
            inputVector += Vector3.right;
        }

        if (inputVector != Vector3.zero) {

            // Normalize input vector to standardize movement speed
            inputVector.Normalize();
            inputVector *= moveSpeed;
            rb.velocity = inputVector;

            // Face player along movement vector
            Quaternion targetRotation = Quaternion.LookRotation(inputVector);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    // For grabing boxes and moving them
    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Input.GetKey(KeyCode.E))
        {

        }
    }*/
}
