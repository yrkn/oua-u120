using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class deadmanager : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public Image fadeImage; 
    public float slowFactor = 2f; // oyunun yavaşlama katsayısı
    public float fadeDuration = 5f; 
    public float waitAfterFade = 2f; // KULLANMA

    private bool isGameOver = false;

    void Start()
    {
        fadeImage.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isGameOver && playerHealth.currentHealth <= 0)
        {
            isGameOver = true;
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        Time.timeScale = 1 / slowFactor;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.unscaledDeltaTime; 
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSecondsRealtime(waitAfterFade);

        SceneManager.LoadScene(2);
    }
}
