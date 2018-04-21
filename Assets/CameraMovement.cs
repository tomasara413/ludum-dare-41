using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float cameraSpeed = 30f; //rychlost pohybu kamery
    public float screenBorder = 10f; //když najedeš myškou na hranice obrazovky, kamera se bude pohybovat
    public float scrollSpeed = 30f; // Rychlost Zoomu
    



    void Start()
    {
    }

    void Update()
    {
        float translation = cameraSpeed * Time.deltaTime; 

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - screenBorder)
        {
            transform.Translate(0,0, translation, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= screenBorder)
        {
            transform.Translate(0, 0, -translation, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= screenBorder)
        {
            transform.Translate(-translation, 0, 0, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.height - screenBorder)
        {
            transform.Translate(translation, 0, 0, Space.World);
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");   //zjistí input scrollu na myšce
        transform.Translate(0, 0, scroll * scrollSpeed * 10f * Time.deltaTime);     //zoomuje

        //Je potřeba dokončit:
        //1) hranice světa a hranice zoomu
        //2) rotaci kamery
        //3) Udělat kameru pěkne SMOOTH :3

    }
}
