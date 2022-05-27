using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class AddressablesSpawner : MonoBehaviour
{
    [SerializeField] private string prefabAddress;

    protected IEnumerator Start()
    {
        var request = Addressables.InstantiateAsync(prefabAddress, transform.position, transform.rotation, transform.parent);
        yield return request;
    }
}
