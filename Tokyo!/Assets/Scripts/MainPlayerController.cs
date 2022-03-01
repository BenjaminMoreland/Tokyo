using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Literally all of this is made by Ryan

public class MainPlayerController : MonoBehaviour
{
    //speed and movement variables
    public float speed;
    public float airSpeed;
    public float moveInputH;

    //Laser spawner
    public GameObject Laser;
    public float Cooldown = 0.2f;
    float Timer = 0;
    public float LaserSpeed = 15;
    public Vector3 Offset1 = new Vector3(.07f, 1f, 0);
    public Vector3 Offset2 = new Vector3(-.07f, 1f, 0);
    public float FireError = 1f;

    //screen shake things
    public FollowCamera FC;
    public float FireShakeTime = 0.1f;
    public float FireShakeMagnitude = 0.1f;

    //grab this to adujust physics
    private Rigidbody2D myRb;
    private Collider2D myCollider;

    //used for checking what direction to be flipped
    private bool facingRight = true;

    //things for ground checking
    private bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump things
    public int extraJumps = 1;
    private int jumps;
    public float jumpForce;
    private bool jumpPressed = true;

    private float jumpTimer = 0;
    public float jumpTime = 0.2f;

    public float gravityScale = 5;

    public float groundDrag = 5;
    public float airDrag = 1;

    private AudioSource myAud;
    public AudioClip jumpNoise;
    public AudioClip deathNoise;

    //ladder things
    private bool isClimbing;
    public LayerMask whatIsLadder;
    public float ladderDist;
    private float moveInputV;
    public float climbSpeed;

    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    //animation
    private Animator myAnim;

    //use this to turn on and off player controls
    private bool controlOn = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zapper")) 
            {
            StartCoroutine(OnDeath());
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        FC = FindObjectOfType<FollowCamera>();

        jumps = extraJumps;

        RespawnPoint = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (controlOn)
        {
            Timer += Time.deltaTime;
            //increase the timer based on time passed
            if (Timer > Cooldown && (Input.GetMouseButtonDown(1)))
            {
                //animator settings
                myAnim.SetBool("Shooting", true);
                //reset the timer
                Timer = 0;
                //fire the lasers
                Fire(Offset1);
                FC.TriggerShake(FireShakeTime, FireShakeMagnitude);
            }
            else if (Timer > Cooldown && !(Input.GetMouseButtonDown(1)))
            {
                myAnim.SetBool("Shooting", false);
            }

            //check for ground
            moveInputH = Input.GetAxisRaw("Horizontal");
            if (isGrounded == true)
            {
                jumps = extraJumps;
            }
            //check if jump can be triggered
            if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && isGrounded == true && isClimbing == false)
            {
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
            }
            else if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && jumps > 0 && isClimbing == false)
            {
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
                jumps--;
            }
            else if (Input.GetAxisRaw("Jump") == 0)
            {
                jumpPressed = false;
                jumpTimer = 0;
            }
            else if (jumpPressed == true && jumpTimer < jumpTime)
            {
                jumpTimer += Time.deltaTime;
                myRb.drag = airDrag;
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                jumpPressed = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (controlOn)
        {
            //check for ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            //set animators on ground
            myAnim.SetBool("OnGround", isGrounded);

            //ladder things


            moveInputV = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
            //check for the ladder if around the player
            RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, ladderDist, whatIsLadder);

            //if ladder was found see if we are climbing, stop falling
            if (hitInfo.collider != null)
            {
                myRb.gravityScale = 0;
                isClimbing = true;
                if (moveInputV > 0)
                {
                    myRb.AddForce(new Vector2(0, climbSpeed));
                }
                else if (moveInputV < 0)
                {
                    myRb.AddForce(new Vector2(0, -climbSpeed));
                }
                else
                {
                    myRb.velocity = new Vector2(myRb.velocity.x, 0);
                }
            }
            else
            {
                myRb.gravityScale = gravityScale;
                isClimbing = false;
            }

           
            //horizontal movement
            moveInputH = Input.GetAxisRaw("Horizontal");
            //animator settings
            if (moveInputH == 0)
            {
                myAnim.SetBool("Moving", false);
            }
            else
            {
                myAnim.SetBool("Moving", true);
            }

            if (isGrounded && !jumpPressed || isClimbing)
            {
                myRb.drag = groundDrag;
                myRb.AddForce(new Vector2(moveInputH * speed, 0));
            }
            else
            {
                myRb.drag = airDrag;
                myRb.AddForce(new Vector2(moveInputH * airSpeed, 0));
            }
            //check if we need to flip the player direction
            if (facingRight == false && moveInputH > 0)
                Flip();
            else if (facingRight == true && moveInputH < 0)
            {
                Flip();
            }
        }
    }
    void Fire(Vector3 offset)
    {
        if (facingRight == false)
        {
        //create the object with a position offset and affected by the rotation of the spawner
        Vector3 spawnPos = transform.position + transform.rotation * -offset;
        GameObject clone = Instantiate(Laser, spawnPos, transform.rotation);
            Vector3 Scaler = clone.transform.localScale;
            Scaler.x *= -1;
            clone.transform.localScale = Scaler;
        //set the speed of the clone
        Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
        cloneRb.velocity = -transform.right * LaserSpeed;
        }
        else if(facingRight == true)
        {
            Vector3 spawnPos = transform.position + transform.rotation * offset;
            GameObject clone = Instantiate(Laser, spawnPos, transform.rotation);
            //set the speed of the clone
            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
            cloneRb.velocity = transform.right * LaserSpeed;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Zapper"))
        {
            StartCoroutine(OnDeath());
        }
    }

    private IEnumerator OnDeath()
    {
        //stop moving
        myRb.velocity = Vector2.zero;

        //start death animation
        //make this the name of whatever death animation is on your player.
        //myAnim.Play("");

        //disable gravity
        myRb.gravityScale = 0;

        //disable player controls
        controlOn = false;

        //disable player collision
        myCollider.enabled = false;

        //stop other sound effects and play the death sound effect
        myAud.Stop();
        myAud.PlayOneShot(deathNoise);

        //time it takes for animation to complete
        yield return new WaitForSeconds(1.15f);

        //re enable everything disabled
        myCollider.enabled = true;
        controlOn = true;
        myRb.gravityScale = gravityScale;

        //respawn at designated location
        transform.position = RespawnPoint;
    }
}