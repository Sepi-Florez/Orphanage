using UnityEngine;

public class SoundTest : MonoBehaviour
{
    //public SphereCollider soundZone;
    public AudioSource source;
    public Transform player;
    public float maxShakeDistance;

    void Start()
    {
        source = GetComponent<AudioSource>();
        //soundZone = GetComponentInChildren<SphereCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
        {
            source.Play();
            PlayerControlPhysics.Shake((0.1f * (1-(Vector3.Distance(player.position, transform.position)/ maxShakeDistance))), (0.1f *(1-(Vector3.Distance(player.position, transform.position)/ maxShakeDistance))));
            //print((0.1f * (1 - (Vector3.Distance(player.position, transform.position) / maxShakeDistance))) + " - " + (0.1f * (1 - (Vector3.Distance(player.position, transform.position) / maxShakeDistance))));
        }
    }

}