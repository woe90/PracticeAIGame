using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update

        [SerializeField] Transform playerTrans;
    //?? 
                [SerializeField] Rigidbody rb;

            [SerializeField] Transform trans;

    [SerializeField] float distanceOffset;
    [SerializeField] float angleoffSet;

    float radiusofSatisfaction;
    float maxSpeed;
    float turnSpeed;
    private void Start()

    {
        radiusofSatisfaction = 2f;
            maxSpeed = 3f;

        //rb.velocity = new Vector3(1f, 0f, 0f);
    }
    // Update is called once per frame
    void Update()
    {

        RunKinematicArrive();

    }
    private void RunKinematicArrive()
    {//calculate vector towards character

        Vector3 towards = playerTrans.position = trans.position;

        Quaternion targetRotation = Quaternion.LookRotation(towards);

        trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);

        if (towards.magnitude < radiusofSatisfaction)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        //normalize and multiple by max speed
        towards.Normalize();

    

        towards *= maxSpeed* Time.deltaTime;
        rb.velocity = towards;

        //float newX = Trans.position.x + towards x;
        //float newZ = Trans.position.z + towards.z;
        //trans.position = new Vector 3 (newX, trans.position.y, newZ);


    }

}


//rotation does not work with physics, but we don't need deltatime when we use a rigid body using physics