using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{ 
    [SerializeField] private AssetReferenceGameObject player; 
    
    private void Start() 
    { 
        player.InstantiateAsync(instantiateInWorldSpace: true).Completed += (instantiate) => 
        { 
            Debug.LogError(instantiate.Status); 
            EventCallback.OnGameStart(instantiate.Result.transform); 
        }; 
    } 
}