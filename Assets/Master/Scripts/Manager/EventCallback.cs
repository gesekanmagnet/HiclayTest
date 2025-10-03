using System;

public static class EventCallback
{
    public static Action<UnityEngine.Transform> OnGameStart { get; set; } = delegate { };
    public static Action<GameResult> OnGameOver { get; set; } = delegate { };

    public static Action<int> OnPlayerHit { get; set; } = delegate { };
    public static Action<float> OnFillDash { get; set; } = delegate { };
    public static Action<int> OnBossHealth { get; set; } = delegate { };
    public static Action OnBossSpawn { get; set; } = delegate { };

    public static Action<bool> OnDemandUpdate { get; set; } = delegate { };
    public static Action<long, float> OnUpdateProgress { get; set; } = delegate { };
}

public enum GameResult { Lose, Win }