using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] protected int bulletLayer = 7;
    [SerializeField] protected float fireRate = 2f, bulletSpeed = 15f;
    [SerializeField] protected Color bulletColor;
    [SerializeField] protected AudioClip clip;

    protected float timeInterval;

    public virtual void Shoot(Vector2 direction)
    {
        if(Time.time >= timeInterval)
        {
            Bullet bullet = PoolingHandle.bulletPooling.GetItem();
            bullet.Initialize(bulletColor, transform.position, direction == Vector2.zero ? Vector2.right : direction, bulletLayer, damage: 1, speed: bulletSpeed);
            AudioEmitter.PlayOneShot(clip);

            timeInterval = Time.time + 1f / fireRate;
        }
    }
}