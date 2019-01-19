using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterParticlesRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator DisableAfterParticlesRoutine()
    {
        float timeToDisable = Time.time + particles.main.duration;

        while (Time.time < timeToDisable)
        {
            yield return null;
        }

        gameObject.SetActive(false);

    }
}
