using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject player, boss, bullet;
    [SerializeField] private AssetReferenceGameObject[] level;
    
    private async void Start() 
    {
        await UpdateContent();
        await LoadAssets();
        await LoopUpdate();
    }

    private async System.Threading.Tasks.Task LoopUpdate()
    {
        while (Application.isPlaying)
        {
            Debug.Log("30 detik");
            bool update = await UpdateContent();
            if (update)
            {
                Debug.Log("update");
                AssetManager.ReleaseAll();
                await LoadAssets();
            }

            await System.Threading.Tasks.Task.Delay(30000);
        }
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

    private async System.Threading.Tasks.Task<bool> UpdateContent()
    {
        var catalogs = await Addressables.CheckForCatalogUpdates().Task;
        if (catalogs.Count > 0)
            await Addressables.UpdateCatalogs(catalogs).Task;

        System.Collections.Generic.List<AssetReference> allRefs = new();
        allRefs.Add(player);
        allRefs.Add(boss);
        allRefs.Add(bullet);
        allRefs.AddRange(level);
        //Debug.Log(allRefs.Count);

        var size = await Addressables.GetDownloadSizeAsync(allRefs).Task;
        if (size > 0)
        {
            EventCallback.OnUpdate(true);
            Debug.LogError($"Update size: {size / (1024f * 1024f):F2} MB");
            var downloadUpdate = Addressables.DownloadDependenciesAsync(allRefs, Addressables.MergeMode.Union, true);

            while (downloadUpdate.IsDone == false)
            {
                var status = downloadUpdate.GetDownloadStatus();
                EventCallback.OnUpdateProgress(status.DownloadedBytes, status.Percent);
                await System.Threading.Tasks.Task.Yield();
            }

            EventCallback.OnUpdate(false);
            return true;
        }

        EventCallback.OnUpdate(false);
        return false;
    }
}