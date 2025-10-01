using DG.Tweening;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Combat combat;
    private Health health;

    [SerializeField] private float shootInterval = 5f, duration = 3f;
    [SerializeField] private LayerMask shootTarget;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer hitRenderer;

    private float shootTimer, stopTimer;

    private void Awake()
    {
        combat = GetComponent<Combat>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.OnHealthReduced.AddListener((health) =>
        {
            EventCallback.OnBossHealth(health);
            hitRenderer.DOAlphaFlash(0, 1, .3f);
        });

        health.OnHalfHealth.AddListener(() =>
        {
            HalfHealth();
        });

        health.OnZeroHealth.AddListener(() =>
        {
            EventCallback.OnGameOver(GameResult.Win);
        });
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;
        if (shootTimer < shootInterval)
            combat.Shoot(direction.normalized);

        if (shootTimer < shootInterval)
            shootTimer += Time.deltaTime;
        else
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= shootInterval)
            {
                stopTimer = 0;
                shootTimer = 0;
            }
        }
    }

    public void Initialize() => health.Initialize();

    private void HalfHealth()
    {
        float startX = transform.position.x;

        transform.DOMoveX(startX + 10f, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMoveX(startX - 10f, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
    }
}