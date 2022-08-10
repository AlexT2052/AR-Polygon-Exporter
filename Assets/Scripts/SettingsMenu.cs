using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    DrawMeasurements drawMeasurements;

    [SerializeField]
    GameObject settingsIcon;

    [SerializeField]
    MeshGenerator meshGenerator;

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
}
