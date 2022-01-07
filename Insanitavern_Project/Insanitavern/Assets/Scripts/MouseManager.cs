using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 groundPosition;

    private void Update()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            groundPosition = ray.GetPoint(distance);
        }
    }
}
