using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private float juice;
    
    // Start is called before the first frame update
    void Start()
    {
        juice = 35f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 30, 5) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        obj.SetActive(false);
        SimpleAIMove.batteryLife += juice;
    }
}
