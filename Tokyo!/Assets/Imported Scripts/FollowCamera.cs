using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Tooltip("Drag the object from the hierarchy tab into the slot that you wabt the camera to follow.")]
    public GameObject Target;
    [Tooltip("Set between 0 and 1. Kinda how fast it moves to the target."), Range(0,1)]
    public float LerpVal = 0.8f;

    float ShakeTime = 0;
    float ShakeMagnitude = 0;
    //call this function to make the screen shake
    public void TriggerShake(float time, float amount)
    {
        if (ShakeTime < time)
        {
            ShakeTime = time;
        }
        if(ShakeMagnitude < amount)
        {
            ShakeMagnitude = amount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //fixed update runs once per physics frame
    private void FixedUpdate()
    {
        if(Target != null)
        {
            //calculate where the camera is moving towards
            Vector3 newPos = Target.transform.position;
            newPos.z = transform.position.z;
            //lerp towards the camera to make a smoothing effect in the movement
            transform.position = Vector3.Lerp(transform.position, newPos, LerpVal);
        }

        if (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
            Vector3 randDir = Random.insideUnitCircle * ShakeMagnitude;
            transform.position += randDir;
        }
        else
        {
            ShakeMagnitude = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerShake(0.2f, 0.2f);
        }
    }
}