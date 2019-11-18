using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAIMove : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Text tc;

    //Temporary use this
    [SerializeField] private Transform target;
    private Vector3 towards;

    private float moveSpeed;
    private float turnSpeed;
    private float radiusOfSat;
    private float timer;
    private float obstacleBumpSeed;

    public static float batteryLife;
    public float energyDecrease;
    public static bool moveCommand;

    private Quaternion targetRotation;

    public float viewRadius;
    private float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private List<Transform> visibleTargets = new List<Transform>();
    private List<Transform> openList = new List<Transform>();
    private HashSet<Transform> closedList = new HashSet<Transform>();

    // Start is called before the first frame update
    void Start() {
        //StartCoroutine("FindTargetsWithDelay", .2f);
        batteryLife = 100000;
        moveSpeed = 4f;
        turnSpeed = 10f;
        radiusOfSat = 1f;
        timer = 0;
        viewAngle = 360;
        obstacleBumpSeed = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        OnSceneGUI();
        FindVisibleTargets();

        foreach (Transform visibleTarget in visibleTargets) {
            if (openList.Contains(visibleTarget) == false && closedList.Contains(visibleTarget) == false) {
                print("Adding " + visibleTarget);
                openList.Add(visibleTarget);
            }
        }
        print("************* Open List ************");

        foreach(Transform n in openList) {
            print("OpenList: " + n);
        }
        print("************** Closed List *************");
        foreach (Transform n in closedList) {
            print("ClosedList: " + n);
        }
        print("************ Current Target **************");

        if (batteryLife > 0 && moveCommand == true) {

            if (openList.Count > 0 && target == null) {
                target = openList[0];              
            }
            print("Current Target: " + target);
            if (target != null) {
                MoveToTarget();
            } else {
                MoveForward();
            }

        } else if(batteryLife > 0 && moveCommand == false) {
            UsingBatteryLife(0.5f);
        }
    }

    #region Movement
    private void Move(Vector3 towards) {
        if (towards != Vector3.zero && towards.magnitude > radiusOfSat) {
            towards.Normalize();
            towards *= moveSpeed;
            rb.velocity = towards;

            targetRotation = Quaternion.LookRotation(towards);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
            UsingBatteryLife(5f);
        } else {
            rb.velocity = Vector3.zero;
            if(target != null)
                Wait();
        }
    }

    private void Wait() {
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);
        UsingBatteryLife(2f);
        if (timer >= 5) {
            closedList.Add(target);
            openList.Remove(target);
            target = null;
            timer = 0;
        }
    }

    private void MoveToTarget() {
        towards = new Vector3(target.position.x - trans.position.x, 0f, target.position.z - trans.position.z);//target.position - trans.position;
                                                                                                              //towards = target.position - trans.position;
                                                                                                              //lookAtTarget = targetPosition - trans.position;
        Move(towards);
    }

    private void MoveForward() {
        if (trans.position.x >= 11f || trans.position.x <= 8f) {
            towards = new Vector3(-10f, 0f, 0f);
            Move(towards);
        } else {
            towards += Vector3.forward;
            Move(towards);
        }
    }
    #endregion

    private void UsingBatteryLife(float energyDecrease) {
        if (!(batteryLife == 0))
        {
            batteryLife -= Time.deltaTime * energyDecrease;
            tc.text = "Battery: " + batteryLife.ToString();
        }
        else
        {
            tc.text = "Battery: 0";
        }
    }

    void OnCollisionEnter(Collision collision) {
        print("AI Colliding with something");
        if (collision.gameObject.layer != 8) { 
            print("Return");
            return;
        }

        // Calcualte vector from player to obstacle
        Vector3 toObstacle = collision.gameObject.transform.position - trans.position;
        toObstacle.Normalize();
        toObstacle.y = 0f;

        float dot = Vector3.Dot(trans.right, toObstacle);

        // Obstacle is on the left of the obstacle -> push player right
        if (dot < 0f) {
            trans.position += trans.right * obstacleBumpSeed;
        } else {
            trans.position += trans.right * -1f * obstacleBumpSeed;
        }
    }

    #region AI Detection
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    /*IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }*/

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void OnSceneGUI() {
        foreach (Transform visibleTarget in visibleTargets) {
            Debug.DrawLine(transform.position, visibleTarget.position, Color.red);
        }
    }
    #endregion

}
