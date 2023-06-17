using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private List<Vector3> trailPoints = new List<Vector3>();

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Add the current player position to the trail points list
        trailPoints.Add(transform.position);

        // Update the line renderer with the trail points
        lineRenderer.positionCount = trailPoints.Count;
        lineRenderer.SetPositions(trailPoints.ToArray());
    }
}
