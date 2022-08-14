using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawMeasurements : MonoBehaviour
{
    [SerializeField]
    private TextMesh distanceTextPrefab;

    List<TextMesh> textMeshes;
    public int decimalCount
    { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        decimalCount = 4;
        textMeshes = new List<TextMesh>();
        //vectorList = new List<Vector2>();
    }

    public void Draw(List<Vector3> vectorList)
    {
        
        if (vectorList.Count >= 2 && textMeshes.Count != (vectorList.Count - 1))
        {
            for (int i = textMeshes.Count; i < vectorList.Count; i++)
            {
                textMeshes.Add(Instantiate(distanceTextPrefab, Vector3.Lerp(vectorList[vectorList.Count - 2], vectorList[vectorList.Count - 1], 0.5f), Quaternion.identity));
            }
        }

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

    //Attempt at doing with only stored vector2s, not fun or easy
    public void Draw(List<Vector2> vectorList)
    {
        if (vectorList.Count >= 2 && textMeshes.Count != (vectorList.Count - 1))
        {
            for (int i = textMeshes.Count; i < vectorList.Count; i++)

            textMeshes.Add(Instantiate(distanceTextPrefab, Util.ToVector3(Vector2.Lerp(vectorList[vectorList.Count - 1], vectorList[vectorList.Count - 2], 0.5f)), Quaternion.identity));
        }

        for (int i = 0; i < textMeshes.Count; i++)
        {
            float distanceBetweenBoth = (float)Math.Round(Vector2.Distance(vectorList[i], vectorList[i + 1]), decimalCount);
            textMeshes[i].transform.position = Util.ToVector3(Vector2.Lerp(vectorList[i], vectorList[i + 1], 0.5f));
            Vector3 direction = vectorList[i] - vectorList[i + 1];
            textMeshes[i].transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            textMeshes[i].transform.Rotate(0, 90, 0);
            textMeshes[i].transform.Translate(new Vector3(0, 0.05f, 0));
            textMeshes[i].text = distanceBetweenBoth.ToString() + " m";
        }
    }
}
