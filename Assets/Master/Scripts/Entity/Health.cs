using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent OnZeroHealth { get; set; } = new();
    public UnityEvent<int> OnHealthReduced { get; set; } = new();
    public UnityEvent OnHalfHealth { get; set; } = new();

    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private bool invicible;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        invicible = false;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (invicible) return;
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        OnHealthReduced?.Invoke(currentHealth);

        if (currentHealth == (maxHealth / 2)) OnHalfHealth?.Invoke();
        //Debug.Log(name + " current health: " + currentHealth);

        if (currentHealth <= 0) OnZeroHealth?.Invoke();
    }

    public void CantDie()
    {
        invicible = true;
    }
}