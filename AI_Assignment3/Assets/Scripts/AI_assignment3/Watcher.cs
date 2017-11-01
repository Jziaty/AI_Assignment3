using UnityEngine;
using System.Collections;

public class Watcher : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private float currentZoom = 10f;

    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
    }


    void LateUpdate()
    {
        if (target)
        {
            transform.position = transform.parent.position - offset * currentZoom;

            Vector3 watchPoint = target.GetComponent<FlockController>().flockCenter;
            transform.LookAt(watchPoint + target.transform.position);
        }
    }
}
