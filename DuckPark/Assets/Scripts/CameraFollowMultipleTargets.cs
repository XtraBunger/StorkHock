using UnityEngine;
using System.Collections.Generic;

public class CameraFollowMultipleTargets : MonoBehaviour
{
    public List<Transform> targets; // Add all player transforms here

    public Vector3 offset;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Camera cam;

    public float killZoneY = -20f; // Set this in Inspector
    public Transform teleportTarget; // Assign in Inspector

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();

        // Teleport players who fall below killZoneY
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] != null && targets[i].position.y < killZoneY)
            {
                if (teleportTarget != null)
                {
                    targets[i].position = teleportTarget.position;
                    // Optionally reset velocity if using Rigidbody2D
                    Rigidbody2D rb = targets[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                        rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float greatestDistance = GetGreatestDistance();
        float newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.size.x; // You can use size.y if vertical spread is more important
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }
}
