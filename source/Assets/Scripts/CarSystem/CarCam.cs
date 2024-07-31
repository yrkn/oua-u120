using UnityEngine;

public class CarCam : MonoBehaviour
{
    public Transform target; 
    public float distance = 5.0f; // araba ile uzaklık
    public float height = 2.0f; // yerden yük.
    public float rotationSpeed = 5.0f; // dönüş hızı
    public float minY = -20f; // aşağı sınır
    public float maxY = 80f; // yukarı sınır
    public float smoothTime = 0.1f; 

    private float currentX = 0f;
    private float currentY = 0f;
    private float smoothX = 0f;
    private float smoothY = 0f;
    private float smoothVelocityX = 0f;
    private float smoothVelocityY = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target)
            return;
      
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
       
        currentY = Mathf.Clamp(currentY, minY, maxY);
        
        smoothX = Mathf.SmoothDamp(smoothX, currentX, ref smoothVelocityX, smoothTime);
        smoothY = Mathf.SmoothDamp(smoothY, currentY, ref smoothVelocityY, smoothTime);
      
        Quaternion rotation = Quaternion.Euler(smoothY, smoothX, 0);
      
        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance + new Vector3(0, -height, 0));

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothTime);
    }

    void OnApplicationFocus(bool hasFocus)
    {   
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
