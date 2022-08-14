using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float CrossProduct(Vector2 a, Vector2 b )
    {
        // cz = axby - aybx
        return a.x * b.y - a.y * b.x;
    } 

    public static Vector2 To3DVector2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    public static Vector2 ToVector3 (Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    public static T GetItemFromCircularArray<T>(T[] array, int index)
    {
        if(index >= array.Length)
        {
            return array[index % array.Length];
        }
        else if (index < 0)
        {
            return array[index % array.Length + array.Length];
        }
        else
        {
            return array[index];
        }
    }
    public static T GetItemFromCircularArray<T>(List<T> list, int index)
    {
        if (index >= list.Count)
        {
            return list[index % list.Count];
        }
        else if (index < 0)
        {
            return list[index % list.Count + list.Count];
        }
        else
        {
            return list[index];
        }
    }

    /**
     * Algorithim from here: https://ideone.com/PnPJgb
     * Information from here: https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
     * 
    */
    public static bool detectIntersection(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 CmP = new Vector3(C.x - A.x, 0, C.z - A.z);
        Vector3 r = new Vector3(B.x - A.x, 0, B.z - A.z);
        Vector3 s = new Vector3(D.x - C.x, 0, D.z - C.z);

        float CmPxr = CmP.x * r.z - CmP.z * r.x;
        float CmPxs = CmP.x * s.z - CmP.z * s.x;
        float rxs = r.x * s.z - r.z * s.x;

        if (CmPxr == 0f)
        {
            // Lines are collinear, and so intersect if they have any overlap

            return ((C.x - A.x < 0f) != (C.x - B.x < 0f))
                || ((C.z - A.z < 0f) != (C.z - B.z < 0f));
        }

        if (rxs == 0f)
            return false; // Lines are parallel.

        float rxsr = 1f / rxs;
        float t = CmPxs * rxsr;
        float u = CmPxr * rxsr;

        return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
    }

    /**
     * Credit https://answers.unity.com/questions/684909/how-to-calculate-the-surface-area-of-a-irregular-p.html
     */
    public static float superficieOfIrregularPolygon(List<Vector3> vectorList)
    {
        float temp = 0;
        int i = 0;
        for (; i < vectorList.Count; i++)
        {
            if (i != vectorList.Count - 1)
            {
                float mulA = vectorList[i].x * vectorList[i + 1].z;
                float mulB = vectorList[i + 1].x * vectorList[i].z;
                temp = temp + (mulA - mulB);
            }
            else
            {
                float mulA = vectorList[i].x * vectorList[0].z;
                float mulB = vectorList[0].x * vectorList[i].z;
                temp = temp + (mulA - mulB);
            }
        }
        temp *= 0.5f;
        return Mathf.Abs(temp);
    }

    public static List<Vector2> ConvertToCorrectVector2s(List<Vector3> vectorList)
    {
        List<Vector2> newVectorList = new List<Vector2>();

        foreach (Vector3 v3 in vectorList)
        {
            newVectorList.Add(new Vector2(v3.x, v3.z));
        }

        return newVectorList;
    }
}
