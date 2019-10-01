using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    //Need to revise and use animations

    [SerializeField] private Transform gate;
    [SerializeField] private Transform plate;

    public float speed;
    private bool isOpened = false;
    private Vector3 gateClosedPosition;
    private Vector3 gateOpenedPosition;

    private Vector3 pressed;
    private Vector3 notPressed;

    private void Start()
    {
        gateClosedPosition = gate.transform.position;
        gateOpenedPosition = new Vector3(gate.transform.position.x, 6.5f, gate.transform.position.z);

        notPressed = plate.transform.position;
        pressed = new Vector3(plate.transform.position.x, -0.49f, plate.transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOpened == false)
        {
            Debug.Log("Opening gate");
            isOpened = true;
            gate.transform.position = gateOpenedPosition;
            plate.transform.position = pressed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpened == true)
        {
            Debug.Log("Closing gate");
            isOpened = false;
            gate.transform.position = gateClosedPosition;
            plate.transform.position = notPressed;
        }
    }
}
