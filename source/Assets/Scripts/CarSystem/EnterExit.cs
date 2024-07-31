using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnterExitSystem : MonoBehaviour
{

    public MonoBehaviour CarController;
    public Transform Car;
    public Transform Player;

    [Header("Kameralar")]
    public GameObject PlayerCam;
    public GameObject CarCam;


    bool Candrive;



    // Start is called before the first frame update
    void Start()
    {
        CarController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && Candrive)  
        {
           
            CarController.enabled = true; 

           
            Player.transform.SetParent(Car);
            Player.gameObject.SetActive(false);

           
            PlayerCam.gameObject.SetActive(false);
            CarCam.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
           

            CarController.enabled = false;


            Player.transform.SetParent(null);
            Player.gameObject.SetActive(true);

           
            PlayerCam.gameObject.SetActive(true);
            CarCam.gameObject.SetActive(false);
        }
    }
   

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Candrive = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Candrive = false;
        }
    }
}