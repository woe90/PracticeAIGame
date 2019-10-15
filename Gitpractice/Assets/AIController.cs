using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
	[SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private float moveSpeed;
    private float turnSpeed;

    private Transform thingToPull; // null if nothing, else a link to some pullable crate

    private void Start() {
        moveSpeed = 4f;
        turnSpeed = 10f;
    }

    private void Update() {
        move();
        if (Input.GetKey(KeyCode.E)) {
            Debug.Log("Grabbing");
            //AttemptGrab();              //attempt the grab when the key has been pressed

        }
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
}