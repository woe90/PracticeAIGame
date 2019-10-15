using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private float moveSpeed;
    private float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
        turnSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
