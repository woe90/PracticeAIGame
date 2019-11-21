using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public bool obstacle;
    public Vector3 worldPosition;
    public int XPos;
    public int YPos;

    public int fCost;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool blocked, Vector3 worldPos, int x, int y) {
        obstacle = blocked;
        worldPosition = worldPos;
        XPos = x;
        YPos = y;
    }
}
