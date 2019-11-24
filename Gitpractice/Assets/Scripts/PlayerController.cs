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
    private LayerMask mask;

    private bool grabbingObj;
    private bool grabbingObjToggle;
    private bool commandToggle;

    private Quaternion targetRotation;
    private Vector3 inputVector;
    private Vector3 towards;
    private Vector3 playerDirection;
    private float direction;
    private bool isGrounded;
    private float fallMutipler;

    private void Start() {
        moveSpeed = 10f;
        turnSpeed = 10f;
        grabbingObj = false;
        commandToggle = false;
        grabbingObjToggle = false;
        fallMutipler = 3.5f;
    }

    private void Update() {
        if (isGrounded == false) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMutipler - 1) * Time.deltaTime;
        }
        Move();

        //do toggle grab to see if that fix issue noted in collisonenter
        if (Input.GetKeyDown(KeyCode.E)) {
            //Grab();
            grabbingObjToggle = !grabbingObjToggle;

            if (grabbingObjToggle == true) {
                Grab();
            } else if (grabbingObjToggle == false) {
                LetGo();
            }
        }

        // Stop & Go Command for bot
        if (Input.GetKeyDown(KeyCode.R)) {
            commandToggle = !commandToggle;

            if (commandToggle == true) {
                SimpleAIMove.moveCommand = true;
            } else {
                SimpleAIMove.moveCommand = false;
            }
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

        if (inputVector != Vector3.zero && isGrounded == true) {

            // Normalize input vector to standardize movement speed
            inputVector.Normalize();
            inputVector *= moveSpeed;
            rb.velocity = inputVector;

            // Face player along movement vector
            targetRotation = Quaternion.LookRotation(inputVector);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        } else {
            //rb.velocity = Vector3.zero;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void Grab()
    {
        Debug.Log("Grabbing GrabbleBox");
        Debug.Log(interactableObj);
        grabbingObj = true;
        interactableObj.collider.GetComponent<Rigidbody>().mass = 10f;
        interactableObj.collider.GetComponent<FixedJoint>().connectedBody = rb;
    }

    private void LetGo()
    {
        Debug.Log("Letting go of GrabbleBox");
        Debug.Log("Before object: " + interactableObj);
        grabbingObj = false;
        interactableObj.collider.GetComponent<Rigidbody>().mass = 1000f;
        interactableObj.collider.GetComponent<FixedJoint>().connectedBody = null;
        interactableObj = null;
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
            isGrounded = true;
            return;
        }

        if(collision.collider.tag == "GrabbleBox" && !grabbingObj)
        {
            Debug.Log("Hitting GrabbleBox");
            interactableObj = collision;
            //Debug.Log("Current Object after hitting box " + interactableObj);
            isGrounded = true;
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

    private void OnCollisionExit(Collision collision)
    {
        if ((1<<collision.gameObject.layer == 10))
        {
            isGrounded = false;
        }
    }
}
