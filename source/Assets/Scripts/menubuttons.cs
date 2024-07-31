using UnityEngine;
using UnityEngine.SceneManagement;

public class menubuttons : MonoBehaviour
{
    
    public GameObject kontroltusu;
    public GameObject kontroldencikis;
    
    
    public void kontrolegiris()
    {
        kontroltusu.SetActive(true);
    }

    public void kontroldencikistusu()
    {
        kontroltusu.SetActive(false);
    }

    public void basla()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // 1 - 0 - 0 eski hali 

    public void yenidendene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void menudonus()
    {
        Time.timeScale = 1f;
        ZombieAI.totalZombiesKilled = 0; 
        SceneManager.LoadScene(1);      
    }
}
