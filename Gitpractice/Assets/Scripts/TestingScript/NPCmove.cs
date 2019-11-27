using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCmove : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform _destination;
    NavMeshAgent _navMeshAgent;
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {

        }
        else
        {
            SetDestination();
        }
    }

    // Update is called once per frame
    private void SetDestination()
    {

        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);

        }

    }
}
