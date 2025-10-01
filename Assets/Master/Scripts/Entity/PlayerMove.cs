using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 8f, jumpForce = 15f, dashForce = 25f, dashCooldown = 2f, dashTime = .3f, rayGroundDistance = .2f;
    [SerializeField] private LayerMask jumpLayer;
    [SerializeField] private AudioClip dashClip, jumpClip, hitClip;

    private float fallGravity = 10f;
    
    private new Rigidbody2D rigidbody;
    private PlayerInput input;
    private Combat combat;
    private Health health;
    private TrailRenderer trail;
    public bool jumpable;

    private bool canDash = true, isDash = false;
    private Vector2 lastInput;

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector2.down * rayGroundDistance);
    }

    private void Awake()
    {
        combat = GetComponent<Combat>();
        health = GetComponent<Health>();
        rigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        EventCallback.OnGameOver += Initialize;
    }

    private void OnDisable()
    {
        EventCallback.OnGameOver -= Initialize;
    }

    private void Start()
    {
        health.OnZeroHealth.AddListener(() =>
        {
            EventCallback.OnGameOver(GameResult.Lose);
            Debug.Log(GameResult.Lose);
        });

        health.OnHealthReduced.AddListener((health) => 
        {
            AudioEmitter.PlayOneShot(hitClip);
            EventCallback.OnPlayerHit(health);
        });
    }

    private void Update()
    {
        lastInput = input.move == Vector2.zero ? lastInput : input.move; 

        if (isDash) return;
        
        if(input.shoot)
            combat.Shoot(lastInput);

        if (input.dash && canDash)
            StartCoroutine(Dash());
    }

    public void FixedUpdate()
    {
        if (isDash) return;

        if (input.jump)
        {
            input.jump = false;
            
            if (IsRayTouching() == false) return;
            
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioEmitter.PlayOneShot(jumpClip);
        }

        if (!Mathf.Approximately(0, input.move.x))
        {
            rigidbody.transform.rotation = -input.move .x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }

        rigidbody.linearVelocity = new(Mathf.RoundToInt(input.move.x) * speed, rigidbody.linearVelocityY);
        GravityModifier();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent<MovingPlatform>(out var platform))
        {
            if (platform.ParentPlayer == false) return;

            rigidbody.interpolation = RigidbodyInterpolation2D.None;
            transform.SetParent(platform.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<MovingPlatform>(out var platform))
        {
            transform.SetParent(null);
            rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    private bool IsRayTouching()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, rayGroundDistance, jumpLayer);
        //Debug.Log(hitInfo.collider.name);
        jumpable = hitInfo.collider != null;
        return hitInfo.collider != null;
    }

    private void Initialize(GameResult gameResult) => health.Initialize();

    private void GravityModifier()
    {
        rigidbody.gravityScale = IsRayTouching() ? 1f : rigidbody.linearVelocityY < 0 ? fallGravity : rigidbody.linearVelocityY > 0 ? 8f : 1f;
    }

    private System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDash = true;
        trail.emitting = true;

        AudioEmitter.PlayOneShot(dashClip);
        
        rigidbody.gravityScale = 0f;
        rigidbody.linearVelocity = transform.right * dashForce;

        float elapsedTime = dashTime;
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / dashTime);
            EventCallback.OnFillDash(t);
            yield return null;
        }

        trail.emitting = false;
        isDash = false;

        float elapsedCooldown = 0f;
        while (elapsedCooldown < dashCooldown)
        {
            elapsedCooldown += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedCooldown / dashCooldown);
            EventCallback.OnFillDash(t);
            yield return null;
        }

        canDash = true;
    }
}