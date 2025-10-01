using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayInterface : MonoBehaviour
{
    [SerializeField] private RectTransform bossFillBar, bossHealthPanel;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private CanvasGroup completePanel;
    [SerializeField] private Image dashFill;

    private Vector2 bossPanelStartPosition;

    private void OnEnable()
    {
        EventCallback.OnBossHealth += UpdateBossFill;
        EventCallback.OnBossSpawn += BossSpawn;
        EventCallback.OnGameOver += Gameover;
        EventCallback.OnPlayerHit += UpdatePlayerHealth;
        EventCallback.OnFillDash += UpdateDashFill;
    }

    private void OnDisable()
    {
        EventCallback.OnBossHealth -= UpdateBossFill;
        EventCallback.OnBossSpawn -= BossSpawn;
        EventCallback.OnGameOver -= Gameover;
        EventCallback.OnPlayerHit -= UpdatePlayerHealth;
        EventCallback.OnFillDash -= UpdateDashFill;
    }

    private void Start()
    {
        bossPanelStartPosition = bossHealthPanel.anchoredPosition;
    }

    private void BossSpawn()
    {
        bossHealthPanel.DOAnchorPos(Vector2.zero, 2f);
    }

    private void Gameover(GameResult gameResult)
    {
        if(gameResult == GameResult.Win)
        {
            completePanel.DOFade(1, .5f).OnComplete(() =>
            {
                completePanel.DOFade(0, 3f).SetDelay(3f);
            });
        }

        bossHealthPanel.DOAnchorPos(bossPanelStartPosition, 2f);
        bossFillBar.localScale = new(1, 1);

        UpdatePlayerHealth(5);
    }

    private void UpdatePlayerHealth(int health) => playerHealthText.SetText("HP: " + health.ToString());
    private void UpdateDashFill(float value) => dashFill.fillAmount = value;

    private void UpdateBossFill(int health)
    {
        Vector3 scale = new(health / 100f, 1f);
        bossFillBar.localScale = scale;
    }
}