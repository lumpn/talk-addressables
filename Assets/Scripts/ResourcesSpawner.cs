using System.Collections;
using UnityEngine;

public sealed class ResourcesSpawner : MonoBehaviour
{
    [SerializeField] private string prefabName;

    protected IEnumerator Start()
    {
        var request = Resources.LoadAsync<GameObject>(prefabName);
        yield return request;

        var prefab = request.asset;
        Object.Instantiate(prefab, transform.position, transform.rotation, transform.parent);
    }
}
