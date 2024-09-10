using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class AStarPlayerMovement : NetworkBehaviour
{
    public static AStarPlayerMovement Instance;
    public Grid grid;
    public int currentIndex = 0;

    public bool canWalk = false;
    public int speedMod = 1;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Update()
    {
        if(canWalk || Input.GetKeyDown(KeyCode.M))
        {
            MoveOnPath();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerColl")
        {
            canWalk = false;
            currentIndex = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCollExit")
            canWalk = true;
    }
    public void MoveOnPath()
    {
        if(!canWalk)
            canWalk = true;

        if (currentIndex > grid.path.Count || grid.path.Count == 0)
        {
            currentIndex = 0;
            canWalk = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, grid.path[currentIndex].worldPos, Time.deltaTime * speedMod);
        
        if (Vector3.Distance(transform.position, grid.path[currentIndex].worldPos) <= 0.1)
        {
            currentIndex++;
        }

        if (Vector3.Distance(transform.position, grid.path[grid.path.Count-1].worldPos) <= 1)
        {
            canWalk = false;
        }

    }

}
