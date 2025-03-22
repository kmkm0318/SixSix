using UnityEngine;

public class AlwaysOnTop : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offset;

    void LateUpdate()
    {
        transform.position = target.position + new Vector3(0, offset, 0);
        transform.rotation = Quaternion.identity;
    }
}
