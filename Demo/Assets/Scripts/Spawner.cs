using UnityEngine;

public sealed class Spawner : MonoBehaviour
{
    [SerializeField] private string prefabPath;

    void Start()
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        Object.Instantiate(prefab, transform.position, transform.rotation, transform.parent);
    }
}
