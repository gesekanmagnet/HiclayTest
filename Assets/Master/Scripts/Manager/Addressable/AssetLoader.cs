using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    //[SerializeField] private AssetReferenceGameObject player, boss, bullet;
    //[SerializeField] private AssetReferenceGameObject[] level;
    [SerializeField] private AssetLabelReference mainLabel, bulletLabel, playerLabel, shootClipLabel, levelLabel, bossLabel, hatLabel, playerSpriteLabel;

    private async void Start() 
    {
        //await Addressables.InitializeAsync().Task;
        //Debug.Log("Addressables initialized");
        bool update = await AnyUpdate();
        if (update)
            await UpdateContent();

        await LoadAssets();
        await LoopUpdate();
    }

    private async System.Threading.Tasks.Task LoopUpdate()
    {
        while (Application.isPlaying)
        {
            Debug.Log("30 detik");
            bool update = await AnyUpdate();
            EventCallback.OnDemandUpdate(update);

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

        var handle = Addressables.LoadResourceLocationsAsync(levelLabel);
        await handle.Task;
        Debug.Log($"Found {handle.Result.Count} locations for label Level");
        var sorted = handle.Result.OrderBy(loc => loc.PrimaryKey).ToList();

        foreach (var item in sorted)
        {
            Debug.Log("Before load");
            var level = await Addressables.InstantiateAsync(item, GameController.Instance.EnvironmentParent).Task;
            Debug.Log("After load");
            AssetManager.AddHandle(levelLabel.labelString, level, level);
            GameController.Instance.levels.Add(level);
            //Debug.Log(item.PrimaryKey);
        }

        await Load<AudioClip>(shootClipLabel);
        await Load<Sprite>(hatLabel);
        await Load<Sprite>(playerSpriteLabel);
        await Load<GameObject>(bulletLabel);
        await LoadGameObject(bossLabel);
        //var boss = await Addressables.InstantiateAsync("Boss", instantiateInWorldSpace: true).Task;
        //AssetManager.AddHandle("Boss", boss, boss);
        //GameController.Instance.boss = boss.GetComponent<Boss>();

        await LoadGameObject(playerLabel);
        EventCallback.OnUpdate();
        //var player = await Addressables.InstantiateAsync("Player", instantiateInWorldSpace: true).Task;
        //AssetManager.AddHandle("Player", player, player);
        //EventCallback.OnGameStart(player.transform);
    }

    private async System.Threading.Tasks.Task LoadGameObject(AssetLabelReference label)
    {
        var handle = Addressables.LoadResourceLocationsAsync(label);
        await handle.Task;
        foreach (var item in handle.Result)
        {
            var asset = await Addressables.InstantiateAsync(item, instantiateInWorldSpace: true).Task;
            Debug.Log("Spawn");
            AssetManager.AddHandle(label.labelString, asset, asset);
        }
        Addressables.Release(handle);
    }

    private async System.Threading.Tasks.Task Load<T>(AssetLabelReference label) where T : Object
    {
        var obj = Addressables.LoadAssetAsync<T>(label);
        await obj.Task;
        AssetManager.AddHandle(label.labelString, obj.Result, obj);
    }

    private async System.Threading.Tasks.Task<bool> AnyUpdate()
    {
        var catalogs = await Addressables.CheckForCatalogUpdates().Task;
        if (catalogs.Count > 0)
            await Addressables.UpdateCatalogs(catalogs).Task;

        var locations = await Addressables.LoadResourceLocationsAsync(mainLabel).Task;
        var size = await Addressables.GetDownloadSizeAsync(locations).Task;
        Addressables.Release(locations);
        if (size > 0) return true;
        return false;
    }

    private async System.Threading.Tasks.Task UpdateContent()
    {
        var downloadUpdate = Addressables.DownloadDependenciesAsync(mainLabel);

        while (downloadUpdate.IsDone == false)
        {
            var status = downloadUpdate.GetDownloadStatus();
            EventCallback.OnUpdateProgress(status.DownloadedBytes, status.Percent);
            await System.Threading.Tasks.Task.Yield();
        }

        EventCallback.OnDemandUpdate(false);
    }
}