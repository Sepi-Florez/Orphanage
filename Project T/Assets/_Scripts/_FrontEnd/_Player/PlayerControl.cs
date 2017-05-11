using UnityEngine;

[System.Serializable]
public class MovementSettings
{
    public float movSpd = 6f;// = 6;
    public float runMult = 2.0f;// = 2.0f;

    [Space(10)]
    public float jumpApex = 3f;//= 3;
    public float secToApex = .5f;// = .5f;

    [Space(10)]
    public float maxDistToGrnd = .1f;
    public float maxAirTimeUnharmed = .25f;
    public float damgPerSecAirTime = 25f;
}

[System.Serializable]
public class CameraSettings
{
    public Transform myCamera;
    public LayerMask ignore;
    public Vector2 Sensitivity;

    public bool clampRot = true;// = true;

    [Range(0f, 360f)]
    public float clampΔ = 180f;// = 180f;
    public float camSmthTime = 5f;// = 5f;

    [Space(10)]
    public Color debugCol;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControl : MonoBehaviour
{   
    public CameraSettings cameraSettings;
    [Space(10)]
    public MovementSettings movementSettings;

    public HealthManager healthManager;

    float gravity, jumpVelocity;
    private Quaternion charTgtRot, camTgtRot;
    private Transform charTrans, lastTrans;

    private float airTime;
    private float distToGrnd;
    private CapsuleCollider col;
    private Rigidbody rigid;
    [HideInInspector]
    public bool running;

    float xRot;

    private void Start()
    {
        cameraSettings.myCamera = GetComponentInChildren<Camera>().transform;
        charTrans = transform;
        lastTrans = transform;
        col = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        gravity = (2 * movementSettings.jumpApex) / Mathf.Pow(movementSettings.secToApex, 2); //((2*a)/t^2)
        jumpVelocity = Mathf.Abs(gravity) * movementSettings.secToApex * rigid.mass; //(|((2*a)/t^2)| * t)

        distToGrnd = col.height - col.center.y;
    }

    private void FixedUpdate()
    {
        rigid.AddForce(-gravity * rigid.mass * transform.up);
        //print(isGrnded());
        if (!isGrnded())
        {
            airTime += Time.deltaTime;
        }
        if (isGrnded())
        {
            if (airTime > movementSettings.maxAirTimeUnharmed)
            {
                healthManager.UpdateHP(-(movementSettings.damgPerSecAirTime * airTime));

            }
            airTime = 0;
        }

        Velocity();
    }

    private void LateUpdate()
    {
        LookRot();

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrnded())
            {
                rigid.velocity += jumpVelocity * transform.up;
            }
        }
        if (Input.GetButton("Sprint"))
        {
            if (isGrnded()) { running = true; }
            else { running = false; }
        }
        else
        {
            running = false;
        }
        charTrans.Translate(Input.GetAxis("Horizontal") * movementSettings.movSpd * (Input.GetButton("Sprint") ? movementSettings.runMult : 1f) * Time.deltaTime, 0, Input.GetAxis("Vertical") * movementSettings.movSpd * (Input.GetButton("Sprint") ? movementSettings.runMult : 1f) * Time.deltaTime); //move the character forth/ back with Vertical axis:
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = cameraSettings.debugCol;
        //Vector3 hi = new Vector3(0, .5f, 0);
        //        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        //Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));

        Vector3 ΔA = Quaternion.AngleAxis(-(cameraSettings.clampΔ / 2), transform.right) * transform.forward;
        Vector3 ΔB = Quaternion.AngleAxis((cameraSettings.clampΔ / 2), transform.right) * transform.forward;

        Gizmos.DrawRay(transform.position, ΔA);//θA);
        Gizmos.DrawRay(transform.position, ΔB);//θB);
    }

    public void LookRot()
    {
        xRot += Input.GetAxis("Mouse Y") * cameraSettings.Sensitivity.x;

        if (cameraSettings.clampRot) {xRot = Mathf.Clamp(xRot, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2));}

        cameraSettings.myCamera.transform.localEulerAngles = new Vector3(-xRot, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSettings.Sensitivity.y, 0);
    }

    public bool isGrnded()
    {
        //m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hit;

        Vector3 feetPos = transform.position - new Vector3(0, (col.height / 2), 0);
        Debug.DrawRay(feetPos, Vector3.down * movementSettings.maxDistToGrnd, Color.blue);

        //if (Physics.SphereCast(feetPos, col.radius, Vector3.down , out hit, movementSettings.maxDistToGrnd , cameraSettings.ignore))//((col.height / 2f) - col.radius) + minDistToGrnd))
        if(Physics.Raycast(feetPos, Vector3.down, out hit, movementSettings.maxDistToGrnd))
        {
            //return true;
            return hit.distance <= distToGrnd + movementSettings.maxDistToGrnd;
        }
        else
        {
            return false;

        }
    }

    public Vector3 Velocity()
    {
        //print(lastTrans.position + "_" + charTrans.position);
        Vector3 vel =  (charTrans.position - lastTrans.position).normalized;
        lastTrans = charTrans;
        //print(vel);
        return vel;
    }
}