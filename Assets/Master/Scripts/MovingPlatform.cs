using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float duration = 5f;
    [SerializeField] private SpriteRenderer hitRenderer;
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool parentPlayer = true;

    private Health health;
    public bool ParentPlayer => parentPlayer;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        transform.DOMove(direction, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        if (health == null) return;
        health.OnHealthReduced.AddListener((health) =>
        {
            hitRenderer.DOAlphaFlash(0, 1f, .3f);
        });
        health.OnZeroHealth.AddListener(() =>
        {
            AudioEmitter.PlayOneShot(clip);
            gameObject.SetActive(false);
        });
    }
}