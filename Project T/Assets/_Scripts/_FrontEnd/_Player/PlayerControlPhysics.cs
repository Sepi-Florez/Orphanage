using UnityEngine;
using System.Collections;

[System.Serializable]
public class MovSettings {
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
public class CamSettings {
    public bool useCamera = true;
    public Transform myCamera;
    public Vector2 sensitivity = new Vector2(3, 3);

    public bool clampRot = true;
    [Range(0f, 360f)]
    public float clampΔ = 170f;
    [Space(10)]
    public bool headBobEnabled = true;
    public float headBobSpd = 0.15f;
    public float headBobRotMult = 0.25f;
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlPhysics : MonoBehaviour {
    public Rigidbody rigid;
    public Animator animator;
    public CapsuleCollider coll;
    //public HealthManager healthManager;
    [Space(10)]
    public MovSettings movementSettings;
    [Space(10)]
    public CamSettings cameraSettings;

    //public HealthManager healthManager;
    public static PlayerControlPhysics instance;

    //[HideInInspector]
    //public Vector3 impactForceDirection;

    float minJumpVel, maxJumpVel;
    private Transform charTrans;
    private Vector3 camOriginal;

    private float jumpTimer, airTime, spd;

    float xRot, zRot, bobTimer;

    public SoundManager soundManager;

    private void Start() {
        if (cameraSettings.myCamera == null && cameraSettings.useCamera) //In case you forget to assign your Camera
        {
            cameraSettings.myCamera = GetComponentInChildren<Camera>().transform;
        }
        if (rigid == null) //in case you forget to assign the CharacterController
        {
            rigid = GetComponentInChildren<Rigidbody>();
        }
        camOriginal = GetComponentInChildren<Camera>().transform.localPosition;

        CursorLock.SetPlayerScripts(this);

        rigid.freezeRotation = true;
        rigid.useGravity = false;

        instance = this;
    }

    private void Update() {
        #region Camera

        if (cameraSettings.useCamera) {
            LookRot();
        }

        #endregion

        #region JumpVelocities 

        maxJumpVel = Mathf.Sqrt(2 * Mathf.Abs(movementSettings.gravity) * movementSettings.maxJumpApex);
        minJumpVel = Mathf.Sqrt(2 * Mathf.Abs(movementSettings.gravity) * movementSettings.minJumpApex);

        #endregion

        #region Movement

        spd = (IsGrnded() ? (Input.GetButton("Sprint") ? movementSettings.movSpd * movementSettings.runMult : movementSettings.movSpd) : movementSettings.airSpd); //Sets speed according to runMult if you are pressing the Sprint Button.

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

        if (Input.GetButtonDown("Jump") && IsGrnded()) {
            rigid.velocity = new Vector3(velocityChange.x, maxJumpVel, velocityChange.z);
        }
        // Cancel the jump when the button is no longer pressed
        if (Input.GetButtonUp("Jump") && !IsGrnded()) {
            if (rigid.velocity.y > minJumpVel) {
                rigid.velocity = new Vector3(velocityChange.x, minJumpVel, velocityChange.z);
            }
        }
        #endregion

        #region Falling Damage
        if (!IsGrnded()) {
            airTime += Time.deltaTime;
        }
        if (IsGrnded()) {
            if (airTime >= movementSettings.maxAirTimeUnharmed) {
                HealthManager.UpdateHP(-(movementSettings.damgPerSecAirTime * airTime));
            }
            airTime = 0;
        }
        #endregion

        #region Animations

        if (!IsGrnded()) {
            animator.SetFloat("moveY", (Mathf.Abs(rigid.velocity.y)));
        }
        else {
            animator.SetFloat("moveY", 15);
        }

        if (IsGrnded()) {
            animator.SetFloat("walkSpd", ((Input.GetButton("Sprint") && (((Input.GetAxis("Horizontal")) > 0f) || (Mathf.Ceil(Mathf.Abs(Input.GetAxis("Vertical"))) > 0f))) ? 2 : ((Mathf.Ceil(Mathf.Abs(Input.GetAxis("Horizontal"))) > 0f) || (Mathf.Ceil(Mathf.Abs(Input.GetAxis("Vertical"))) > 0f)) ? 1 : 0));
        }
        else {
            animator.SetFloat("walkSpd", 0);
        }

        animator.SetBool("picking", Input.GetButtonDown("Interaction"));

        #endregion

        rigid.AddForce(velocityChange, ForceMode.VelocityChange);

        rigid.AddForce(new Vector3(0, -movementSettings.gravity, 0));
    }

    public void AddForce(Vector3 fDir) {
        rigid.AddRelativeForce(fDir, ForceMode.Impulse);
        rigid.AddRelativeForce(new Vector3(0, movementSettings.gravity, 0));
    }

    public void LookRot() {
        xRot += Input.GetAxis("Mouse Y") * cameraSettings.sensitivity.x;
        zRot = (cameraSettings.headBobEnabled ? HeadBob() : 0f); //if head bobbing is enabled the HeadBob() function will run.

        if (cameraSettings.clampRot) { xRot = Mathf.Clamp(xRot, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2)); }

        cameraSettings.myCamera.transform.localEulerAngles = new Vector3(-xRot, 0, zRot);
        transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSettings.sensitivity.y, 0);
    }

    public bool IsGrnded() {
        RaycastHit hit;
        //return (Physics.Raycast((transform.position + coll.center), -transform.up, out hit, (coll.height / 2) * 1.1f));
        return (Physics.SphereCast(transform.position + coll.center, coll.radius, -transform.up, out hit, (coll.height / 2) * 1.1f));
    }

    public float HeadBob()
    {
        float waveSlice = 0.0f;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(Input.GetAxis("Vertical")) == 0) {
            bobTimer = 0f;
        }
        else {
            waveSlice = Mathf.Sin(bobTimer);
            bobTimer = bobTimer + (cameraSettings.headBobSpd * (Input.GetButtonDown("Sprint") ? movementSettings.runMult : 1));

            if (bobTimer > Mathf.PI * 2) {
                bobTimer = bobTimer - (Mathf.PI * 2);
            }
        }

        if (waveSlice != 0)
        {
            float change = waveSlice * cameraSettings.headBobRotMult;

            /*if (IsGrnded() && waveSlice >= -1)
            {
                soundManager.SoundLister(Random.Range(2, 4));
                //GetMat.GetMaterial(new Ray(transform.position, -transform.up));
            }*/

            float inputAxes = Mathf.Abs(Input.GetAxis("Horizontal") + Mathf.Abs(Input.GetAxis("Vertical")));
            inputAxes = Mathf.Clamp(inputAxes, 0f, 1f);
            change = inputAxes * change;

            return (change);
        }
        else {
            return (0f);
        }
    }

    #region screenShake
    public static void Shake(float duration, float amount) {
        instance.StartCoroutine(instance.scrShake(duration, amount));
    }

    public IEnumerator scrShake(float duration, float amount) {
        while (duration > 0) {
            cameraSettings.myCamera.localPosition = camOriginal + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        cameraSettings.myCamera.localPosition = camOriginal;
    }
    #endregion
}