using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDecomposer : MonoBehaviour {
    public static Node[,] worldData2;

    private static int[] worldSize;
    private static int[] startWorld;

    private static int nodeSize;

    private static int terrainWidth;
    private static int terrainLength;

    private static int rows;
    private static int cols;

    private void Start() {
        SetWorldSizes();

        terrainWidth = 40;
        terrainLength = 40;

        //Any smaller starts dealing with floats
        // Requires more reworking that a navMesh would probably be better
        nodeSize = 1;

        rows = terrainWidth / nodeSize;
        cols = terrainLength / nodeSize;

        worldData2 = new Node[rows, cols];

        DecomposeWorld();
    }

    public static void DecomposeWorld() {
        float startX = -20;
        float startZ = -20;

        float nodeCenterOffset = nodeSize / 2f;

        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {

                float x = startX + nodeCenterOffset + (nodeSize * col);
                float z = startZ + nodeCenterOffset + (nodeSize * row);

                Vector3 startPos = new Vector3(x, 5f, z);
                Vector3 worldPos = new Vector3(x, 0.5f, z);

                // Does our raycast hit anything at this point in the map

                RaycastHit hit;

<<<<<<< Updated upstream
                // Bit shift the index of the layer (9) to get a bit mask
                int layerMask = 1 << 9;
=======
                // Bit shift the index of the layer (8) to get a bit mask
                int obstacleMask = 1 << 8;
                int targetMask = 1 << 9;
                int groundMask = 1 << 10;
>>>>>>> Stashed changes

                // This would cast rays only against colliders in layer 9.

                //Keep for notes
                // But instead we want to collide against everything except layer 9. The ~ operator does this, it inverts a bitmask.
                //layerMask = ~layerMask;

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(startPos, Vector3.down, out hit, Mathf.Infinity, obstacleMask) &&
                    Physics.Raycast(startPos, Vector3.down, out hit, Mathf.Infinity, groundMask)) {
                    print("Hit something at row: " + row + " col: " + col);
                    Debug.DrawRay(startPos, Vector3.down * 5, Color.red, 50000);
                    worldData2[row, col] = new Node(true, true, worldPos, row, col);
                } else if (Physics.Raycast(startPos, Vector3.down, out hit, Mathf.Infinity, groundMask)) {
                    Debug.DrawRay(startPos, Vector3.down * 5, Color.green, 50000);
                    worldData2[row, col] = new Node(true, false, worldPos, row, col);
                } else {
                    Debug.DrawRay(startPos, Vector3.down * 5, Color.yellow, 50000);
                    worldData2[row, col] = new Node(false, false, worldPos, row, col);
                }
            }
        }
    }

    // Gets the grid exact world position 
    public static Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + terrainWidth / 2) / terrainWidth;
        float percentY = (worldPosition.z + terrainLength / 2) / terrainLength;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int y = Mathf.RoundToInt((cols - 1) * percentX);
        int x = Mathf.RoundToInt((rows - 1) * percentY);

        return worldData2[x, y];
    }

    public static List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                // 0,0 is the origin point of the 8 way connectivity
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.XPos + x;
                int checkY = node.YPos + y;

                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < cols) {
                    neighbours.Add(worldData2[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    private void SetWorldSizes() {
        worldSize[0] = 10;
        worldSize[1] = 10;
        worldSize[2] = 10;
        worldSize[3] = 10;
        worldSize[4] = 10;
        worldSize[5] = 10;
        worldSize[6] = 10;
        worldSize[7] = 10;

        startWorld[0] = 10;
        startWorld[1] = 10;
        startWorld[2] = 10;
        startWorld[3] = 10;
        startWorld[4] = 10;
        startWorld[5] = 10;
        startWorld[6] = 10;
        startWorld[7] = 10;
    }
}
