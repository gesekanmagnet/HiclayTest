using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject player, boss, bullet;
    [SerializeField] private AssetReferenceGameObject[] level;
    [SerializeField] private AssetRef<AudioClip> combatClip;

    System.Collections.Generic.List<AssetReference> allRefs = new();
    public static AssetLoader Instance { get; private set; }

    public AssetRef<AudioClip> CombatClip => combatClip;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start() 
    {
        //bool update = await AnyUpdate();
        //if(update)
        //    await UpdateContent();
        
        await LoadAssets();
        await LoopUpdate();
    }

    private async System.Threading.Tasks.Task LoopUpdate()
    {
        while (Application.isPlaying)
        {
            Debug.Log("30 detik");
            bool update = await AnyUpdate();
            if (update)
            {
                Debug.Log("update");
                EventCallback.OnDemandUpdate(true);
            }
            else
                EventCallback.OnDemandUpdate(false);

            await System.Threading.Tasks.Task.Delay(30000);
        }
    }

    public async void DownloadUpdate()
    {
        AssetManager.ReleaseAll();
        await UpdateContent();
        await LoadAssets();
    }

    private async System.Threading.Tasks.Task LoadAssets()
    {
        GameController.Instance.levels.Clear();

        foreach (var item in level)
        {
            var result = await item.InstantiateAsync(GameController.Instance.EnvironmentParent).Task;
            AssetManager.AddHandle(result);
            GameController.Instance.levels.Add(result);
        }

        //var combatClip = this.combatClip.assetReference.LoadAssetAsync<AudioClip>();
        //AssetManager.AddHandle(combatClip);
        //this.combatClip.Value = await combatClip.Task;

        var bullet = await this.bullet.InstantiateAsync(instantiateInWorldSpace: true).Task;
        AssetManager.AddHandle(bullet);
        PoolingHandle.bullet = bullet.GetComponent<Bullet>();

        var boss = await this.boss.InstantiateAsync(instantiateInWorldSpace: true).Task;
        AssetManager.AddHandle(boss);
        GameController.Instance.boss = boss.GetComponent<Boss>();

        var player = await this.player.InstantiateAsync(instantiateInWorldSpace: true).Task;
        AssetManager.AddHandle(player);
        EventCallback.OnGameStart(player.transform);
    }

    private async System.Threading.Tasks.Task<bool> AnyUpdate()
    {
        var catalogs = await Addressables.CheckForCatalogUpdates().Task;
        if (catalogs.Count > 0)
            await Addressables.UpdateCatalogs(catalogs).Task;

        allRefs.Clear();
        allRefs.Add(player);
        allRefs.Add(boss);
        allRefs.Add(bullet);
        allRefs.AddRange(level);
        //allRefs.Add(combatClip.assetReference);
        //Debug.Log(allRefs.Count);

        var size = await Addressables.GetDownloadSizeAsync(allRefs).Task;
        if (size > 0) return true;
        return false;
    }

    private async System.Threading.Tasks.Task UpdateContent()
    {
        var downloadUpdate = Addressables.DownloadDependenciesAsync(allRefs, Addressables.MergeMode.Union, true);

        while (downloadUpdate.IsDone == false)
        {
            var status = downloadUpdate.GetDownloadStatus();
            EventCallback.OnUpdateProgress(status.DownloadedBytes, status.Percent);
            await System.Threading.Tasks.Task.Yield();
        }

        EventCallback.OnDemandUpdate(false);
    }
}

[System.Serializable]
public class AssetRef<T> where T : Object
{
    public AssetReferenceT<T> assetReference;
    public T Value { get; set; }
}