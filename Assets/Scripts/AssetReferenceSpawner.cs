using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class AssetReferenceSpawner : MonoBehaviour
{
    [SerializeField] private AssetReferenceT<GameObject> prefabRef;

    protected IEnumerator Start()
    {
        var request = prefabRef.InstantiateAsync(transform.position, transform.rotation, transform.parent);
        yield return request;
    }
}
