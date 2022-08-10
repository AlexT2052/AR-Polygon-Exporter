using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PolygonHelper
{
    /*public static bool Triangulate(Vector3[] verticies, out int[] triangles, out string errorMessage)
    {
        triangles = null;
        errorMessage = string.Empty;

        if (verticies is null)
        {
            errorMessage = "The vertex list is null.";
            return false;
        }

        if (verticies.Length < 3)
        {
            errorMessage = "Vertex list must have at least 3 verticies.";
            return false;
        }

        if (verticies.Length > 1024)
        {
            errorMessage = "The max vertex list length is 1024";
            return false;
        }

        List<int> indexList = new List<int>();
        for (int i = 0; i < verticies.Length; i++)
        {
            indexList.Add(i);
        }

        int totalTriangleCount = verticies.Length - 1;
        int totalTriangleIndexCount = totalTriangleCount * 3;

        triangles = new int[totalTriangleIndexCount];
        int triangleIndexCount = 0;

        while (indexList.Count > 3)
        {
            for(int i = 0; i < indexList.Count; i++)
            {
                int a = indexList[i];
                int b = Util.GetItemFromCircularArray(indexList, i - 1);
                int c = Util.GetItemFromCircularArray(indexList, i + 1);

                //Vector2 va = verticies[a];
                //Vector2 vb = verticies[b];
                //Vector2 vc = verticies[c];

                Vector2 va = new Vector2(verticies[a].x, verticies[a].z);
                Vector2 vb = new Vector2(verticies[b].x, verticies[b].z);
                Vector2 vc = new Vector2(verticies[c].x, verticies[c].z);
                Vector2 va_to_vb = vb - va;
                Vector2 va_to_vc = vc - va;

                // Is ear test vertex convex?
                if(Util.CrossProduct(va_to_vb, va_to_vc) < 0f)
                {
                    continue;
                }

                bool isEar = true;

                // Does test eear contain any polygon verticies?
                for (int j = 0; j < verticies.Length; j++)
                {
                    if (j == a || j == b || j == c)
                    {
                        continue;
                    }

                    Vector2 p = verticies[j];

                    if (PolygonHelper.pointInTriangleTest(p, vb, va, vc))
                    {
                        isEar = false;
                        break;
                    }
                }

                if (isEar)
                {
                    triangles[triangleIndexCount++] = b;
                    triangles[triangleIndexCount++] = a;
                    triangles[triangleIndexCount++] = c;

                    indexList.RemoveAt(i);
                }
            }
        }

        triangles[triangleIndexCount++] = indexList[0];
        triangles[triangleIndexCount++] = indexList[1];
        triangles[triangleIndexCount++] = indexList[2];

        return true;
            
    }*/

    public static bool Triangulate(Vector2[] vertices, out int[] triangles, out string errorMessage)
    {
        triangles = null;
        errorMessage = string.Empty;

        if (vertices is null)
        {
            errorMessage = "The vertex list is null.";
            return false;
        }

        if (vertices.Length < 3)
        {
            errorMessage = "The vertex list must have at least 3 vertices.";
            return false;
        }

        if (vertices.Length > 1024)
        {
            errorMessage = "The max vertex list length is 1024";
            return false;
        }

        //if (!PolygonHelper.IsSimplePolygon(vertices))
        //{
        //    errorMessage = "The vertex list does not define a simple polygon.";
        //    return false;
        //}

        //if(PolygonHelper.ContainsColinearEdges(vertices))
        //{
        //    errorMessage = "The vertex list contains colinear edges.";
        //    return false;
        //}

        //PolygonHelper.ComputePolygonArea(vertices, out float area, out WindingOrder windingOrder);

        //if(windingOrder is WindingOrder.Invalid)
        //{
        //    errorMessage = "The vertices list does not contain a valid polygon.";
        //    return false;
        //}

        //if(windingOrder is WindingOrder.CounterClockwise)
        //{
        //    Array.Reverse(vertices);
        //}

        List<int> indexList = new List<int>();
        for (int i = 0; i < vertices.Length; i++)
        {
            indexList.Add(i);
        }

        int totalTriangleCount = vertices.Length - 2;
        int totalTriangleIndexCount = totalTriangleCount * 3;

        triangles = new int[totalTriangleIndexCount];
        int triangleIndexCount = 0;

        while (indexList.Count > 3)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int a = indexList[i];
                int b = Util.GetItemFromCircularArray(indexList, i - 1);
                int c = Util.GetItemFromCircularArray(indexList, i + 1);

                Vector2 va = vertices[a];
                Vector2 vb = vertices[b];
                Vector2 vc = vertices[c];

                Vector2 va_to_vb = vb - va;
                Vector2 va_to_vc = vc - va;

                // Is ear test vertex convex?
                if (Util.CrossProduct(va_to_vb, va_to_vc) < 0f)
                {
                    continue;
                }

                bool isEar = true;

                // Does test ear contain any polygon vertices?
                for (int j = 0; j < vertices.Length; j++)
                {
                    if (j == a || j == b || j == c)
                    {
                        continue;
                    }

                    Vector2 p = vertices[j];

                    if (PolygonHelper.pointInTriangleTest(p, vb, va, vc))
                    {
                        isEar = false;
                        break;
                    }
                }

                if (isEar)
                {
                    triangles[triangleIndexCount++] = b;
                    triangles[triangleIndexCount++] = a;
                    triangles[triangleIndexCount++] = c;

                    indexList.RemoveAt(i);
                    break;
                }
            }
        }

        triangles[triangleIndexCount++] = indexList[0];
        triangles[triangleIndexCount++] = indexList[1];
        triangles[triangleIndexCount++] = indexList[2];

        return true;
    }
    public static bool pointInTriangleTest(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        Vector2 ab = b - a;
        Vector2 bc = c - b;
        Vector2 ca = a - c;

        Vector2 ap = p - a;
        Vector2 bp = p - b;
        Vector2 cp = p - c;

        float cross1 = Util.CrossProduct(ab, ap);
        float cross2 = Util.CrossProduct(bc, bp);
        float cross3 = Util.CrossProduct(ca, cp);

        if (cross1 > 0f || cross2 > 0f || cross3 > 0f)
        {
            return false;
        }

        return true;
    }

    /*public static bool IsSimplePolygon(Vector2[] verticies)
    {
        throw new arg
    }*/

    //public static void ComputePolygonArea()
}
