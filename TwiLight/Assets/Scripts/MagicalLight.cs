using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MagicalLight : MonoBehaviour
{
    [SerializeField] private LayerMask lightIgnore;
    [SerializeField] private LayerMask checkMask;
    private Mesh mesh;
    private Vector3 origin;

    private float fov;
    private float startingAngle;
    float viewDistance = 5f;

    public bool isToggled = false;
    private bool hitFlag = false;
    private GameObject hitObject;

    private void Start()
    {
        //Create Mesh and Locate MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 60f;
    }

    private void LateUpdate()
    {
        if (isToggled)
        {
            GenerateLight();
        }

        else
        {
            if (hitObject != null)
            {
                hitObject.SendMessage("OnHitExit");
                hitFlag = false;
                hitObject = null;
            }

            mesh.Clear();
        }
    }
    
    #region Light Emission
    public void GenerateLight()
    {
        //Mesh Fields & Variables
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        //Vector2 offset = new Vector2(0.5f, 0.5f); //offset light projection into walls (add to raycast hit if needed)

        //Vertices, UV, Triangle Storage
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];


        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        //Position & Generate Triangles based on rayCount
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D RaycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, lightIgnore);
            
            //Midpoint Collision Check
            if(i == rayCount / 2)
            {
                RaycastHit2D raycheck = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, checkMask);
                if (raycheck.collider != null && raycheck.transform.CompareTag("SpecialObject"))
                {
                    GameObject LO = raycheck.transform.gameObject;

                    // If no registred hitobject => Entering
                    if (hitObject == null)
                    {
                        LO.SendMessage("OnHitEnter");
                    }

                    // If hit object is the same as the registered one => Stay
                    else if (hitObject.GetInstanceID() == LO.GetInstanceID())
                    {
                        //hitObject.SendMessage("OnHitStay");
                    }

                    // If new object hit => Exit last + Enter new
                    else
                    {
                        hitObject.SendMessage("OnHitExit");
                        LO.SendMessage("OnHitEnter");
                    }

                    hitFlag = true;
                    hitObject = LO;
                }

                // No object hit => Exit last one
                else if (hitFlag)
                {
                    hitObject.SendMessage("OnHitExit");
                    hitFlag = false;
                    hitObject = null;
                }
            }

            //Generate Mesh Vertices
            if (RaycastHit2D.collider == null)
            {
                //No Hit (generate vertex at max range)
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            } 

            else
            {
                //Hit (generate vertex at point of collision)
                vertex = RaycastHit2D.point;
            }

            vertices[vertexIndex] = vertex;

            //Generate Mesh Triangles
            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex++] = vertexIndex - 1;
                triangles[triangleIndex++] = vertexIndex;

                triangleIndex++;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        //Update Mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 500f);
    }

    //Set Light Emission Point
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //Set Light Emission Direction
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    #endregion

    #region Calculations
    //Translate Angle into Vector3 value
    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    //Translate Vector3 into Angle
    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    #endregion
}
