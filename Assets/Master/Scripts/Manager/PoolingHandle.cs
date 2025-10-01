using UnityEngine;

public class PoolingHandle : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private int count;
    [SerializeField] private ParticleCallback particle;

    public static PoolingInstance<Bullet> bulletPooling { get; private set; }
    public static PoolingInstance<ParticleCallback> particlePool { get; private set; }

    private void Awake()
    {
        bulletPooling = new(bullet, count, transform);
        particlePool = new(particle, count, transform);
    }
}