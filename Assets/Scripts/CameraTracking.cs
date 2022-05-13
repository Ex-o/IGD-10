using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform stackerTransform;
    [SerializeField] private float smooth;
    private float offsetY, offsetZ;

    void Start()
    {
        offsetY = transform.position.y - stackerTransform.position.y;
        offsetZ = transform.position.z - stackerTransform.position.z;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, stackerTransform.position.y + offsetY, stackerTransform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
    }
}
