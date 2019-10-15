using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	    Vector3 targetPosition;
		Vector3 lookAtTarget;
		Quaternion playerRot;
		float rotSpeed = 5;
		float speed = 5;
		float radiusOfSat = 2;
		bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
        	SetTargetPosition();
        }
        if(isMoving == true)
        {
        	speed = 5;
        	transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.deltaTime);
    		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    		//targetPosition.Normalize();
    		if (targetPosition.magnitude < radiusOfSat)
    			isMoving = false;
    			speed = 0;
        }
    }

    void SetTargetPosition()
    {
    	Ray click = Camera.main.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit;

    	if (Physics.Raycast(click, out hit, 2000))
    	{
    		targetPosition = hit.point;
    		lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
    		playerRot = Quaternion.LookRotation(lookAtTarget);
    		isMoving = true;
    	}
    }

}
