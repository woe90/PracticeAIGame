using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAIMove : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Text tc;

    private float moveSpeed;
    private float turnSpeed;

    public static float batteryLife;
    public float energyDecrease;

    private Quaternion targetRotation;
    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 4f;
        turnSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (batteryLife > 0)
        {
            Move();
            UsingBatteryLife();
        }
    }

    private void Move()
    {
        inputVector = Vector3.forward;

        if (inputVector != Vector3.zero)
        {
            // Normalize input vector to standardize movement speed
            inputVector.Normalize();
            inputVector *= moveSpeed;
            rb.velocity = inputVector;

            // Face player along movement vector
            targetRotation = Quaternion.LookRotation(inputVector);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void UsingBatteryLife()
    {
        if(!(batteryLife == 0))
        {
            batteryLife -= Time.deltaTime * energyDecrease;
            tc.text = "Battery: " + batteryLife.ToString(); 
        } 
        else
        {
            tc.text = "Battery: 0";
        }
    }
}
