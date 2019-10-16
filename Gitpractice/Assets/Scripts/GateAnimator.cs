using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    bool doorOpen;

    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorOpen = true;
            DoorControl("Open");
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorControl("Close");
        }
    }

    void DoorControl(string direction)
    {
        animator.SetTrigger(direction);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
