using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject player, boss, bullet;
    [SerializeField] private AssetReferenceGameObject[] level;
    
    private async void Start() 
    {
        await UpdateContent();

        foreach (var item in level)
        {
            var result = await item.InstantiateAsync(GameController.Instance.EnvironmentParent).Task;
            GameController.Instance.levels.Add(result);
        }

        var bullet = await this.bullet.InstantiateAsync(instantiateInWorldSpace: true).Task;
        Debug.Log($"Bullet done");
        PoolingHandle.bullet = bullet.GetComponent<Bullet>();

        var boss = await this.boss.InstantiateAsync(instantiateInWorldSpace: true).Task;
        Debug.Log($"Boss done");
        GameController.Instance.boss = boss.GetComponent<Boss>();

        var player = await this.player.InstantiateAsync(instantiateInWorldSpace: true).Task;
        Debug.Log($"Player done");
        EventCallback.OnGameStart(player.transform);
    }

    private async System.Threading.Tasks.Task UpdateContent()
    {
        var catalogs = await Addressables.CheckForCatalogUpdates().Task;
        if (catalogs.Count > 0)
            await Addressables.UpdateCatalogs(catalogs).Task;

        System.Collections.Generic.List<AssetReference> allRefs = new();
        allRefs.Add(player);
        allRefs.Add(boss);
        allRefs.Add(bullet);
        allRefs.AddRange(level);
        Debug.Log(allRefs.Count);

        var size = await Addressables.GetDownloadSizeAsync(allRefs).Task;
        if (size > 0)
        {
            Debug.LogError($"Update size: {size / (1024f * 1024f):F2} MB");
            foreach (var asset in allRefs)
                await Addressables.DownloadDependenciesAsync(asset, true).Task;
        }
    }
}