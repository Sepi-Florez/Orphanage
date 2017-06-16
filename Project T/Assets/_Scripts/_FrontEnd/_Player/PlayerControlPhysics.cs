using UnityEngine;

[System.Serializable]
public class MovSettings
{
    public float movSpd = 6f;
    public float runMult = 2.0f;
    public float airSpd = 3f;
    public float maxVelocityChange = 10f;

    [Space(10)]
    public float gravity = -20f;
    public float maxJumpApex = 4f;
    public float minJumpApex = 3f;
    //public float jumpApex = 3f;
    public float secToApex = .5f;

    [Space(10)]
    public float jumpPauzeDur = .1f;
    public LayerMask ignore;

    [Space(10)]
    public float maxAirTimeUnharmed = .25f;
    public float damgPerSecAirTime = 25f;
}

[System.Serializable]
public class CamSettings
{
    public bool useCamera = true;
    public Transform myCamera;
    public Vector2 sensitivity;

    public bool clampRot = true;

    [Range(0f, 360f)]
    public float clampΔ = 180f;
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlPhysics : MonoBehaviour
{
    public Rigidbody rigid;
    public Animator animator;
    public CapsuleCollider coll;
    public HealthManager healthManager;
    [Space(10)]
    public CamSettings cameraSettings;
    [Space(10)]
    public MovSettings movementSettings;

    //public HealthManager healthManager;
    float minJumpVel, maxJumpVel;
    private Transform charTrans;
    private bool isGrnded;

    private float jumpTimer, airTime, spd;

    float xRot, zRot;

    private void Start()
    {
        if (cameraSettings.myCamera == null && cameraSettings.useCamera) //In case you forget to assign your Camera
        {
            cameraSettings.myCamera = GetComponentInChildren<Camera>().transform;
        }
        if (rigid == null) //in case you forget to assign the CharacterController
        {
            rigid = GetComponentInChildren<Rigidbody>();
        }

        rigid.freezeRotation = true;
        rigid.useGravity = false;
    }

    /*void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrnded)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, maxJumpVel, rigid.velocity.z);
        }
        print(isGrnded);
    }*/

    private void Update()
    {
        isGrnded = IsGrnded();

        #region Camera

        if (cameraSettings.useCamera)
        {
            LookRot();
        }

        #endregion

        #region Gravity and JumpVelocities 

        //gravity = -(2 * movementSettings.maxJumpApex) / Mathf.Pow(movementSettings.secToApex, 2); //((2*a)/t^2)
        //S = maxJumpVel , U = ?, V = 0, A = -20m/s, T = secToApex
        maxJumpVel = Mathf.Sqrt(2 * Mathf.Abs(movementSettings.gravity) * movementSettings.maxJumpApex); //
        minJumpVel = Mathf.Sqrt(2 * Mathf.Abs(movementSettings.gravity) * movementSettings.minJumpApex);

        //print("gravity = -" + (movementSettings.gravity));
        //print("maxJumpVel = " + (maxJumpVel));
        //print("minJumpVel = " + (minJumpVel));

        #endregion

        #region Movement

        spd = (isGrnded ? (Input.GetButton("Sprint") ? movementSettings.movSpd * movementSettings.runMult : movementSettings.movSpd) : movementSettings.airSpd); //Sets speed according to runMult if you are pressing the Sprint Button.

       Vector3 targVel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //The axes of your input will be stored in move direction
        targVel = transform.TransformDirection(targVel); //Axes gets converted to a direction
        targVel *= spd; //Move direction gets multplied by "speed" before jumping and Gravity so those won't be boosted.

        Vector3 currentVel = rigid.velocity;
        Vector3 velocityChange = (targVel - currentVel);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -movementSettings.maxVelocityChange, movementSettings.maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -movementSettings.maxVelocityChange, movementSettings.maxVelocityChange);
        velocityChange.y = 0;

        #endregion

        #region Jumps

        /*if (Input.GetButtonDown("Jump") && isGrnded)
        {
            rigid.velocity = new Vector3(velocityChange.x, maxJumpVel, velocityChange.z);
        }*/

        if (Input.GetButtonDown("Jump") && isGrnded)
        {
            rigid.velocity = new Vector3(velocityChange.x, maxJumpVel, velocityChange.z);
        }
        // Cancel the jump when the button is no longer pressed
        if (Input.GetButtonUp("Jump") && !isGrnded)
        {
            if (rigid.velocity.y > minJumpVel)
            {
                rigid.velocity = new Vector3(velocityChange.x, minJumpVel, velocityChange.z);
            }
        }


        /*
        if (isGrnded)//If the player is on the ground..
        {
            if (Input.GetButton("Jump"))//is pressing Jump..
            {
                if (jumpTimer > movementSettings.jumpPauzeDur)//and is past the time a jumping pauze takes..
                {
                    //movDir.y = maxJumpVel;//jumpVelocity is set to the maximum Jump velocity.
                    rigid.velocity = new Vector3(velocityChange.x, maxJumpVel, velocityChange.z);
                    jumpTimer = 0f;
                }
            }
            else//and is not pressing Jump..
            {
                jumpTimer += 1 * Time.deltaTime; //Jumping pauze timer (or jumpTimer) will go up.
            }
        }
        else if (Input.GetButtonUp("Jump")) //if player is not grounded and the Jump button is released:
        {
            if (rigid.velocity.y > minJumpVel) //and velocity is currently above the minimum amount..
            {
                rigid.velocity = new Vector3(velocityChange.x, minJumpVel, velocityChange.z); //then the velocity of the palyer will be set to the minimum amount
            }
        }
        */
        #endregion

        #region Falling Damage
        if (!isGrnded)
        {
            airTime += Time.deltaTime;
        }
        if (isGrnded)
        {
            if (airTime >= movementSettings.maxAirTimeUnharmed)
            {
                if (healthManager != null)
                {
                    healthManager.UpdateHP(-(movementSettings.damgPerSecAirTime * airTime));
                }
                else{ print("ERROR NO HEALTHMANAGER"); } //commented cuz no healthmanager
            }
            airTime = 0;
        }
        #endregion

        #region Animations

        if (!isGrnded)
        {
            animator.SetFloat("moveY", (Mathf.Abs(rigid.velocity.y)));
        }
        else
        {
            animator.SetFloat("moveY", 15);
        }

        if (isGrnded)
        {
            animator.SetFloat("walkSpd", ((Input.GetButton("Sprint") ? 2 : ((Mathf.Ceil(Mathf.Abs(Input.GetAxis("Horizontal"))) > 0f) || (Mathf.Ceil(Mathf.Abs(Input.GetAxis("Vertical"))) > 0f)) ? 1 : 0)));
        }
        else 
        {
            animator.SetFloat("walkSpd", 0);
        }
        //print(((Input.GetButton("Sprint") ? 2 : ((Mathf.Ceil(Mathf.Abs(Input.GetAxis("Horizontal"))) > 0f) || (Mathf.Ceil(Mathf.Abs(Input.GetAxis("Vertical"))) > 0f)) ? 1 : 0)));

        //print(Mathf.Ceil(Mathf.Abs(Input.GetAxis("Horizontal"))));
        //print(Mathf.Abs(movDir.y));

        #endregion

        rigid.AddForce(velocityChange, ForceMode.VelocityChange);

        rigid.AddForce(new Vector3(0, -movementSettings.gravity, 0));
    }

    public void LookRot()
    {
        xRot += Input.GetAxis("Mouse Y") * cameraSettings.sensitivity.x;
        //zRot += Input.GetAxis("Horizontal") * 3f;
        //print(Input.GetAxis("Mouse X"));

        if (cameraSettings.clampRot) { xRot = Mathf.Clamp(xRot, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2)); }

        cameraSettings.myCamera.transform.localEulerAngles = new Vector3(-xRot, 0, zRot);
        transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSettings.sensitivity.y, 0);
    }

    /*public bool IsGrnded()
    {
        RaycastHit hit;
        return (Physics.SphereCast((transform.position + coll.center), coll.height / 2, -transform.up, out hit, 0.1f));
    }*/
    public bool IsGrnded()
    {
        RaycastHit hit;
        //return (Physics.Raycast((transform.position + coll.center), -transform.up, out hit, (coll.height / 2) * 1.1f));
        return(Physics.SphereCast(transform.position + coll.center, coll.radius , -transform.up, out hit, (coll.height /2) * 1.1f));
    }
}

/*
using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class CharacterControls : MonoBehaviour {
 
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;
 
	void Awake () {
	    rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;
	}
 
	void FixedUpdate () {
	    if (grounded) {
	        // Calculate how fast we should be moving
	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
 
	        // Jump
	        if (canJump && Input.GetButton("Jump")) {
	            rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
	        }
	    }
 
	    // We apply gravity manually for more tuning control
	    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
 
	    grounded = false;
	}
 
	void OnCollisionStay () {
	    grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
} 
*/