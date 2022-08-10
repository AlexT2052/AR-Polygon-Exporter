using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetectSwitchingScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Camera arCamera;

    AudioListener mainCameraAudio;
    AudioListener arCameraAudio;


    private void Awake()
    {
        mainCameraAudio = mainCamera.GetComponent<AudioListener>();
        arCameraAudio = arCamera.GetComponent<AudioListener>();

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            mainCamera.enabled = true;
            mainCameraAudio.enabled = true;
            mainCamera.tag = "MainCamera";

            arCamera.enabled = false;
            arCameraAudio.enabled = false;
            mainCamera.tag = "Untagged";
        }
        else
        {
            mainCamera.enabled = false;
            mainCameraAudio.enabled = false;
            mainCamera.tag = "Untagged";

            arCamera.enabled = true;
            arCameraAudio.enabled = true;
            mainCamera.tag = "MainCamera";
        }
    }
}
