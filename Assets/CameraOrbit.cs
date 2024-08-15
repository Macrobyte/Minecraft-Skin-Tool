using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;  // The object the camera orbits around
    public float distance = 10.0f;  // Default distance from the target
    public float xSpeed = 120.0f;  // Orbit speed around the X-axis
    public float ySpeed = 120.0f;  // Orbit speed around the Y-axis
    public float zoomSpeed = 10.0f;  // Speed of zooming in/out
    public float panSpeed = 0.3f;  // Speed of panning
    public float focusSpeed = 5.0f; // Speed of focusing on the target

    public float yMinLimit = -20f;  // Minimum Y rotation
    public float yMaxLimit = 80f;  // Maximum Y rotation
    public float distanceMin = 2f;  // Minimum zoom distance
    public float distanceMax = 20f;  // Maximum zoom distance

    private float x = 0.0f;  // Current X rotation
    private float y = 0.0f;  // Current Y rotation
    private Vector3 panOffset;  // Offset for panning

    private Vector3 lastMousePosition;  // Last recorded mouse position
    private bool isFocusing = false;  // Whether the camera is currently focusing on the target

    private Vector3 focusTargetPosition;  // Target position during focus
    private Quaternion focusTargetRotation;  // Target rotation during focus

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        distance = Vector3.Distance(transform.position, target.position);
        panOffset = Vector3.zero;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        StartFocus();
    }

    void LateUpdate()
    {
        if (target)
        {
            if (isFocusing)
            {
                Debug.Log("Focusing...");
                PerformFocus();
            }
            else
            {
                if (Input.GetMouseButton(1))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                    isFocusing = false;
                }

                if (Input.GetMouseButton(2))
                {
                    Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                    Vector3 translation = (transform.right * -mouseDelta.x + transform.up * -mouseDelta.y) * panSpeed * Time.deltaTime;
                    panOffset += translation;
                    isFocusing = false;
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("F key pressed, starting focus...");
                    StartFocus();
                }



                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, distanceMin, distanceMax);

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position + panOffset;

                transform.rotation = rotation;
                transform.position = position;

                lastMousePosition = Input.mousePosition;
            }
            
        }
    }

    void StartFocus()
    {
        // Calculate the direction and set target rotation and position for focus
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        focusTargetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        focusTargetPosition = target.position - (focusTargetRotation * Vector3.forward * distance);

        isFocusing = true;

        Debug.Log("Focus initiated. Target position: " + focusTargetPosition + ", Target rotation: " + focusTargetRotation.eulerAngles);
    }

    void PerformFocus()
    {
        // Smoothly interpolate the camera's position and rotation
        transform.position = Vector3.Lerp(transform.position, focusTargetPosition, focusSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, focusTargetRotation, focusSpeed * Time.deltaTime);

        // Check if the camera has reached the desired position and rotation
        if (Vector3.Distance(transform.position, focusTargetPosition) < 0.1f &&
            Quaternion.Angle(transform.rotation, focusTargetRotation) < 1.0f)
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
