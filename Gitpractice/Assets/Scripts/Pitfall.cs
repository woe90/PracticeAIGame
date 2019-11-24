using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour


{
    [SerializeField] GameObject CrossWalk;

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject);

        CrossWalk.SetActive(true);
    }
}
