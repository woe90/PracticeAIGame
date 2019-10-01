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

    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        if (hit.moveDirection.y < -0.3)
        {
            force = new Vector3(0, -0.5f, 0) * movement.gravity * weight;
        }
        else
        {
            force = hit.controller.velocity * pushPower;
        }

        // Apply the push
        body.AddForceAtPosition(force, hit.point);

        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }*/

    /*private void AttemptGrab()
    {
        if (objectToGrab != null)
        {
            objectToGrab.Grab();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IGrabbable grabbableObject = other.GetComponent<IGrabbable>();
        if (grabbableObject != null)
        {
            objectToGrab = grabbableObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IGrabbable>() != null)
        {
            objectToGrab = null;
        }
    }*/

    // For grabing boxes and moving them
    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Input.GetKey(KeyCode.E))
        {

        }
    }*/
}
