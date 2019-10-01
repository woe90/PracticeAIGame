using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmer: Brannon Sibounheuang
// Error log to fix later since currently unsolvable. Cube takes click and moves to point on the plane and moves in a somewhat smooth way.
// Currently a wiggling situation happens when cube arrives near clicked area. problem is issue with the function running.

public class Player : MonoBehaviour
{
    Vector3 TargetPosition;
    Vector3 lookatTarget;
    Quaternion playerRotation;
    float rotSpeed = 1;
    float speed = 2;
    bool moving = false;

    void Start()
    {
        Debug.Log("Pressed primary button.");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Pressed primary button.");
        // checks for the mouse button input and calls a functionn that will get the location of click
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
            Debug.Log("Pressed primary button.");
        }
        if(moving)
        Movement();
    }

    void SetTargetPosition()
    {
        //sending out a ray to gather click point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
                                            //note for future, can limit infinity to lower numbers
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            TargetPosition = hit.point;
            //below helps us get the angle the click was at, to help with smooth turning
            lookatTarget = new Vector3(TargetPosition.x - transform.position.x,
                transform.position.y,
                TargetPosition.z - transform.position.z);
            playerRotation = Quaternion.LookRotation(lookatTarget);
            moving = true;
            // this boolean will allow for the movement method to happen
        }
    }

    void Movement()
    {       //slerp uses collected information to help with a smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, rotSpeed * Time.deltaTime);
        // moves to the vector of where the click was
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, speed * Time.deltaTime);


        // this is where a problem occurs that looks like it will need to be fixed in the future.
        // we want to set the boolean back to false to stop the method from running, but it seems since the location of the click cannot be reached, you have the cube wiggle around the clicked point
        // I'm not sure what could solve the wiggling while also getting to the clicked point as near as possible.
        // taking out the condition here means you have to hold down the click button for cube to move instead of just do 1 click and a move to happen.
        if (transform.position == TargetPosition)
            moving = false;

    }

        
}
    