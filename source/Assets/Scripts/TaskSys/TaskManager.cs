using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  

public class TaskManager : MonoBehaviour
{
    [SerializeField] private DayNightManager dayNightManager;  
    [SerializeField] private string streetLightTag = "light";  
    [SerializeField] private GameObject warningCanvas;  
    [SerializeField] private GameObject vfxObject;  
    [SerializeField] private Camera taskCamera;  
    [SerializeField] private Camera playerCamera;  
    [SerializeField] private MonoBehaviour playerController;  
    [SerializeField] private Main mainScript;  

    private bool eventTriggered = false;  
    private List<GameObject> streetLights = new List<GameObject>();  

    private void Start()
    {
        if (warningCanvas != null)
        {
            warningCanvas.SetActive(false);
        }
        if (vfxObject != null)
        {
            vfxObject.SetActive(false);
        }

        if (taskCamera != null)
        {
            taskCamera.gameObject.SetActive(false);
        }

        GameObject[] lights = GameObject.FindGameObjectsWithTag(streetLightTag);
        foreach (GameObject light in lights)
        {
            streetLights.Add(light);
        }
    }

    private void Update()
    {
        if (!eventTriggered && dayNightManager.currentTime >= 90f)
        {
            eventTriggered = true;
            TriggerEvent();
        }

        CheckTaskCompletion();
    }

    private void TriggerEvent()
    {
        foreach (GameObject light in streetLights)
        {
            light.SetActive(false);
        }

        if (warningCanvas != null)
        {
            warningCanvas.SetActive(true);
        }

        if (vfxObject != null)
        {
            vfxObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == vfxObject)
        {
            SetCursorVisibility(true);

            if (taskCamera != null)
            {
                taskCamera.gameObject.SetActive(true);
            }
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }

            if (playerController != null)
            {
                playerController.enabled = false;
            }
        }
    }

    private void CheckTaskCompletion()
    {
        if (mainScript != null && mainScript.taskStatus == true)
        {
            CompleteTask();
        }
    }

    private void CompleteTask()
    {
        if (warningCanvas != null)
        {
            warningCanvas.SetActive(false);
        }
        if (vfxObject != null)
        {
            vfxObject.SetActive(false);
        }

        if (taskCamera != null)
        {
            taskCamera.gameObject.SetActive(false);
        }
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }

        foreach (GameObject light in streetLights)
        {
            light.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        SetCursorVisibility(false);
    }

    private void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
