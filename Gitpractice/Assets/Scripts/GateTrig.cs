using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrig : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("isOpen", false);
    }
}
