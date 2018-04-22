using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float cameraSpeed = 100f; //rychlost pohybu kamery
    public float screenBorder = 10f; //když najedeš myškou na hranice obrazovky, kamera se bude pohybovat
    public float scrollSpeed = 10f; // Rychlost Zoomu
    public float smoothness = 0.25f;
    public float rotationSpeed = 100f;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    public GameObject bounds;
    float xMin, xMax, yMin, yMax, zMin, zMax; 
    void Update()
    {
        float translation = cameraSpeed * Time.deltaTime;

        targetPosition = transform.position;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - screenBorder)
            targetPosition += translation * new Vector3(transform.forward.x, 0, transform.forward.z);
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= screenBorder)
            targetPosition -= translation * new Vector3(transform.forward.x, 0, transform.forward.z);
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= screenBorder)
            targetPosition -= translation * new Vector3(transform.right.x, 0, transform.right.z);
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - screenBorder)
            targetPosition += translation * new Vector3(transform.right.x, 0, transform.right.z);

        if (Input.GetKey(KeyCode.Q))
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 10 * rotationSpeed * Time.deltaTime, transform.rotation.eulerAngles.z);
        if (Input.GetKey(KeyCode.E))
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 10 * rotationSpeed * Time.deltaTime, transform.rotation.eulerAngles.z);

        float scroll = Input.mouseScrollDelta.y;   //zjistí input scrollu na myšce
        targetPosition += transform.forward * scroll * scrollSpeed * Time.deltaTime;    //zoomuje

        if (bounds)
        {
            //kontroluje přítomnost v krychli objektu bounds
            xMin = bounds.transform.position.x - bounds.transform.localScale.x / 2;
            xMax = bounds.transform.position.x + bounds.transform.localScale.x / 2;
            yMin = bounds.transform.position.y - bounds.transform.localScale.y / 2;
            yMax = bounds.transform.position.y + bounds.transform.localScale.y / 2;
            zMin = bounds.transform.position.z - bounds.transform.localScale.z / 2;
            zMax = bounds.transform.position.z + bounds.transform.localScale.z / 2;

            bool isMin;
            if ((isMin = xMin > targetPosition.x) || xMax < targetPosition.x)
                targetPosition.x = isMin ? xMin : xMax;
            if ((isMin = yMin > targetPosition.y) || yMax < targetPosition.y)
                targetPosition.y = isMin ? yMin : yMax;
            if ((isMin = zMin > targetPosition.z) || zMax < targetPosition.z)
                targetPosition.z = isMin ? zMin : zMax;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1 - smoothness);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 - smoothness);
    }
}
