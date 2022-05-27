using System.Collections;
using System.IO;
using UnityEngine;

public sealed class AssetBundleSpawner : MonoBehaviour
{
    [SerializeField] private string bundleName;
    [SerializeField] private string prefabName;

    protected IEnumerator Start()
    {
        var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleRequest;

        var bundle = bundleRequest.assetBundle;
        var prefabRequest = bundle.LoadAssetAsync<GameObject>(prefabName);
        yield return prefabRequest;

        var prefab = prefabRequest.asset;
        Object.Instantiate(prefab, transform.position, transform.rotation, transform.parent);
    }
}
