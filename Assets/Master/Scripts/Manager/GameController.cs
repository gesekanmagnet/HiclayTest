using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Vector2 doorLockPosition;

    [SerializeField] private Transform player;
    [SerializeField] private Boss boss;
    [SerializeField] private Transform bossRoom, bossRoomDoor;
    [SerializeField] private CinemachineCamera cc;
    [SerializeField] private CinemachinePositionComposer composer;
    [SerializeField] private AudioClip winClip, loseClip, bossSpawnClip;

    private Vector2 bossPosition, playerStartPos, doorOpenPosition;
    public static GameController Instance { get; private set; }
    public Transform Player => player;

    [SerializeField] private GameObject[] levels;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventCallback.OnBossSpawn += BossSpawn;
        EventCallback.OnGameOver += Gameover;
        EventCallback.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        EventCallback.OnBossSpawn -= BossSpawn;
        EventCallback.OnGameOver -= Gameover;
        EventCallback.OnGameStart -= GameStart;
    }

    private void Start()
    {
        LoadLevel(0, true);

        boss.enabled = false;
        doorOpenPosition = bossRoomDoor.position;
        bossPosition = boss.transform.position;
    }

    private void GameStart(Transform player)
    {
        this.player = player;
        cc.Follow = player;
        playerStartPos = player.position;
    }

    private void BossSpawn()
    {
        LoadLevel(2, true);
        bossRoomDoor.DOMove(doorLockPosition, 1f);
        cc.Follow = bossRoom;
        AudioEmitter.PlayOneShot(bossSpawnClip);
        
        var position = composer.Composition;
        position.DeadZone.Enabled = false;
        composer.Composition = position;

        boss.transform.DOMoveY(111f, 5f).OnComplete(() =>
        {
            boss.enabled = true;
        });
    }

    public void LoadLevel(int level, bool disableAllLevel = false)
    {
        if(disableAllLevel)
        {
            foreach (var item in levels)
            {
                if (item.Equals(level)) continue;
                item.SetActive(false);
            }
        }

        levels[level].SetActive(true);
    }

    private void Gameover(GameResult gameResult)
    {
        LoadLevel(0, true);
        cc.Follow = player;

        var position = composer.Composition;
        position.DeadZone.Enabled = true;
        composer.Composition = position;

        player.position = playerStartPos;
        boss.transform.DOMove(bossPosition, 5f);
        boss.DOKill();
        boss.Initialize();
        boss.enabled = false;
        bossRoomDoor.position = doorOpenPosition;
        bossRoom.gameObject.SetActive(true);

        AudioEmitter.PlayOneShot(gameResult.Equals(GameResult.Win) ? winClip : loseClip);
    }
}