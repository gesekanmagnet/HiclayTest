using UnityEngine;

public class BossEvent : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMove>(out var damageable))
        {
            EventCallback.OnBossSpawn();
            gameObject.SetActive(false);
        }
    }
}