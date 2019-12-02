using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAndPlatformTrig : MonoBehaviour
{
    [SerializeField] GameObject belt;
    [SerializeField] GameObject platform;

    private void OnTriggerEnter(Collider collision) {
        platform.SetActive(false);
        
    }
}
