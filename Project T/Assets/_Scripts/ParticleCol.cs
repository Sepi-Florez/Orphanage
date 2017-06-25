using System.Collections;
using UnityEngine;

public class ParticleCol : MonoBehaviour
{
    public ParticleSystem emitter;
    ParticleSystem.EmissionModule emission;

    public void Start()
    {
        emission = emitter.emission;
        emission.enabled = false;
    }

    public void Particle(float dur)
    {
        StartCoroutine(ParticleTimer(dur));
    }

    public IEnumerator ParticleTimer(float duration)
    {
        emission.enabled = true;
        PlayerControlPhysics.Shake(0.5f, 0.2f);
        yield return new WaitForSeconds(duration);
        emission.enabled = false;
    }
}
