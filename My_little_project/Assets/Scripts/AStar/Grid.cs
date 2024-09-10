using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public bool onlyDisplayPath;
    public LayerMask unwalkableLM;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreteGrid();
    }

    public int MaxSize
    {
        get
        { return gridSizeX * gridSizeY; }
    }

    void CreteGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeDiameter);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableLM));
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }
    public List<Node> GetNeighbooringNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    public Node NodeFromFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) /gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) /gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    public List<Node> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(onlyDisplayPath)
        {
            if(path != null)
            {
                foreach(Node node in path)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                Node playerNode = NodeFromFromWorldPoint(player.position);
                foreach (Node node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    if (playerNode == node)
                    {
                        Gizmos.color = Color.cyan;
                    }
                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}
