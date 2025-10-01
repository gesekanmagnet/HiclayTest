using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float enableDuration = 10f;
    [SerializeField] private SpriteRenderer bulletRenderer;

    private new Rigidbody2D rigidbody;

    private float speed = 5f;
    private int damage = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * transform.up, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Health>(out var damageable))
            damageable.TakeDamage(damage);

        PoolingHandle.particlePool.GetItem().transform.position = transform.position;
        PoolingHandle.bulletPooling.ReturnItem(this);
    }

    public void Initialize(Color color, Vector2 position, Vector2 direction, int layer, int damage = 1, float speed = 5f)
    {
        StartCoroutine(Disable());

        bulletRenderer.color = color;
        gameObject.layer = layer;
        this.damage = damage;
        this.speed = speed;
        transform.position = position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private System.Collections.IEnumerator Disable()
    {
        yield return new WaitForSeconds(enableDuration);
        PoolingHandle.bulletPooling.ReturnItem(this);
        
    }
}