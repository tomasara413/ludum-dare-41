using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMechanism : MonoBehaviour {

    GameObject objectToPlace;
    Transform child;
    Camera cam;
    void Start () {
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

            Vector3? worldPoint = getWorldPoint();
            if (worldPoint != null)
            {
                objectToPlace.SetActive(true);
                objectToPlace.transform.position = (Vector3)worldPoint;

                if (Input.GetMouseButtonDown(0))
                {
                    objectToPlace = null;
                    return;
                }
            }
            else
                objectToPlace.SetActive(false);

            if (Input.GetMouseButtonDown(1))
                Destroy(objectToPlace);
        }
	}

    void StartPlacing(GameObject newObject)
    {
        objectToPlace = newObject;
    }

    public Vector3? getWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cam.farClipPlane))
            return hit.point;
        return null;
    }

    List<Color> previousClr = new List<Color>();
    private bool backupColor = true;
    void ColorBuilding()
    {
        Renderer rend;
        if (child != null)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                if (rend = child.GetChild(i).GetComponent<Renderer>())
                {
                    if(backupColor)
                        previousClr.Add(rend.material.color);

                    rend.material.color = Color.Lerp(rend.material.color, placable ? new Color(0.447f, 0.957f, 0, 0.25f) : new Color(1f, 0f, 0f, 0.25f), 0.5f);
                }
            }
            backupColor = false;
        }
    }

    void RecolorBuilding()
    {
        Renderer rend;
        if (child != null)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                if (rend = child.GetChild(i).GetComponent<Renderer>())
                {
                    rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    rend.material.SetInt("_ZWrite", 1);
                    rend.material.DisableKeyword("_ALPHATEST_ON");
                    rend.material.DisableKeyword("_ALPHABLEND_ON");
                    rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    rend.material.renderQueue = -1;
                    rend.material.color = previousClr[i];
                }
            }

            previousClr.Clear();
            backupColor = true;
        }
    }
}
