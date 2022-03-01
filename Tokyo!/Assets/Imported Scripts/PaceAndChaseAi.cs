using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceAndChaseAi : MonoBehaviour
{
    [Tooltip("This array stores the pints that the AI should move through when pacing.")]
    public Vector3[] Points = { Vector3.zero, new Vector3 (5, 0, 0) };

    public float PaceSpeed = 10;
    public int CurrentPoint = 0;
    public float ChaseSpeed = 20;
    [Tooltip("This value is how close to a point it should allow to count as close enough.")]
    public float CloseEnough = 0.1f;

    [Tooltip("How close should the target be before chasing it.")]
    public float ChaseDist = 5;

    [Tooltip("drag in the game object you want chased, will only pace without.")]
    public GameObject Target;

    Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if there is a target
        if (Target != null)
        {
            //Check distance to target to see wether you pace or chase
            Vector2 direction = Target.transform.position - transform.position;
            if(direction.sqrMagnitude <= ChaseDist * ChaseDist)
            {
                Chase(direction);
            }
            else
            {
                Pace();
            }
        }
        else
        {
            Pace();
        }
        
    }

    void Pace()
    {
        //check if near CurrentPoint
        Vector3 direction = Points[CurrentPoint] - transform.position;

        if(direction.magnitude <= CloseEnough)
        {
            //if near move to next
            ++CurrentPoint;
            if(CurrentPoint >= Points.Length)
            {
                CurrentPoint = 0;
            }
            direction = Points[CurrentPoint] - transform.position;
        }
        //Set the speed towards the next point
        Vector2 acceleration = direction.normalized * PaceSpeed * Time.fixedDeltaTime;
        myRb.velocity += acceleration;
    }

    void Chase(Vector2 direction)
    {
        //Set the speed towards the next point
        Vector2 acceleration = direction.normalized * ChaseSpeed * Time.fixedDeltaTime;
        myRb.velocity += acceleration;
    }
}
