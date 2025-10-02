using UnityEngine;

public class PoolingHandle : MonoBehaviour
{
    public static Bullet bullet { get; set; }
    [SerializeField] private int count;
    [SerializeField] private ParticleCallback particle;

    public static PoolingInstance<Bullet> bulletPooling { get; private set; }
    public static PoolingInstance<ParticleCallback> particlePool { get; private set; }

    private void Start()
    {
        EventCallback.OnGameStart += GameStart;
    }

    private void GameStart(Transform t)
    {
        bulletPooling = new(bullet, count, transform);
        particlePool = new(particle, count, transform);
    }
}