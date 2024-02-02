using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovements : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float speed = 5.0f;
    private int waypointIndex = 0;

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[waypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        
        if (transform.position == targetWaypoint.position)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }
}
