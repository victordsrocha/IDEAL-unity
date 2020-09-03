using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMultipleRays : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private float fov;
    private Vector3 origin;
    private float startingAngle;
    [SerializeField] private List<Color> visionState;

    private void Start()
    {
        fov = 90f;
        origin = Vector3.zero;
    }

    private void LateUpdate()
    {
        int rayCount = 11;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 100f;

        visionState = new List<Color>();
        for (int i = 0; i <= rayCount; i++)
        {
            visionState.Add(Color.black);
        }

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                // Hit object
                vertex = raycastHit2D.point;
                vertex = vertex - origin;

                if (raycastHit2D.collider.name == "Trees_Tilemap")
                {
                    visionState[i] = Color.green;
                }
                else if (raycastHit2D.collider.name == "Rocks_Tilemap")
                {
                    visionState[i] = Color.gray;
                }
                else if (raycastHit2D.collider.name == "House_Tilemap")
                {
                    visionState[i] = new Color(0.6f, 0.33f, 0.17f);
                }
                else if (raycastHit2D.collider.name == "Fountain_Tilemap")
                {
                    visionState[i] = Color.blue;
                }
                else if (raycastHit2D.collider.name == "Benchs_Tilemap")
                {
                    visionState[i] = new Color(0.71f, 0.27f, 0.6f);
                }
                else if (raycastHit2D.collider.name == "Block_Tilemap")
                {
                    visionState[i] = Color.black;
                }
            }

            Debug.DrawRay(origin, vertex, Color.magenta);

            angle -= angleIncrease;
        }
    }


    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        // dir -> direction
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(float aimDirection)
    {
        startingAngle = aimDirection + fov / 2f;
    }
}