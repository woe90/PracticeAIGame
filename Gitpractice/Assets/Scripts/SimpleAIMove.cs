using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAIMove : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Text tc;

    //Temporary use this
    [SerializeField] private Transform target;
    private Vector3 towards;

    private float moveSpeed;
    private float turnSpeed;
    private float radiusOfSat;
    private float timer;

    public static float batteryLife;
    public float energyDecrease;

    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 4f;
        turnSpeed = 10f;
        radiusOfSat = 0.2f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (batteryLife > 0)
        {
            if(target != null)
            {
                MoveToTarget();                
            }
            else
            {
                MoveForward();
            }
            
        }
    }

    private void Move(Vector3 towards)
    {
        if (towards != Vector3.zero && towards.magnitude > radiusOfSat)
        {
            towards.Normalize();
            towards *= moveSpeed;
            rb.velocity = towards;

            targetRotation = Quaternion.LookRotation(towards);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
            UsingBatteryLife(5f);
        }
        else
        {
            rb.velocity = Vector3.zero;
            //target = null;
            Wait();
        }
    }

    private void Wait()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        UsingBatteryLife(2f);
        if (timer >= 10)
        {
            target = null;
            timer = 0;
        }
    }

    private void MoveToTarget()
    {
        towards = new Vector3(target.position.x - trans.position.x, 0f, target.position.z - trans.position.z);//target.position - trans.position;
        //towards = target.position - trans.position;
        //lookAtTarget = targetPosition - trans.position;
        

        Move(towards);
        
    }

    private void MoveForward()
    {
        if(trans.position.x >= 10f)
        {
            towards = new Vector3(-10f, 0f, 0f);
            Move(towards);
        } else
        {
            towards = Vector3.forward;
            Move(towards);
        }       
    }

    private void UsingBatteryLife(float energyDecrease)
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
