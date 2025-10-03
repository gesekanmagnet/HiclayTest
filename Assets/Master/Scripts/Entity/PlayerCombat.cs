using UnityEngine;

public class PlayerCombat : Combat
{
    public override void Shoot(Vector2 direction)
    {
        if (Time.time >= timeInterval)
        {
            Bullet bullet = PoolingHandle.bulletPooling.GetItem();
            bullet.Initialize(bulletColor, transform.position, direction == Vector2.zero ? Vector2.right : direction, bulletLayer, damage: 1, speed: bulletSpeed);
            AudioEmitter.PlayOneShot(AssetLoader.Instance.CombatClip.Value);

            timeInterval = Time.time + 1f / fireRate;
        }
    }
}