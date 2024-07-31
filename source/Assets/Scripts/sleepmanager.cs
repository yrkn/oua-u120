using UnityEngine;
using UnityEngine.UI; 
using System.Collections; 

public class SleepManager : MonoBehaviour
{
    [SerializeField] public GameObject sleepArea; // uyuma alanı
    [SerializeField] public float sleepDuration = 10f; // uyuma süresi
    [SerializeField] public float fadeDuration = 2f; // ekranın kararma ve açılma süresi
    [SerializeField] public float reactivateDelay = 10f; // objeyi geri açma süresi
    [SerializeField] public float timeToAdd = 50f; // currentTime'a eklenecek süre
    [SerializeField] public Image fadeImage; 

    private bool isSleeping = false;
    private DayNightManager dayNightManager;
    private PlayerHealth playerHealth;
    private PlayerController playerController;

    private void Start()
    {
        dayNightManager = FindObjectOfType<DayNightManager>();
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();

        if (fadeImage != null)
        {
            Color tempColor = fadeImage.color;
            tempColor.a = 0f;
            fadeImage.color = tempColor;
        }
        else
        {
           // Debug.LogError("image yok");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SleepArea") && !isSleeping)
        {
            StartCoroutine(SleepRoutine());
        }
    }

    private IEnumerator SleepRoutine()
    {
        isSleeping = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        yield return StartCoroutine(FadeToAlpha(1f));

        if (playerHealth != null)
        {
            playerHealth.currentHealth = 100; 
        }
        if (dayNightManager != null)
        {
            dayNightManager.currentTime += timeToAdd; 
        }

        if (sleepArea != null)
        {
            sleepArea.SetActive(false);
        }

        yield return new WaitForSeconds(sleepDuration);

        if (playerController != null)
        {
            playerController.enabled = true; 
        }
        yield return StartCoroutine(FadeToAlpha(0f));

        yield return new WaitForSeconds(reactivateDelay);
        if (sleepArea != null)
        {
            sleepArea.SetActive(true);
        }

        isSleeping = false;
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float elapsedTime = 0f;
        Color startingColor = fadeImage.color;
        Color targetColor = new Color(startingColor.r, startingColor.g, startingColor.b, targetAlpha);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startingColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
    }
}
