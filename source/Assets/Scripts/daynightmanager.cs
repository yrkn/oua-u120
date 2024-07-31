using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayDuration = 60f; 
    [SerializeField] private float nightDuration = 120f;
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;
    [SerializeField] public float currentTime = 0f; 
    [SerializeField] public int dayCount = 0; // GÜN SAYISI BURADA TUTULUYOR
    [SerializeField] private string streetLightTag = "light"; 

    private float sunInitialIntensity;

    void Start()
    {
        if (directionalLight != null)
        {
            sunInitialIntensity = directionalLight.intensity;
        }

        RenderSettings.skybox = daySkybox;

        SetInitialSunPosition();
        SetStreetLights(false); 
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= (dayDuration + nightDuration))
        {
            currentTime = 0f;
            dayCount++; 
            Daily();
        }

        UpdateSun();
        UpdateSkybox();
        UpdateStreetLights();
    }

    void SetInitialSunPosition()
    {
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(90f, 170f, 0f));
        directionalLight.intensity = sunInitialIntensity;
        directionalLight.shadowStrength = 0.5f; 
        directionalLight.shadowBias = 0.05f; 
        directionalLight.shadowNormalBias = 0.4f; 
    }

    void UpdateSun()
    {
        if (currentTime >= 50f && currentTime < 60f)
        {
            float sunAngle = Mathf.Lerp(90f, -90f, (currentTime - 50f) / 10f); 
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));
            directionalLight.intensity = Mathf.Lerp(sunInitialIntensity, 0f, (currentTime - 50f) / 10f); 
        }
        else if (currentTime >= 60f && currentTime < 170f)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(-90f, 170f, 0f));
            directionalLight.intensity = 0f;
        }
        else if (currentTime >= 170f && currentTime < 180f)
        {
            float sunAngle = Mathf.Lerp(-90f, 90f, (currentTime - 170f) / 10f); 
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));
            directionalLight.intensity = Mathf.Lerp(0f, sunInitialIntensity, (currentTime - 170f) / 10f); 
        }
        else if (currentTime >= 180f)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(90f, 170f, 0f));
            directionalLight.intensity = sunInitialIntensity;
            currentTime = 0f; 
        }
    }

    void UpdateSkybox()
    {
        if (currentTime < 60f)
        {
            RenderSettings.skybox = daySkybox;
        }
        else if (currentTime >= 60f && currentTime < 180f)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else if (currentTime >= 180f)
        {
            RenderSettings.skybox = daySkybox;
        }
    }

    void UpdateStreetLights()
    {
        bool areLightsOn = currentTime >= 60f && currentTime < 180f;
        SetStreetLights(areLightsOn);
    }

    void SetStreetLights(bool status)
    {
        GameObject[] streetLights = GameObject.FindGameObjectsWithTag(streetLightTag);
        foreach (GameObject light in streetLights)
        {
            Light lightComponent = light.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = status;
            }
        }
    }

    void Daily()
    {
        Debug.Log("Gün tamamlandı: " + dayCount);
    }
}
