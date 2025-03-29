using UnityEngine;

public class AlwaysOnTop : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offset;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.SetPositionAndRotation(target.position + new Vector3(0, offset, 0), Quaternion.identity);
        }
    }
}
