using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MagicalLight : MonoBehaviour
{
    [SerializeField] private LayerMask lightCheck;
    [SerializeField] private LayerMask contactCheck;
    private Mesh mesh;
    private Vector3 origin;

   //Light Ray Variables
    private float fov;
    private float startingAngle;
    float viewDistance = 2f;

    public bool isToggled = false;
    private bool hitFlag = false; //Contact Flag
    private GameObject hitObject; //Object being contacted by Light

    private void Start()
    {
        //Create Mesh and Locate MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 50f;
    }

    //Update Light Distance Based on Level
    private void Update()
    {
        if(GameController.instance.GetPlayerLight() == 1)
        {
            viewDistance = 2.5f;
        }

        else
        {
            viewDistance = 3.5f;
        }
    }

    private void LateUpdate()
    {
        if (isToggled)
        {
            GenerateLight(); //Activate Light when toggled
        }

        else
        {
            if (hitObject != null)
            {
                hitObject.SendMessage("OnHitExit"); //Notify Special Object when Light is no longer contacting 
                hitFlag = false;
                hitObject = null;
            }

            mesh.Clear(); //Turn off Light
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
            RaycastHit2D RaycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, lightCheck);
            
            //Midpoint Collision Check
            if(i == rayCount / 2)
            {
                RaycastHit2D raycheck = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, contactCheck);
                if (raycheck.collider != null && raycheck.transform.CompareTag("SpecialObject")) //If Check hits a special Object
                {
                    GameObject LO = raycheck.transform.gameObject;

                    // If no registered hitObject => Notify Entering Object
                    if (hitObject == null)
                    {
                        LO.SendMessage("OnHitEnter");
                    }

                    //If hitObject is the same as the registered one => Notify OnStay                    
                    else if (hitObject.GetInstanceID() == LO.GetInstanceID())
                    {
                        hitObject.SendMessage("OnHitStay");
                    }

                    // If new hitObject hit => Notify OnExit Previous Object + OnEnter New Object
                    else
                    {
                        hitObject.SendMessage("OnHitExit");
                        LO.SendMessage("OnHitEnter");
                    }

                    hitFlag = true;
                    hitObject = LO;
                }

                // No new Object hit => Notify OnExit & Clear Check Variables
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
