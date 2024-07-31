using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private bool isBreaking;
    private Rigidbody carRigidbody;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 1000f;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleSteering()
    {
        Vector3 rotation = transform.up * horizontalInput * maxSteeringAngle * Time.fixedDeltaTime;
        carRigidbody.MoveRotation(carRigidbody.rotation * Quaternion.Euler(rotation));
    }

    private void HandleMotor()
    {
        Vector3 force = transform.forward * verticalInput * motorForce;
        carRigidbody.AddForce(force);

        if (isBreaking)
        {
            Vector3 brakeForce = -carRigidbody.velocity * this.brakeForce * Time.fixedDeltaTime;
            carRigidbody.AddForce(brakeForce);
        }
    }
}
