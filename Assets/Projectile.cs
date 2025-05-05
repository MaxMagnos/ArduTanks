using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float height = 5f;
    public float speed = 5f;
    public float minDistance;
    public float maxDistance;
    public AnimationCurve heightCurve;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float launchDistance;
    private float progress;
    private float scaledSpeed;

    public GameObject parentTank { get; private set; }

    public void Initialize(float distance, GameObject tank)
    {
        var normDistance = Mathf.InverseLerp(0, 1024f, distance);
        launchDistance = Mathf.Lerp(minDistance, maxDistance, normDistance);
        parentTank = tank;
        startPosition = transform.position + (transform.rotation * Vector3.up);
        targetPosition = startPosition + (transform.rotation * Vector3.up) * launchDistance;
        progress = 0f;
    }

    private void Update()
    {
        progress += (speed / launchDistance) * Time.deltaTime;
        float clampedProgress = Mathf.Clamp01(progress);

        Vector3 linearPosition = Vector3.Lerp(startPosition, targetPosition, clampedProgress);
        float arc = height * heightCurve.Evaluate(clampedProgress);
        Vector3 arcOffset = transform.rotation * new Vector3(0f, 0f, -arc);

        transform.position = linearPosition + arcOffset;

        if (clampedProgress >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * Vector3.up));
    }
}