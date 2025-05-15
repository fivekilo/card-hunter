using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteRender : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void plot(List<Vector3>points)
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;  // 起点宽度
        lineRenderer.endWidth = 0.2f;    // 终点宽度
        lineRenderer.material.color = Color.black;
        List<Vector3>points = new List<Vector3> {Vector3.zero+Vector3.back,Vector3.right*5+Vector3.back,Vector3.up*5+Vector3.back };
        plot(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
