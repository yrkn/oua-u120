using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class winmanager : MonoBehaviour
{
    public ZombieAI zombieAI; 
    public Image fadeImage; 
    public Image aimImage; 
    public float slowFactor = 2f; // oyunun yavaşlama katsayısı
    public float fadeDuration = 5f; 
    public float waitAfterFade = 2f; // KULLANMA
    [SerializeField] public float killSayisi = 50f; 

    private bool hasWon = false;

    void Start()
    {
        fadeImage.gameObject.SetActive(false);

        if (aimImage != null)
        {
            aimImage.gameObject.SetActive(true);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!hasWon && ZombieAI.totalZombiesKilled >= killSayisi) // static erişim
        {
            hasWon = true;
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 1 / slowFactor;

        if (aimImage != null)
        {
            aimImage.gameObject.SetActive(false);
        }

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

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(3);
    }
}