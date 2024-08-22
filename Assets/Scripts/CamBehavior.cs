using UnityEngine;

public class CamBehavior : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        cam.transform.position = offset + transform.position;
        cam.transform.LookAt(target, Vector3.up);
    }
}
