using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public bool ground;
    public bool obstacle;
    public Vector3 worldPosition;
    public int XPos;
    public int YPos;

    public int fCost;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkable, bool blocked, Vector3 worldPos, int x, int y) {
        ground = walkable;
        obstacle = blocked;
        worldPosition = worldPos;
        XPos = x;
        YPos = y;
    }
}
