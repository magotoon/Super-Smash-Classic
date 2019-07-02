using UnityEngine;
using System.Collections;

public class MovingPlatforms : MonoBehaviour
{

    /*
    to set up the moving platform, attach the script to the platform you want to move.
    create two empty game objects and name them waypoint 1 and 2 (you should be able to 
    add more that two waypoitns). set the first waypoint exactly where the platform is
    and move the second waypoint in the x,y,or z axis until you reach the desired 
    destination of the platform.
    */

    // For moving
    public int Marker = 0;
    public Transform[] Waypoints;
    public float speed = 1;
    // End

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[Marker].transform.position, speed * Time.deltaTime);
        if (transform.position == Waypoints[Marker].transform.position)
        {
            Marker++;
        }
        if (Marker == Waypoints.Length)
        {
            Marker = 0;
        }
    }
}
