using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    DrawMeasurements drawMeasurements;

    [SerializeField]
    GameObject settingsIcon;

    [SerializeField]
    MeshGenerator meshGenerator;

    [SerializeField]
    Text enableMeasurementsText;

    public void DisableBoolAnimator(Animator anim)
    {
        settingsIcon.SetActive(true);
        anim.SetBool ("IsDisplayed", false);
        meshGenerator.disableRaycasts = false;
    }

    public void EnableBoolAnimator(Animator anim)
    {
        settingsIcon.SetActive(false);
        anim.SetBool("IsDisplayed", true);
        meshGenerator.disableRaycasts = true;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

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

    public void toggleMeasurements()
    {
        if (meshGenerator.drawMeasurements)
        {
            meshGenerator.drawMeasurements = false;
            enableMeasurementsText.text = "Enable Measurements";
        }
        else
        {
            meshGenerator.drawMeasurements = true;
            enableMeasurementsText.text = "Disable Measurements";
        }

    }
}
