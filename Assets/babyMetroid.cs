using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class babyMetroid : MonoBehaviour
{
    private GameObject glass;
    private Animator glassAnimator;
    private GameObject tubeCollider;
    private Transform transform;
    private CircleCollider2D cc;
    private int cracks = 1;

    public float distanceToCircle = 0.75f;
    public float radius = 0.1f;
    public float speed = 0.3f;
    public int TurningMargin = 50;
    private float facingAngle;

    // Use this for initialization
    void Start ()
    {
		glass = GameObject.FindGameObjectWithTag("glass");
        glassAnimator = glass.GetComponent<Animator>();
        tubeCollider = GameObject.FindGameObjectWithTag("tubeCollider");
        tubeCollider.SetActive(true);

        cc = GetComponent<CircleCollider2D>();
        transform = GetComponent<Transform>();
        facingAngle = Random.Range(0,361) * Mathf.Deg2Rad;
        //Debug.Log("facingAngle: " + facingAngle * Mathf.Rad2Deg);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //get sphere position
        Vector3 spherePos = (new Vector3(Mathf.Cos(facingAngle) * distanceToCircle, Mathf.Sin(facingAngle) * distanceToCircle, 0)) + transform.position;
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 2), new Vector3(spherePos.x, spherePos.y, 2), Color.red);

        //get a point on the sphere
        Vector3 waypoint = (Random.onUnitSphere * radius) + spherePos;
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 2), new Vector3(waypoint.x, waypoint.y, 2), Color.green);

        //get position vector
        Vector3 toWaypoint = waypoint - transform.position;

        //calculate new facingAngle
        facingAngle = processFacingAngle(toWaypoint);
        //Debug.Log("facingAngle: " + facingAngle * Mathf.Rad2Deg);

        //travel
        transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "tubeBoundary")
        {
            //Debug.Log("Tube Collide");

            //check if it collides with the tube, if it does, play hit and send a message to the tube to crack.
            cracks++;
            if (cracks >= 7)
            {// if the cracks break the glass then turn off the colliders
                tubeCollider.SetActive(false);
            }
            glassAnimator.SetTrigger("hit");

            float curAngle = facingAngle * Mathf.Rad2Deg;
            float newAngle = (curAngle + Random.Range(180 - TurningMargin, 180 + TurningMargin + 1)) % 360;
            newAngle *= Mathf.Deg2Rad;
            facingAngle = processFacingAngle(new Vector3(Mathf.Cos(newAngle), Mathf.Sin(newAngle), 0));
            //Debug.Log("facingAngle: " + facingAngle * Mathf.Rad2Deg);
        }

        if (col.gameObject.tag == "metroidBoundary")
        {
            float curAngle = facingAngle * Mathf.Rad2Deg;
            float newAngle = (curAngle + Random.Range(180 - TurningMargin, 180 + TurningMargin + 1)) % 360;
            newAngle *= Mathf.Deg2Rad;
            facingAngle = processFacingAngle(new Vector3(Mathf.Cos(newAngle), Mathf.Sin(newAngle), 0));
            //Debug.Log("facingAngle: " + facingAngle * Mathf.Rad2Deg);
        }
    }

    private float processFacingAngle(Vector3 waypoint)
    {

        int quadrant = 1;
        if (waypoint.y > 0)
        {
            if (waypoint.x >= 0)
                quadrant = 1;
            if (waypoint.x < 0)
                quadrant = 2;
        }
        else if (waypoint.y < 0)
        {
            if (waypoint.x >= 0)
                quadrant = 4;
            if (waypoint.x < 0)
                quadrant = 3;
        }

        float angle = Mathf.Atan(waypoint.y/waypoint.x);

        if (quadrant == 2)
            return angle + Mathf.PI;
        if (quadrant == 3)
            return Mathf.PI + angle;
        if (quadrant == 4)
            return angle + (2 * Mathf.PI);
        else
            return angle;
    }
}
