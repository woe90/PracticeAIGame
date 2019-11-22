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

    private float moveSpeed, turnSpeed;
    private float radiusOfSat;
    private float timer, timeThreshold;
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
    private ArrayList path = new ArrayList();

    // Start is called before the first frame update
    void Start() {
        //StartCoroutine("FindTargetsWithDelay", .2f);
        batteryLife = 100000;
        moveSpeed = 4f;
        turnSpeed = 10f;
        radiusOfSat = 0.2f;
        timer = 0;
        viewAngle = 360;
        obstacleBumpSeed = 0.5f;
        timeThreshold = 1f;
    }

    // Update is called once per frame
    void Update() {
        OnSceneGUI();
        FindVisibleTargets();

        foreach (Transform visibleTarget in visibleTargets) {
            if (openList.Contains(visibleTarget) == false && closedList.Contains(visibleTarget) == false) {
                //print("Adding " + visibleTarget);
                openList.Add(visibleTarget);
            }
        }
        /*print("************* Open List ************");

        foreach(Transform n in openList) {
            print("OpenList: " + n);
        }
        print("************** Closed List *************");
        foreach (Transform n in closedList) {
            print("ClosedList: " + n);
        }
        print("************ Current Target **************");*/

        if (batteryLife > 0 && moveCommand == true) {

            if (openList.Count > 0 && target == null) {
                target = openList[0];
            }
            //print("Current Target: " + target);
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
        if (timer >= timeThreshold) {
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
        print("trans.position.x " + trans.position.x);
        if (trans.position.x >= 11f || trans.position.x <= 8.5f) {
            print("Returning to rail");
            towards = new Vector3(10f, 0f, 0f);
            Move(towards);
        } else {
            print("Moving Forward");
            towards = Vector3.forward;

            Move(towards);
        }
    }
    #endregion

    private void UsingBatteryLife(float energyDecrease) {
        if (!(batteryLife == 0)) {
            batteryLife -= Time.deltaTime * energyDecrease;
            tc.text = "Battery: " + batteryLife.ToString();
        } else {
            tc.text = "Battery: 0";
        }
    }

    void OnCollisionEnter(Collision collision) {
        print("AI Colliding with something");
        if (collision.gameObject.layer == 8) {
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

        if (collision.gameObject.layer == 9) {
            trans.position += trans.up * 0.01f;
        }
    }

    #region AI Detection
    /*IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }*/

    void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
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

    private void AStar(Vector3 targetPos) {
        Node startNode = WorldDecomposer.NodeFromWorldPoint(trans.position);
        Node targetNode = WorldDecomposer.NodeFromWorldPoint(targetPos);

        print("Start " + startNode.worldPosition);
        print("End " + targetNode.worldPosition);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0) {

            int lowestFCost = openList[0].fCost;
            int lowFCostIndex = 0;

            for (int i = 0; i < openList.Count; i++) {
                if ((i + 1) == openList.Count) {
                    Debug.Log("Out of bound");
                } else {
                    if (lowestFCost > openList[i + 1].fCost) {
                        //swap openList[i + 1] to openList[i]
                        lowestFCost = openList[i + 1].fCost;
                        lowFCostIndex = i + 1;
                    } else if (openList[i].fCost == openList[i + 1].fCost) { // If F cost are same, compare H cost
                        if (openList[i].hCost > openList[i + 1].hCost) {
                            lowFCostIndex = i + 1;
                        }
                    }
                }
            }
            Node currentNode = openList[lowFCostIndex];
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in WorldDecomposer.GetNeighbours(currentNode)) {
                if (!neighbour.ground || neighbour.obstacle || closedList.Contains(neighbour)) {
                    continue;
                }

                if (openList.Contains(neighbour)) {
                    int newGCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newGCost < neighbour.gCost) {
                        neighbour.gCost = newGCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode); ;
                        neighbour.fCost = neighbour.gCost + neighbour.hCost;
                        neighbour.parent = currentNode;
                        openList.Add(neighbour);
                    }
                }

                if (!openList.Contains(neighbour)) {
                    neighbour.gCost = currentNode.gCost + GetDistance(currentNode, neighbour); ;
                    neighbour.hCost = GetDistance(neighbour, targetNode); ;
                    neighbour.fCost = neighbour.gCost + neighbour.hCost;
                    neighbour.parent = currentNode;
                    openList.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode) {
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.XPos - nodeB.XPos);
        int dstY = Mathf.Abs(nodeA.YPos - nodeB.YPos);

        return 14 * Mathf.Min(dstX, dstY) + 10 * Mathf.Abs(dstX - dstY);
    }

    #region AI Astar
    /*#region Variables
    // variables that can be accessed / assigned via unity
    // Layer mask so that only the ground is clickable and not the player or any object in game
    public LayerMask whatCanBeClickOn;
    private float maxSpeed;
    private float rotSpeed;
    private float radiusOfSat;

    // Components
    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    // private variables 
    private Vector3 targetPosition;
    private Vector3 target;
    private Vector3 lookAtTarget;
    private Quaternion playerRot;

    ArrayList path = new ArrayList();
    #endregion

    void Start() {
        maxSpeed = 4f;
        rotSpeed = 10f;
        radiusOfSat = 2f;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition();
            AStar();
            MovePlayer();
        }
        //move();
    }

    // Deals with setting the target position of where the mouse click hit from the camera to the object
    // Initalize targetPosition, lookAtTarget, playerRot
    private void SetTargetPosition() {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(myRay, out hit, 10000, whatCanBeClickOn)) {
            targetPosition = new Vector3(hit.point.x, trans.position.y, hit.point.z);
            Debug.Log("Clicking on plane" + targetPosition);
            //lookAtTarget = targetPosition - trans.position;
            //playerRot = Quaternion.LookRotation(lookAtTarget);
        }
    }

    #region Movement
    // Deals with the character movement
    // Uses maxSpeed, radiusOfSat
    private void move() {
        if (lookAtTarget != Vector3.zero && lookAtTarget.magnitude > radiusOfSat && Vector3.Distance(trans.position, targetPosition) > 0.1f) {
            lookAtTarget.Normalize();
            lookAtTarget *= maxSpeed;
            rb.velocity = lookAtTarget;

            playerRot = Quaternion.LookRotation(lookAtTarget);
            trans.rotation = Quaternion.Lerp(trans.rotation, playerRot, rotSpeed * Time.deltaTime);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    void MovePlayer() {
        StartCoroutine(MovePlayerCoroutine());
    }

    // Ensures player moves along a waypoint path as using move without 
    //  coroutine will make the player go straight towards the end without any regards for obstacles
    IEnumerator MovePlayerCoroutine() {
        print(path);
        print(path.Count);
        foreach (Node n in path) {
            print("World Pos " + n.worldPosition);
            target = n.worldPosition;
            lookAtTarget = target - trans.position;
            playerRot = Quaternion.LookRotation(lookAtTarget);

            while (Vector3.Distance(trans.position, target) > .1f) {
                trans.rotation = Quaternion.Lerp(trans.rotation, playerRot, rotSpeed * Time.deltaTime);
                trans.position = Vector3.MoveTowards(transform.position, target, maxSpeed * Time.deltaTime);
                yield return null;
            }
        }
        path.RemoveRange(0, path.Count);
    }
    #endregion*/
    #endregion

}
