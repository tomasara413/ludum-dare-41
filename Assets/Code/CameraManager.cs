using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
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

    private BuildingManager bm;
    public BuildingList buildingList;

    public GameObject FogOfWar;
    private Renderer rend;
    private Shader FOW;
    private Camera cam;
    private int layerMask;

    private void Start()
    {
        bm = GameObject.FindGameObjectWithTag("Managers").GetComponent<BuildingManager>();
        buildingList = bm.GetBuildingList();
        rend = FogOfWar.GetComponent<Renderer>();
        cam = GetComponent<Camera>();
        FOW = Shader.Find("Unlit/FogOfWar");
        layerMask = LayerMask.GetMask("FogOfWar");
    }

    void Update()
    {
        UpdateMovement();
        UpdateFOW();
    }

    void UpdateMovement()
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

    List<GameObject> teamObs;
    Vector4[] camVectors;
    float[] distances;
    int previousCount = 0;
    RaycastHit rh;
    void UpdateFOW()
    {
        if (teamObs == null)
            teamObs = buildingList.GetListOfVisionObjects(0);

        if (teamObs != null)
        {
            //Debug.Log(teamObs.Count);
            camVectors = new Vector4[teamObs.Count == 0 ? 1 : teamObs.Count];
            distances = new float[teamObs.Count == 0 ? 1 : teamObs.Count];
            for (int i = 0; i < teamObs.Count; i++)
            {
                if (Physics.Raycast(transform.position, teamObs[i].transform.position - transform.position, out rh, cam.farClipPlane, layerMask))
                    camVectors[i] = rh.point;
                distances[i] = teamObs[i].GetComponent<Building>().VisionRange;
            }

            rend.material.SetInt("_VectorsCount", teamObs.Count);
            /*if (previousCount != teamObs.Count)
            {
                rend.material = new Material(FOW);
                
                previousCount = teamObs.Count;
            }*/

            rend.material.SetVectorArray("_Vectors", camVectors);
            rend.material.SetFloatArray("_Distances", distances);
        }
    }
}
