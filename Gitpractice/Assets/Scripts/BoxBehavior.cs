using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour, IGrabbable
{
    FixedJoint joint;

    private void Awake() {
        joint = GetComponent<FixedJoint>();
    }

    public void Grab() {
        joint.autoConfigureConnectedAnchor = true;
    }

    public void Drop() { 
        joint.autoConfigureConnectedAnchor = false;
    }
}
