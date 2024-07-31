using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Başlangıç canı
    public int currentHealth; // Güncel can
    public bool isDead;
    public RuntimeAnimatorController animatorController;
    public int damageAmount = 20; // Zombi hasar miktarı

    public PlayerController playerController;
    public Weapon weapon;
    public tpscam tpscam; 
    public Canvas canvas; 
    public string healthUIImageName = "HealthBar"; 
    private Animator animator;

    private Image healthUI; 

    public GameObject healZone; 
    public float healInterval = 3f; 
    public int healAmount = 20; 

    private bool isInHealZone = false; 
    private float uiUpdateInterval = 0.1f; // UI güncelleme aralığı
    private Coroutine uiUpdateCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = animatorController;

        if (canvas != null)
        {
            Transform healthUITransform = canvas.transform.Find(healthUIImageName);
            if (healthUITransform != null)
            {
                healthUI = healthUITransform.GetComponent<Image>();
            }
        }

        UpdateHealthUI(); 

        if (uiUpdateCoroutine != null)
        {
            StopCoroutine(uiUpdateCoroutine);
        }
        uiUpdateCoroutine = StartCoroutine(UpdateHealthUICoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieTrigger") && !isDead)
        {
            ChangeHealth(-damageAmount); 
        }

        if (other.gameObject == healZone)
        {
            isInHealZone = true;
            StartCoroutine(HealOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == healZone)
        {
            isInHealZone = false;
            StopCoroutine(HealOverTime()); 
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 

        UpdateHealthUI(); // Can güncellemesini hemen yaptıktan sonra UI'ı güncelle

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthUI.fillAmount = fillAmount;
        }
    }

    private IEnumerator UpdateHealthUICoroutine()
    {
        while (!isDead)
        {
            UpdateHealthUI();
            yield return new WaitForSeconds(uiUpdateInterval);
        }
    }

    private void Die()
    {

        if (isDead) return;

        isDead = true;

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        if (tpscam != null)
        {
            tpscam.enabled = false;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (weapon != null)
        {
            weapon.enabled = false;
        }

        if (canvas != null)
        {
            StartCoroutine(DisableCanvasAfterDelay(2f));
        }

        if (uiUpdateCoroutine != null)
        {
            StopCoroutine(uiUpdateCoroutine);
        }
    }

    private IEnumerator HealOverTime()
    {
        while (isInHealZone)
        {
            ChangeHealth(healAmount); // Canı artır
            yield return new WaitForSeconds(healInterval); 
        }
    }

    private IEnumerator DisableCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (Application.isPlaying && healthUI != null)
        {
            UpdateHealthUI();
        }
    }
}
