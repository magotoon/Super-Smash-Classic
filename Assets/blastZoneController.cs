using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blastZoneController : MonoBehaviour
{
    public LayerMask[] whatToDestroy;
    public Transform[] spawnPoint;
    public float killRadius;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collider entered");

        System.Random rand = new System.Random();

        collision.gameObject.transform.position = spawnPoint[rand.Next(0, spawnPoint.Length)].position;
        
    }
}
