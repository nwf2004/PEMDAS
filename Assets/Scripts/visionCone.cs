using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visionCone : MonoBehaviour
{
    [Header("Cone Properties")]
    public float viewDist;
    public float fov;
    public float targetFov;
    public int rayCount;
    public float padding;
    public float scaleSpeed;

    [Header("Aiming Properties")]
    public float aimFov;
    public float targetAimFov;
    public float aimScaleSpeed;
    public float aimDist;
    public float aimEffectStartDist;

    [Header("Designations")]
    //Vision Cone
    public GameObject coneObj;
    public LayerMask walls;
    public LayerMask scan;
    //Aiming
    public LineRenderer line1;
    public LineRenderer line2;

    #region Privates
    private Mesh mesh;
    private List<worldObject> viewedObjects = new List<worldObject>();
    private Vector3 origin;
    #endregion
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
        
    }

    private void FixedUpdate()
    {
        drawCone();
        drawAim();
    }

    void drawCone()
    {
        fov = Mathf.MoveTowards(fov, targetFov, scaleSpeed * Time.deltaTime);

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
            Vector3 rayAngle = vectorFromAngle(angle);

            RaycastHit2D scanRay = Physics2D.Raycast(origin, rayAngle, viewDist, scan);
            if (scanRay.collider != null)
            {
                worldObject otherObject = scanRay.collider.GetComponent<worldObject>();
                if (otherObject != null)
                {
                    if (!otherObject.memorized)
                    {
                        otherObject.viewCover++;
                        viewedObjects.Add(otherObject);
                    }
                }
            }

            RaycastHit2D wallRay = Physics2D.Raycast(origin, rayAngle, viewDist, walls);
            //Debug.DrawRay(origin, vectorFromAngle(angle).normalized * viewDist, Color.green, 1f);
            if (wallRay.collider != null)
            {
                vertex = wallRay.point + new Vector2(rayAngle.normalized.x * padding, rayAngle.normalized.y * padding);
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

        foreach(worldObject currentObject in viewedObjects)
        {
            if(currentObject.viewCover >= currentObject.viewMemReq)
            {
                currentObject.memorize();
            }
            else
            {
                currentObject.viewCover = 0;
            }
        }
        viewedObjects.Clear();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = tris;
    }

    void drawAim()
    {
        aimFov = Mathf.MoveTowards(aimFov, targetAimFov, aimScaleSpeed * Time.deltaTime);
        float aimAngle = (transform.rotation.eulerAngles.z) + (aimFov / 2);
        float aimAngleIncrease = aimFov / 2;

        for (int i = 0; i <= 2; i++)
        {
            Vector3 vertex;
            Vector3 rayAngle = vectorFromAngle(aimAngle);
            RaycastHit2D aimRay = Physics2D.Raycast(origin, rayAngle, viewDist, walls);
            if (aimRay.collider != null)
            {
                vertex = aimRay.point;
            }
            else
            {
                vertex = origin + vectorFromAngle(aimAngle) * aimDist;
            }


            Vector2 startPoint = new Vector2(origin.x, origin.y) + new Vector2(rayAngle.normalized.x * aimEffectStartDist, rayAngle.normalized.y * aimEffectStartDist);

            if (i == 0)
            {
                line1.SetPosition(0, startPoint);
                line1.SetPosition(1, vertex);
            }
            else if (i == 2)
            {
                line2.SetPosition(0, startPoint);
                line2.SetPosition(1, vertex);
            }
            aimAngle -= aimAngleIncrease;
        }
    }

    Vector3 vectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
