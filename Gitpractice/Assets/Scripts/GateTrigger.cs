using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    //Need to revise and use animations

    [SerializeField] private Transform gate;

    public float speed;
    private bool isOpened = false;

    Vector3 startPos;
    Vector3 endPos;

    private void Start()
    {
        startPos = gate.transform.position;
        endPos = gate.transform.position + gate.transform.up * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOpened == false)
        {
            Debug.Log("Opening gate");
            isOpened = true;
            Vector3 targetPosition = new Vector3(gate.transform.position.x, 30, gate.transform.position.z);

            float timeStartedLerping = 0f;
            float lerpTime = 1f;
            float currentLerpTime;

            timeStartedLerping += Time.deltaTime;
            if (timeStartedLerping > lerpTime)
            {
                timeStartedLerping = lerpTime;
            }

            //float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageCompleted = timeStartedLerping / lerpTime;

            gate.transform.position = Vector3.Lerp(gate.transform.position, targetPosition, percentageCompleted);

        }
    }
}
