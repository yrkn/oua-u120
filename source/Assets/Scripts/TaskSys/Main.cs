using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main Instance;

    public int switchCount;
    private int onCount = 0;

    public bool taskStatus = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchChange(int points) 
    {
        onCount = onCount + points;
        if (onCount == switchCount)
        {
            taskStatus = true;
            StartCoroutine(ResetTaskStatusAfterDelay(5f)); 
        }
    }

    private IEnumerator ResetTaskStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        taskStatus = false; 
    }

    private void Update()
    {
        
    }
}
