using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraOrbit : MonoBehaviour
{
    [Header("Camera Target")]
    [SerializeField] private Transform orbitTarget;

    [Header("Zoom")]
    private float distance = 10.0f;  // Distance from the target
    [SerializeField] [Range(0.5f, 3)] private float minZoomDistance = 2f;  
    [SerializeField] [Range(4, 20)] private float maxZoomDistance = 20f;
    [SerializeField] [Range(1, 20)] private float zoomSpeed = 10.0f;

    [Header("Orbit")]
    [SerializeField] [Range(1, 1000)] private float xOrbitSpeed = 120.0f;  
    [SerializeField] [Range(1, 1000)] private float yOrbitSpeed = 120.0f;  
    [SerializeField] [Range(-360, 0)] private float yMinAngleLimit = -20f;  
    [SerializeField] [Range(0, 360)] private float yMaxAngleLimit = 80f;

    [Header("Pan")]
    [SerializeField] [Range(0.1f, 5)] private float panSpeed = 0.3f;  

    [Header("Focus")]
    [SerializeField] [Range(1, 10)] private float focusSpeed = 5.0f;
    
    
    private float x = 0.0f;  // Current X rotation
    private float y = 0.0f;  // Current Y rotation
    private Vector3 panOffset;  // Offset for panning

    private Vector3 lastMousePosition;
    private bool isFocusing = false; 

    private Vector3 focusTargetPosition;  // Target position during focus
    private Quaternion focusTargetRotation;  // Target rotation during focus

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        distance = Vector3.Distance(transform.position, orbitTarget.position);
        panOffset = Vector3.zero;

        StartFocus();
    }

    void LateUpdate()
    {
        if (orbitTarget)
        {
            if (isFocusing)
            {  
                PerformFocus();
            }
            else
            {
                if (Input.GetMouseButton(1))
                {
                    x += Input.GetAxis("Mouse X") * xOrbitSpeed * Time.deltaTime;
                    y -= Input.GetAxis("Mouse Y") * yOrbitSpeed * Time.deltaTime;
                    y = ClampAngle(y, yMinAngleLimit, yMaxAngleLimit);
                    isFocusing = false;
                }

                //if (Input.GetMouseButton(2))
                //{
                //    Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                //    Vector3 translation = (transform.right * -mouseDelta.x + transform.up * -mouseDelta.y) * panSpeed * Time.deltaTime;
                //    panOffset += translation;
                //    isFocusing = false;
                //}

                //if (Input.GetKeyDown(KeyCode.F))
                //{    
                //    StartFocus();
                //}

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoomDistance, maxZoomDistance);

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + orbitTarget.position + panOffset;

                transform.rotation = rotation;
                transform.position = position;

                lastMousePosition = Input.mousePosition;
            }
            
        }
    }

    void StartFocus()
    {
        // Calculate the direction and set target rotation and position for focus
        Vector3 directionToTarget = (orbitTarget.position - transform.position).normalized;
        focusTargetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        focusTargetPosition = orbitTarget.position - (focusTargetRotation * Vector3.forward * distance);

        isFocusing = true; 
    }

    void PerformFocus()
    {
        // Smoothly interpolate the camera's position and rotation
        transform.position = Vector3.Lerp(transform.position, focusTargetPosition, focusSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, focusTargetRotation, focusSpeed * Time.deltaTime);

        // Check if the camera has reached the desired position and rotation
        if (Vector3.Distance(transform.position, focusTargetPosition) < 0.1f && Quaternion.Angle(transform.rotation, focusTargetRotation) < 1.0f)
        {
            Debug.Log("Focus complete.");
            isFocusing = false;
            panOffset = Vector3.zero;

            // Update x and y angles to match the new rotation
            Vector3 newAngles = transform.eulerAngles;
            x = newAngles.y;
            y = newAngles.x;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
