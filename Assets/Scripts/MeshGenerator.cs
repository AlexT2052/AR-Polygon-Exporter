using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class SerializableList<Vector2>
{
    public List<Vector2> list;
}

[RequireComponent(typeof(MeshFilter))] // Requires there to be a mesh filter component
public class MeshGenerator : MonoBehaviour
{

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    [SerializeField]
    public SerializableList<Vector2> serializableList;
    List<Vector3> vectorList;
    List<GameObject> placementIndicators;
    LineRenderer lr;
    GameObject line;
    Mesh mesh;

    int hitIndicatorIndex = 0;
    int[] meshTriangles;    // List of the triangles that will form our mesh
    float lineWidth = 0.01f;
    bool polygonFinished = false;
    bool truePolygon = false;
    bool repositioning = false;
    bool isOnPc = false;
    public bool disableRaycasts
    { get; set; }
    public bool drawMeasurements
    { get; set; }

    [SerializeField]
    DrawMeasurements drawMeasurementsClass;

    [SerializeField]
    GameObject placedPrefab;

    [SerializeField]
    GameObject groundPlane;

    [SerializeField]
    Text squareFootageText;

    //[SerializeField]
    private Camera m_Camera;

    [SerializeField]
    Material collisionDetectedMaterial;

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    // Start is called before the first frame update
    void Awake()
    {
        isOnPc = SystemInfo.deviceType == DeviceType.Desktop;
        //groundPlane = TrackableType.PlaneWithinPolygon;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        m_Camera = Camera.main;
        vectorList = new List<Vector3>();
        placementIndicators = new List<GameObject>();
        disableRaycasts = false;
        drawMeasurements = false;


        line = new GameObject();
        lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 0;
        lr.loop = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!disableRaycasts)
        {
            bool firstClick = false;
            if (Input.GetMouseButtonDown(0))
            {
                firstClick = true;
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 20);
                Vector3 groundHit = default;

                if (raycastHits.Length > 0)
                {
                    bool hitGround = false;
                    bool hitIndicator = false;
                    bool isOnPc = SystemInfo.deviceType == DeviceType.Desktop;

                    if (!isOnPc)
                    {
                        if (m_RaycastManager.Raycast(Input.mousePosition, s_Hits, TrackableType.PlaneWithinPolygon))
                        {
                            hitGround = true;
                            groundHit = s_Hits[0].pose.position;
                        }
                    }

                    foreach (RaycastHit rh in raycastHits)
                    {
                        if (isOnPc)
                        {
                            if (rh.transform.gameObject == groundPlane)
                            {
                                groundHit = rh.point;
                                hitGround = true;
                            }
                        }

                        for (int i = 0; i < placementIndicators.Count; i++)
                        {
                            if (!repositioning)
                            {
                                if (rh.transform.gameObject == placementIndicators[i])
                                {
                                    placementIndicators[i].GetComponent<Outline>().OutlineColor = Color.yellow;
                                    hitIndicator = true;
                                    hitIndicatorIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                    if (hitGround)
                    {
                        groundHit.y = 0.01f; // To make line renderer not clip into ground and look weird

                        if (firstClick && hitIndicator) // Set to the repositioning mode to essentially disable everything else while still holding down.
                        {
                            repositioning = true;
                        }

                        if (repositioning) // reposition if in repositioning mode
                        {
                            placementIndicators[hitIndicatorIndex].transform.position = groundHit;
                            vectorList[hitIndicatorIndex] = groundHit;

                            isFullPolygonTrue();
                            if (polygonFinished && truePolygon)
                            {
                                DrawMesh();
                                if (!isOnPc)
                                {
                                    squareFootageText.text = Util.superficieOfIrregularPolygon(vectorList) + " m�";
                                }
                            }
                        }
                        else if (!polygonFinished) 
                        {
                            isNewSegmentTrue();

                            if (firstClick)
                            {
                                vectorList.Add(groundHit);
                                placementIndicators.Add(Instantiate(placedPrefab, groundHit, Quaternion.identity));
                                lr.positionCount = vectorList.Count;
                            }
                            else
                            {
                                vectorList[vectorList.Count - 1] = groundHit;
                                placementIndicators[placementIndicators.Count - 1].transform.position = groundHit;
                            }

                            if (vectorList.Count >= 4 && Vector3.Distance(groundHit, vectorList[0]) < 0.1f)
                            {
                                if (truePolygon)
                                {
                                    vectorList.RemoveAt(vectorList.Count - 1);
                                    Destroy(placementIndicators[placementIndicators.Count - 1]);
                                    placementIndicators.RemoveAt(placementIndicators.Count - 1);
                                    lr.positionCount = vectorList.Count;
                                    polygonFinished = true;
                                    lr.loop = true;
                                    DrawMesh();
                                    squareFootageText.enabled = true;
                                    if (!isOnPc)
                                    {
                                        squareFootageText.text = Util.superficieOfIrregularPolygon(vectorList) + " m�";
                                    }
                                    Debug.Log(Util.superficieOfIrregularPolygon(vectorList));
                                }
                            }
                        }
                    }

                    DrawLines();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                saveVector2s();
                if (repositioning)
                {
                    repositioning = false;
                }

                for (int i = 0; i < placementIndicators.Count; i++)
                {
                    placementIndicators[i].GetComponent<Outline>().OutlineColor = i == 0 ? Color.red : Color.cyan;
                }

                //placementIndicators[0].GetComponent<Renderer>().material.color = Color.red;
                //Debug.Log(superficieOfIrregularPolygon())
            }
        }

        if (drawMeasurements)
        {
            drawMeasurementsClass.Draw(vectorList, polygonFinished);
        } else if (!isOnPc)
        {
            drawMeasurementsClass.remove();
        }
    }

    public void UpdateLoop()
    {
        DrawLines();
        isNewSegmentTrue();
    }

    public bool getVectorList(out List<Vector3> vectorList)
    {
        vectorList = this.vectorList;
        if (this.vectorList.Count > 2)
        {
            return true;
        }
        return false;   
    }

    public void DrawMesh()
    {
        mesh.Clear();
        mesh.vertices = vectorList.ToArray();

        // The triangle vertices must be done in clockwise order.
        /*meshTriangles = new int[3 * (vectorList.Count - 2)];
        for (int i = 0; i < vectorList.Count - 2; i++)
        {
            meshTriangles[3 * i] = 0;
            meshTriangles[(3 * i) + 1] = i + 1;
            meshTriangles[(3 * i) + 2] = i + 2;
        }*/

        PolygonHelper.Triangulate(Util.ConvertToCorrectVector2s(vectorList).ToArray(), out int[] triangles, out string errorMessage);

        mesh.triangles = triangles;
    }

    public void DrawLines()
    {
        int count = 0;
        foreach (Vector3 vec3 in vectorList)
        {
            lr.SetPosition(count, vec3);
            count++;
        }
    }


    // Detect if polygon is self intersecting
    private bool isNewSegmentTrue()
    {
        int listLength = vectorList.Count;
        if (vectorList.Count >= 4)
        {
            for (int i = 0; i < vectorList.Count - 3; i++)
            {
                if (Util.detectIntersection(vectorList[listLength - 1], vectorList[listLength - 2], vectorList[i], vectorList[i + 1]))
                {
                    lr.material = collisionDetectedMaterial;
                    truePolygon = false;
                    return false;
                }
            }
            lr.material = new Material(Shader.Find("Sprites/Default"));
            truePolygon = true;
            return true;
        } else
        {
            return false;
        }
    }

    private bool isFullPolygonTrue()
    {
        int listLength = vectorList.Count;

        if (vectorList.Count >= 4)
        {
            int add = -1;
            for (int i = vectorList.Count - 1, j = 0; j < vectorList.Count - 2; i = j, j++)
            {
                for (int c = j + 1; c < vectorList.Count - 1 + add; c++)
                {
                    if (Util.detectIntersection(vectorList[i], vectorList[j], vectorList[c], vectorList[c + 1]))
                    {
                        lr.material = collisionDetectedMaterial;
                        truePolygon = false;
                        return false;
                    }
                }
                add = 0;
            }
            lr.material = new Material(Shader.Find("Sprites/Default"));
            truePolygon = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void saveVector2s()
    {
        serializableList.list.Clear();

        if (polygonFinished)
        {
            //vectorList.RemoveAt(vectorList.Count - 1);
            foreach (Vector3 v3 in vectorList)
            {
                serializableList.list.Add(new Vector2(v3.x, v3.z));
            }
            //serializableList.list.RemoveAt(serializableList.list.Count - 1);
            SavePoints.Save(serializableList);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (vectorList.Count > 0)
        {
            Gizmos.DrawWireSphere(vectorList[0], 0.1f);

            foreach (Vector3 vec3 in vectorList)
            {
                Gizmos.DrawSphere(vec3, 0.05f);
            }
        }
    }*/
}
