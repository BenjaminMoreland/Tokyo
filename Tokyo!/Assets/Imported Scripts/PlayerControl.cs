using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float Speed = 500;
    public float RotationSpeed = 10;
    Rigidbody2D myRb;

    //Laser spawner
    public GameObject Laser;
    public float Cooldown = 0.2f;
    float Timer = 0;
    public float LaserSpeed = 15;
    public Vector3 Offset1 = new Vector3(.07f, 1f, 0);
    public Vector3 Offset2 = new Vector3(-.07f, 1f, 0);

    public float FireError = 1f;

    //screen shake things
    FollowCamera FC;
    public float FireShakeTime = 0.1f;
    public float FireShakeMagnitude = 0.1f;

    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        FC = FindObjectOfType<FollowCamera>();
        RespawnPoint = transform.position;
    }

    //fixed update runs on physics times0

    private void FixedUpdate()
    {
        //grab input from the user
        float ySpeed = Input.GetAxisRaw("Vertical") * Speed;
        float rotSpeed = Input.GetAxisRaw("Horizontal") * RotationSpeed;

        //Add forces and torque
        myRb.AddForce(transform.right * ySpeed * Time.fixedDeltaTime);
        myRb.AddTorque(-rotSpeed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        //increase the timer based on time passed
        Timer += Time.deltaTime;
        if(Timer > Cooldown && Input.GetAxisRaw("Jump") == 1)
        {
            //reset the timer
            Timer = 0;
            //fire the lasers
            Fire(Offset1);
            Fire(Offset2);
            FC.TriggerShake(FireShakeTime, FireShakeMagnitude);
        }
    }
    //spawns one object with an offset fromt the spawner
    void Fire(Vector3 offset)
    {
        //create the object with a position offset and affected by the rotation of the spawner
        Vector3 spawnPos = transform.position + transform.rotation * offset;
        GameObject clone = Instantiate(Laser, spawnPos, transform.rotation);
        //set the speed of the clone
        Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
        cloneRb.velocity = transform.right * LaserSpeed;
    }
}