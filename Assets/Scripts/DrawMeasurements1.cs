using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DrawMeasurements1 : MeshGenerator
{
    [SerializeField]
    private MeshGenerator meshGenerator;

    [SerializeField]
    private TextMesh distanceTextPrefab;

    List<TextMesh> textMeshes;
    private List<Vector3> vectorList;
    public int decimalCount
    { get; set; }
    /*{
        get { return textMeshes.Count; }
        set { decimalCount = value; }
    }*/
    private int lastCount;

    // Start is called before the first frame update
    void Start()
    {
        decimalCount = 4;
        textMeshes = new List<TextMesh>();
        //vectorList = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (meshGenerator.getVectorList(out vectorList) && textMeshes.Count > 0)
        {
            for (int i = 0; i < textMeshes.Count; i++)
            {
                float distanceBetweenBoth = (float)Math.Round(Vector3.Distance(vectorList[i], vectorList[i + 1]), decimalCount);
                textMeshes[i].transform.position = Vector3.Lerp(vectorList[i], vectorList[i + 1], 0.5f);
                Vector3 direction = vectorList[i] - vectorList[i + 1];
                textMeshes[i].transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                textMeshes[i].transform.Rotate(0, 90, 0);
                textMeshes[i].transform.Translate(new Vector3(0, 0.05f, 0));
                textMeshes[i].text = distanceBetweenBoth.ToString() + " m";
            }
        }

        if (textMeshes.Count != (vectorList.Count - 1))
        {
            textMeshes.Add(Instantiate(distanceTextPrefab, Vector3.Lerp(vectorList[vectorList.Count - 1], vectorList[vectorList.Count - 2], 0.5f), Quaternion.identity));
        }
    }
}
