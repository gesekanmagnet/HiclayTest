using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
    private ParticleSystem system;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
        var module = system.main;
        module.useUnscaledTime = true;
    }

    private void OnParticleSystemStopped()
    {
        PoolingHandle.particlePool.ReturnItem(this);
    }
}