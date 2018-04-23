using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlaceabilityDetector : MonoBehaviour
{
    BuildingManager bm;
    Collider c;
    int counter = 0;
    GameObject importantPoints;
    Terrain t;

    private void Start()
    {
        bm = GameObject.FindGameObjectWithTag("Managers").GetComponent<BuildingManager>();
        c = GetComponent<Collider>();
        c.isTrigger = true;
        foreach (Transform t in transform)
        {
            if (t.name == "ImportantPoints")
            {
                importantPoints = t.gameObject;
                break;
            }
        }
        t = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Terrain>();
    }


    RaycastHit rh;
    float? min = null, max = null;
    private void Update()
    {
        if (importantPoints != null && t != null)
        {
            foreach (Transform point in importantPoints.transform)
            {
                if (point.position.y != t.SampleHeight(point.position))
                {
                    if (Physics.Raycast(point.position, Vector3.down, out rh, Mathf.Infinity, bm.TerrainMask) || Physics.Raycast(point.position, Vector3.up, out rh, Mathf.Infinity, bm.TerrainMask))
                    {
                        if (min == null)
                        {
                            min = max = rh.point.y;
                            continue;
                        }

                        //Debug.Log(rh.collider.name);
                        if (rh.point.y < min)
                            min = rh.point.y;
                        else if (rh.point.y > max)
                            max = rh.point.y;
                    }
                    else
                    {
                        bm.Placeable = false;
                        break;
                    }
                }
                else
                {
                    if (point.position.x >= t.transform.position.x && point.position.z >= t.transform.position.z && point.position.x <= t.transform.position.x + t.terrainData.size.x && point.position.z <= t.transform.position.z + t.terrainData.size.z)
                    {
                        if (min == null)
                            min = max = point.position.y;
                    }
                    else
                    {
                        bm.Placeable = false;
                        break;
                    }
                }
            }

            if (min != null)
            {
                if ((float)max - (float)min > bm.MaxDeltaY)
                    bm.Placeable = false;
                else
                {
                    //Debug.Log(counter);
                    if (counter <= 0)
                        bm.Placeable = true;
                    else
                        bm.Placeable = false;
                }
                min = max = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (!other.gameObject.GetComponent<Terrain>())
            counter++;
    }
    private void OnTriggerExit(Collider other)
    {   
        if (!other.gameObject.GetComponent<Terrain>())
            counter--;
    }
}

