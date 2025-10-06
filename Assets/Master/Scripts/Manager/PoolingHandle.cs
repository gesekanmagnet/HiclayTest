using UnityEngine;

public class PoolingHandle : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private ParticleCallback particle;

    public static PoolingInstance<Bullet> bulletPooling { get; private set; }
    public static PoolingInstance<ParticleCallback> particlePool { get; private set; }

    private void Awake()
    {
        particlePool = new(particle, count, transform);
    }

    private void OnEnable()
    {
        EventCallback.OnUpdate += Load;
    }

    private void Load()
    {
        bulletPooling = new(AssetManager.Get<GameObject>("Bullet").GetComponent<Bullet>(), count, transform);
    }
}