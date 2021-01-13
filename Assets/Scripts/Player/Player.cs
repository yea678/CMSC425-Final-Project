using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public float mouseSensitvity = 7;
    public float moveSpeed = 15f;
    public float jumpForce = 13f;
    private CharacterController cc;
    public Camera playerCam;
    private float cameraPitch;
    public bool controlsEnabled;
    private Vector3 initPos;
    private Quaternion initRot;
    private Pickupable currPickup;
    private bool isHolding;
    public WorldState currState;
    public AudioClip footsteps, jump, land;
    private AudioSource walkSFX, jumpSFX, landSFX;
    private float prevPlayTime;
    public GameObject joint;
    private FixedJoint fj;
    private float yVel;
    private bool lastGrounded;
    public float grav = 20f;

    void Start()
    {
        initPos = transform.position;
        initRot = transform.rotation;
        cc = GetComponent<CharacterController>();
        cameraPitch = 0;
        controlsEnabled = false;
        isHolding = false;
        yVel = 0;
        lastGrounded = cc.isGrounded;

        walkSFX = gameObject.AddComponent<AudioSource>(); 
        walkSFX.clip = footsteps;
        jumpSFX = gameObject.AddComponent<AudioSource>(); 
        jumpSFX.clip = jump;
        landSFX = gameObject.AddComponent<AudioSource>(); 
        landSFX.clip = land;
    }

    void Update()
    {
        if(controlsEnabled)
        {
            if (Input.GetKeyDown("r"))
            {
                resetPosition();
            }
            moveCharacter();
        }
        if(isHolding)
        {
            adjustPickupPos();
        }
        checkForFall();
    }

    void FixedUpdate()
    {
        checkForJointBreak();
    }

    private void oldMove()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float velX = rb.velocity.x;
        float velY = rb.velocity.y;
        float velZ = rb.velocity.z;

        float horizontalVel = Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velZ, 2));

        if (Input.GetKeyDown("space") && Mathf.Abs(velY) < 0.5f)
        {
            rb.AddRelativeForce(Vector3.up*500);
        }

        if (Input.GetKey("w"))
        {
            rb.AddRelativeForce(Vector3.forward*15);
        }

        if (Input.GetKey("s"))
        {
            rb.AddRelativeForce(Vector3.back*15);
        }

        if (Input.GetKey("a"))
        {
            rb.AddRelativeForce(Vector3.left*15);
        }

        if (Input.GetKey("d"))
        {
            rb.AddRelativeForce(Vector3.right*15);
        }

        if (!Input.GetKey("w") &&
            !Input.GetKey("s") &&
            !Input.GetKey("a") &&
            !Input.GetKey("d") &&
            horizontalVel <= 8.5f)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (horizontalVel >= 10f)
        {
            rb.velocity = rb.velocity * (10f / rb.velocity.magnitude);
        }
    }

    public void enableControls()
    {
        controlsEnabled = true;
    }

    private void moveCharacter()
    {
        rotateView();
        handlePlayerMovement();
    }

    private void rotateView()
    {
        float horizontal = Input.GetAxisRaw("Mouse X") * mouseSensitvity;
        float vertical = Input.GetAxisRaw("Mouse Y") * mouseSensitvity * -1;

        cameraPitch = Mathf.Clamp(cameraPitch+vertical, -90f, 90f);

        Vector3 playerVec = new Vector3(0, horizontal, 0);
        Vector3 camVec = new Vector3(cameraPitch, 0, 0);

        transform.Rotate(playerVec, Space.Self);
        playerCam.transform.localEulerAngles = camVec;
    }

    private void handlePlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(horizontal, -0.1f, vertical);
        moveVector = Vector3.ClampMagnitude(moveVector, 1) * moveSpeed * Time.deltaTime;
        moveVector = transform.TransformVector(moveVector);

        cc.Move(moveVector);

        if(!walkSFX.isPlaying && (vertical != 0 || horizontal != 0) && cc.isGrounded)
        {   
            prevPlayTime = Time.time;
            walkSFX.Play();
        }
        else if(walkSFX.isPlaying && (Time.time - prevPlayTime) > 1 && (vertical == 0 && horizontal == 0) )
        {
            walkSFX.Stop();
        }

        if(cc.isGrounded && yVel < 0)
        {
            yVel = 0f;
        }

        if(Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            yVel = jumpForce;
            walkSFX.Stop();
            jumpSFX.Play();
        }
        
        yVel += (-grav * Time.deltaTime);
        Vector3 vertVec = Vector3.up * yVel * Time.deltaTime;
        if(vertVec != Vector3.zero)
        {
            CollisionFlags cf = cc.Move(vertVec);

            if((cf & CollisionFlags.Above) != 0)
            {
                yVel = 0;
            }
        }

        lastGrounded = cc.isGrounded;
    }

    private void resetPosition()
    {
        //Need to temporarily disable controller, or it won't teleport
        cc.enabled = false;
        transform.position = initPos;
        transform.rotation = initRot;
        playerCam.transform.localEulerAngles = Vector3.zero;
        cameraPitch = 0;
        cc.enabled = true;
    }

    public void pickup(Pickupable p)
    {
        Rigidbody rb = p.GetComponent<Rigidbody>();
        Collider coll = p.GetComponent<Collider>();
        
        Physics.IgnoreCollision(cc, coll);

        rb.isKinematic = true;
        p.transform.rotation = playerCam.transform.rotation;
        p.transform.position = joint.transform.position;
        rb.isKinematic = false;

        fj = joint.AddComponent<FixedJoint>();
        fj.breakForce = 5000f;
        fj.connectedBody = rb;
        currPickup = p;
        p.setHeld(true);
        isHolding = true;
    }

    public void drop(Pickupable p)
    {
        if(currPickup == p)
        {
            Collider coll = p.GetComponent<Collider>();
            Physics.IgnoreCollision(cc, coll, false);
            isHolding = false;
            p.setHeld(false);
            Destroy(fj);
        }
    }

    private void adjustPickupPos()
    {
        if(currPickup.touchingColliders() == 0)
        {
            currPickup.transform.position = joint.transform.position;
            currPickup.transform.rotation = playerCam.transform.rotation;
        }
    }
    
    private void checkForJointBreak()
    {
        if(fj == null && isHolding)
        {
            drop(currPickup);
        }
    }

    private void checkForFall()
    {
        if(transform.position.y < -5)
        {
            resetPosition();
            Reset.resetAll();
        }
    }
}
