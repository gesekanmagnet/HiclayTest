using UnityEngine;

public class EnemyA : MonoBehaviour
{
    [SerializeField] private Vector2 radiusSize;
    [SerializeField] private float shootInterval = 5f;
    [SerializeField] private LayerMask target;
    [SerializeField] private AudioClip destroyClip;
    [SerializeField] private SpriteRenderer hitRenderer;

    private Combat combat;
    private Health health;

    private float shootTimer, stopTimer;
    private bool counting;

    private void Awake()
    {
        combat = GetComponent<Combat>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.OnZeroHealth.AddListener(() =>
        {
            AudioEmitter.PlayOneShot(destroyClip);
            gameObject.SetActive(false);
        });

        health.OnHealthReduced.AddListener((health) =>
        {
            hitRenderer.DOAlphaFlash(0, 1f, .3f);
        });
    }

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, radiusSize, 0, target);
        if (collider)
        {
            Vector2 direction = collider.transform.position - transform.position;
            if (shootTimer < shootInterval)
            {
                counting = true;
                combat.Shoot(direction.normalized);
            }
        }
        else counting = false;

        if(shootTimer < shootInterval && counting)
            shootTimer += Time.deltaTime;
        else
        {
            stopTimer += Time.deltaTime;

            if(stopTimer >= shootInterval)
            {
                stopTimer = 0;
                shootTimer = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, radiusSize);
    }
}