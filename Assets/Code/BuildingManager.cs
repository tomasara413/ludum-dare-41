using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    
    GameObject objectToPlace;
    Transform child;
    Camera cam;

    private bool previousPlaceable = false;
    public bool Placeable = true;

    private int terrainMask;
    public int TerrainMask
    {
        get { return terrainMask; }
    }
    private Rigidbody rigid;

    public float MaxDeltaY;
    ObjectManager om;
    ResourcesManager rm;

    void Start () {
        om = GameObject.FindGameObjectWithTag("Managers").GetComponent<ObjectManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GameObject managers = GameObject.FindGameObjectWithTag("Managers");
        rm = managers.GetComponent<ResourcesManager>();
        terrainMask = LayerMask.GetMask("Terrain");
    }
	
	void Update () {
        if (objectToPlace)
        {
            if (child == null)
            {
                foreach (Transform child in objectToPlace.transform)
                {
                    if (child.name == "Mesh")
                    {
                        this.child = child;
                        break;
                    }
                }
            }

            if (Placeable != previousPlaceable)
            {
                previousPlaceable = Placeable;

                ColorBuilding();
            }

            Vector3? worldPoint = getWorldPoint();
            if (worldPoint != null)
            {
                objectToPlace.SetActive(true);
                objectToPlace.transform.position = (Vector3)worldPoint;

                if (Input.GetMouseButtonDown(0) && Placeable)
                {
                    rm.GoldAmmount -= objectToPlace.GetComponent<Building>().Gold;
                    RecolorBuilding();
                    PlaceabilityDetector oc;
                    if (oc = objectToPlace.GetComponent<PlaceabilityDetector>())
                    {
                        Destroy(oc);
                        Destroy(rigid);
                    }
                    Building b;
                    if (b = objectToPlace.GetComponent<Building>())
                        b.Placed = true;

                    om.GetTeamObjectList().AddGameObjectToList(b.team, objectToPlace);

                    objectToPlace = null;
                    child = null;
                    return;
                }
            }
            else
                objectToPlace.SetActive(false);

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(objectToPlace);
                objectToPlace = null;
                child = null;
            }
        }
	}

    public void StartPlacing(GameObject newObject)
    {
        previousPlaceable = !(Placeable = true);
        objectToPlace = newObject;
        rigid = objectToPlace.AddComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    public Vector3? getWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cam.farClipPlane, terrainMask))
            return hit.point;
        return null;
    }

    List<Color> previousClr = new List<Color>();
    private bool backupColor = true;
    void ColorBuilding()
    {
        Renderer rend;
        int j = 0;

        if (child != null)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                if (rend = child.GetChild(i).GetComponent<Renderer>())
                {
                    foreach (Material material in rend.materials)
                    {  
                        if (backupColor)
                            previousClr.Add(material.color);
                        // Možná se tohle bude muset nastavit v původních materálech 
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.DisableKeyword("_ALPHABLEND_ON");
                        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = 3000;

                        material.color = Color.Lerp(previousClr[j], Placeable ? new Color(0.447f, 0.957f, 0, 0.25f) : new Color(1f, 0f, 0f, 0.25f), 0.5f);
                        j++;
                    }
                }
            }
            backupColor = false;
        }
    }

    void RecolorBuilding()
    {
        Renderer rend;
        int j = 0;

        if (child != null)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                if (rend = child.GetChild(i).GetComponent<Renderer>())
                {
                    foreach (Material material in rend.materials)
                    {
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        material.SetInt("_ZWrite", 1);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.DisableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = -1;

                        material.color = previousClr[j];
                        j++;
                    }
                }
            }

            previousClr.Clear();
            backupColor = true;
        }
    }

    
}
