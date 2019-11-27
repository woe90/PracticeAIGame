using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallTurnon : MonoBehaviour


{
    [SerializeField] GameObject CrossWalk;

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject);
        MeshRenderer MeshComponent = gameObject.GetComponent<MeshRenderer>();

        CrossWalk.SetActive(true);

        MeshComponent.enabled = true;
    }
}