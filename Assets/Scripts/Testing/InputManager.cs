#define ON_ANDROID
#if UNITY_ANDROID
#undef ON_ANDROID
#endif

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;


public class InputManager : MonoBehaviour
{
    [SerializeField]
    private MeshGenerator meshGenerator;

#if ON_ANDROID
    [SerializeField]
    private ARRaycastManager m_RaycastManager;
#else
    [SerializeField]
    GameObject groundPlane;
#endif

    List<Vector3> vectorList;
    List<GameObject> placementIndicators;
    int hitIndicatorIndex = 0;
    bool repositioning = false;

    // Start is called before the first frame update
    void Start()
    {
        vectorList = new List<Vector3>();
        placementIndicators = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 20);

        bool hitGround = false;
        bool hitPoint = false;
        bool updateMesh = false;
        Vector3 groundHit = default;
        Vector3 pointHit = default;

#if ON_ANDROID
        if (Input.touchCount > 0)
        {
            List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

            Vector2 touchPosition = Input.GetTouch(0).position;
            if (m_RaycastManager.Raycast(Input.mousePosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                hitGround = true;
                groundHit = s_Hits[0].pose.position;
            }
        }
#else


        foreach (RaycastHit rh in raycastHits)
        {
            if (rh.transform.gameObject == groundPlane)
            {
                hitGround = true;
                groundHit = rh.point;
            }
        }
#endif

        foreach (RaycastHit rh in raycastHits)
        {
            for (int i = 0; i < placementIndicators.Count; i++)
            {
                if (rh.transform.gameObject == placementIndicators[i])
                {
                    hitPoint = true;
                    pointHit = rh.point;
                    hitIndicatorIndex = i;
                }
            }
        }

        if (hitGround)
        {
            if (hitGround && hitPoint)
            {
                repositioning = true;
            }

            if (repositioning)
            {
                placementIndicators[hitIndicatorIndex].transform.position = groundHit;
                vectorList[hitIndicatorIndex] = groundHit;
                updateMesh = true;
            }
        }


        if (updateMesh)
        {
            /*meshGenerator.DrawLines();
            if ()
            meshGenerator.DrawMesh();*/

            //meshGenerator.Update
        }
    }
}
