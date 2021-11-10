using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visionCone : MonoBehaviour
{
    public float viewDist;
    public float fov;
    public int rayCount;
    public Vector3 origin;
    public LayerMask walls;
    public LayerMask scan;
    public GameObject coneObj;
    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        //Quaternion targetRot = Quaternion.Euler(0, 0, fov / 2);
        //coneObj.transform.rotation = targetRot;

        coneObj.transform.parent = null;

        mesh = new Mesh();
        coneObj.GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        drawCone();
    }

    private void FixedUpdate()
    {
        
    }

    void drawCone()
    {
        origin = transform.position;

        float angle = (transform.rotation.eulerAngles.z) + (fov / 2);
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] tris = new int[rayCount * 3];

        vertices[0] = origin;

        int vertIndex = 1;
        int triIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D scanRay = Physics2D.Raycast(origin, vectorFromAngle(angle), viewDist, scan);
            if (scanRay.collider != null)
            {
                worldObject otherObject = scanRay.collider.GetComponent<worldObject>();
                if (otherObject != null)
                {
                    if (!otherObject.memorized)
                    {
                        otherObject.memorize();
                    }
                }
            }

            RaycastHit2D wallRay = Physics2D.Raycast(origin, vectorFromAngle(angle), viewDist, walls);
            //Debug.DrawRay(origin, vectorFromAngle(angle).normalized * viewDist, Color.green, 1f);
            if (wallRay.collider != null)
            {
                vertex = wallRay.point;
            }
            else
            {
                vertex = origin + vectorFromAngle(angle) * viewDist;
            }

            vertices[vertIndex] = vertex;

            if (i > 0)
            {
                tris[triIndex] = 0;
                tris[triIndex + 1] = vertIndex - 1;
                tris[triIndex + 2] = vertIndex;

                triIndex += 3;
            }

            vertIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = tris;
    }

    Vector3 vectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
