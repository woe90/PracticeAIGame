using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pitfall : MonoBehaviour
{
    [SerializeField] GameObject CrossWalk;
    [SerializeField] GameObject pitfall;

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject);

        CrossWalk.SetActive(true);
        pitfall.SetActive(false);
        SimpleAIMove.pitfall = false;
    }
}
