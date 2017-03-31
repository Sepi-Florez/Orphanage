using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public struct CameraSettings //als je standaard values wilt hebben moeten je variables in een void
    {
        public Transform myCamera;
        public LayerMask ignore;
        public Vector2 Sensitivity;

        public bool clampRot;// = true;

        [Range(0f,360f)]
        public float clampΔ;// = 180f;
        public float camSmthTime;// = 5f;
        [Space(10)]
        public Color debugCol;
    }
    [System.Serializable]
    public struct MovementSettings
    {
        public float movSpd;// = 6;
        public float runMult;// = 2.0f;
        [Space(10)]
        public float jumpApex;//= 3;
        public float secToApex;// = .5f;
        [Space(10)]
        public float minDistToGrnd;
    }
    
    public CameraSettings cameraSettings;
    [Space(10)]
    public MovementSettings movement;

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

        gravity = (2 * movement.jumpApex) / Mathf.Pow(movement.secToApex, 2); //((2*a)/t^2)
        jumpVelocity = Mathf.Abs(gravity) * movement.secToApex * rigid.mass; //(|((2*a)/t^2)| * t)

        distToGrnd = col.height - col.center.y;
    }

    private void FixedUpdate()
    {
        rigid.AddForce(-gravity * rigid.mass * transform.up);
    }

    private void LateUpdate()
    {
        LookRotation();

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrnded())
            {
                rigid.velocity += jumpVelocity * transform.up;
            }
        }
        charTrans.Translate(Input.GetAxis("Horizontal") * movement.movSpd * (Input.GetButton("Sprint") ? movement.runMult : 1f) * Time.deltaTime, 0, Input.GetAxis("Vertical") * movement.movSpd * (Input.GetButton("Sprint") ? movement.runMult : 1f) * Time.deltaTime); //move the character forth/ back with Vertical axis:
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

    /*public void LookRotation()
    {
        charTgtRot = transform.localRotation;
        camTgtRot = cameraSettings.myCamera.localRotation;

        float yRot = Input.GetAxis("Mouse X") * cameraSettings.Sensitivity.x;
        float xRot = Input.GetAxis("Mouse Y") * cameraSettings.Sensitivity.y;

        charTgtRot *= Quaternion.Euler(0f, yRot, 0f);
        camTgtRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (cameraSettings.clampRot) { camTgtRot = ClampRotationXAxis(camTgtRot); }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, charTgtRot, cameraSettings.camSmthTime * Time.deltaTime);
        cameraSettings.myCamera.localRotation = Quaternion.Slerp(cameraSettings.myCamera.localRotation, camTgtRot, cameraSettings.camSmthTime * Time.deltaTime);
    }*/

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
    public void LookRotation()
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

        if (Physics.SphereCast(transform.position - new Vector3(0, col.height/2f, 0), col.radius, Vector3.down, out hit, distToGrnd + movement.minDistToGrnd))//((col.height / 2f) - col.radius) + minDistToGrnd))
        {
            return hit.distance <= distToGrnd + movement.minDistToGrnd;
        }
        else
        {
            return false;

        }
    }
}