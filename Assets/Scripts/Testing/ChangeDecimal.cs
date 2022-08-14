using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDecimal : MonoBehaviour
{
    [SerializeField]
    DrawMeasurements drawMeasurements;

    /*private void Awake()
    {
        placeOnPlane = sessionOrigin.GetComponent<PlaceOnPlane>();
    }*/

    public void increaseDecimal()
    {
        if (drawMeasurements.decimalCount > 10)
        {
            drawMeasurements.decimalCount++;
        }
    }

    public void decreaseDecimal()
    {
        if (drawMeasurements.decimalCount > 2)
        {
            drawMeasurements.decimalCount--;
        }
    }
}
