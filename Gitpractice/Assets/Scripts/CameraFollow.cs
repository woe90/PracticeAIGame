using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform trans;
    public Transform target;

	public float smoothSpeed = 0.5f;
	public Vector3 offset;

	void FixedUpdate ()
	{

		Vector3 desiredPosition = target.position + offset;

        //Debug.Log(desiredPosition);
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

		//transform.LookAt(target);
	}
}
