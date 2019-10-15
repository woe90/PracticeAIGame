using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private float moveSpeed;
    private float turnSpeed;

    private Collision interactableObj;

    private bool grabbingObj;

    private Quaternion targetRotation;
    private Vector3 inputVector;
    private Vector3 towards;
    private Vector3 playerDirection;
    private float direction;
    private float boxCounter;


    private void Start() {
        moveSpeed = 10f;
        turnSpeed = 10f;
        grabbingObj = false;
        boxCounter = 0;
    }

    private void Update() {

        Move();

        //do toggle grab to see if that fix issue noted in collisonenter
        if (Input.GetKey(KeyCode.E) && !grabbingObj && boxCounter == 0)
        {
            Grab();
        }

        if (Input.GetKeyUp(KeyCode.E) && grabbingObj && boxCounter == 1)
        {
            LetGo();
        }
    }

    //Need to figure out how to make player push and pull box in one direction of the box
    private void Move() {
        

        inputVector = Vector3.zero;

        //if (!Input.GetKey(KeyCode.E))
        //{
            // Check for movement input
            if (Input.GetKey(KeyCode.W))
            {
                inputVector += Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                inputVector += Vector3.back;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector += Vector3.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputVector += Vector3.right;
            }
        //} else {
           /* if (Input.GetKey(KeyCode.W))
            {
                inputVector += Vector3.forward;
            } else if (Input.GetKey(KeyCode.S))
            {
                inputVector += Vector3.back;
            }
        }*/

        if (inputVector != Vector3.zero) {

            // Normalize input vector to standardize movement speed
            inputVector.Normalize();
            inputVector *= moveSpeed;
            rb.velocity = inputVector;

            // Face player along movement vector
            targetRotation = Quaternion.LookRotation(inputVector);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    private void Grab()
    {
        Debug.Log("Grabbing GrabbleBox");
        Debug.Log("Before boxCounter: " + boxCounter);
        Debug.Log(interactableObj);
        grabbingObj = true;
        boxCounter += 1;
        interactableObj.collider.GetComponent<Rigidbody>().mass = 10f;
        interactableObj.collider.GetComponent<FixedJoint>().connectedBody = rb;
        Debug.Log("After boxCounter: " + boxCounter);
    }

    private void LetGo()
    {
        Debug.Log("Letting go of GrabbleBox");
        Debug.Log("Before boxCounter: " + boxCounter);
        Debug.Log("Before object: " + interactableObj);
        grabbingObj = false;
        boxCounter -= 1;
        interactableObj.collider.GetComponent<Rigidbody>().mass = 1000f;
        interactableObj.collider.GetComponent<FixedJoint>().connectedBody = null;
        interactableObj = null;
        Debug.Log("After boxCounter: " + boxCounter);
        Debug.Log("After object: " + interactableObj);
    }

    // Current issue
    // If Player grab an object and collides with another object, the current object the player is grabbing will remain stuck to them
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hitting Something");
        //Debug.Log("Current Object " + interactableObj);

        if (collision.collider.tag == "Ground") { 
            Debug.Log("Colliding with ground");
            return;
        }

        if(collision.collider.tag == "GrabbleBox" && !grabbingObj && boxCounter == 0)
        {
            Debug.Log("Hitting GrabbleBox");
            interactableObj = collision;
            //Debug.Log("Current Object after hitting box " + interactableObj);
            return;
        }

        if (collision.collider.tag == "Battery")
        {
            //Pickup battery by hiding or deleting object
            //Add battery percentage to AI
            Debug.Log("Picked up battery");

            return;
        }
    }
}
