using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private int bulletLayer = 7;
    [SerializeField] private float fireRate = 2f, bulletSpeed = 15f;
    [SerializeField] private Color bulletColor;
    [SerializeField] private AudioClip clip;

    private float timeInterval;

    public void Shoot(Vector2 direction)
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