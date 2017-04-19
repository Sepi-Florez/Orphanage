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
    public float minDistToGrnd;
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
public class PlayerControlV2 : MonoBehaviour
{   
    public CameraSettings cameraSettings;
    [Space(10)]
    public MovementSettings movementSettings;

    float gravity, jumpVelocity;
    private Quaternion charTgtRot, camTgtRot;
    private Transform charTrans;

    private float distToGrnd;
    private CapsuleCollider col;
    private Rigidbody rigid;

    float xRot;

    private void Start()
    {
        cameraSettings.myCamera = GetComponentInChildren<Camera>().transform;
        charTrans = transform;
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
        charTrans.Translate(Input.GetAxis("Horizontal") * movementSettings.movSpd * (Input.GetButton("Sprint") ? movementSettings.runMult : 1f) * Time.deltaTime, 0, Input.GetAxis("Vertical") * movementSettings.movSpd * (Input.GetButton("Sprint") ? movementSettings.runMult : 1f) * Time.deltaTime); //move the character forth/ back with Vertical axis:
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = cameraSettings.debugCol;
        //Vector3 hi = new Vector3(0, .5f, 0);
        
        Vector3 ΔA = Quaternion.AngleAxis(-(cameraSettings.clampΔ / 2), transform.right) * transform.forward;
        Vector3 ΔB = Quaternion.AngleAxis((cameraSettings.clampΔ / 2), transform.right) * transform.forward;

        Gizmos.DrawRay(transform.position, ΔA);//θA);
        Gizmos.DrawRay(transform.position, ΔB);//θB);
    }

    /*Quaternion ClampRotationXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2));

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;

        //Mathf.Clamp(xRot, -(Mathf.Deg2Rad * cameraSettings.clampΔ/2), (Mathf.Deg2Rad * cameraSettings.clampΔ / 2)) ; }//{ xRot = Mathf.Tan(0.5f * Mathf.Deg2Rad * Mathf.Clamp(2.0f * Mathf.Rad2Deg * Mathf.Atan(xRot), -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2))); } //{ xRot = Mathf.Clamp(xRot, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2)); }
    }*/
    public void LookRot()
    {
        xRot += Input.GetAxis("Mouse Y") * cameraSettings.Sensitivity.x;

        if (cameraSettings.clampRot) {xRot = Mathf.Clamp(xRot, -(cameraSettings.clampΔ / 2), (cameraSettings.clampΔ / 2));}

        cameraSettings.myCamera.transform.localEulerAngles = new Vector3(-xRot, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSettings.Sensitivity.y, 0);
    }

    private bool isGrnded()
    {
        //m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hit;

        Debug.DrawRay(transform.position - new Vector3(0, col.height / 2, 0), -transform.up);

        if (Physics.SphereCast(transform.position - new Vector3(0, col.height/2f, 0), col.radius, Vector3.down, out hit, distToGrnd + movementSettings.minDistToGrnd))//((col.height / 2f) - col.radius) + minDistToGrnd))
        {
            return hit.distance <= distToGrnd + movementSettings.minDistToGrnd;
        }
        else
        {
            return false;

        }
    }
}