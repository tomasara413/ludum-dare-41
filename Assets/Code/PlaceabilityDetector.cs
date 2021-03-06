﻿using Buildings;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlaceabilityDetector : MonoBehaviour
{
    BuildingManager bm;
    ResourcesManager rm;

    BoxCollider buildingCollider;

    int counter = 0;
    GameObject importantPoints;
    Terrain t;
    Building b;

    private void Start()
    {
        GameObject managers = GameObject.FindGameObjectWithTag("Managers");
        bm = managers.GetComponent<BuildingManager>();
        rm = managers.GetComponent<ResourcesManager>();
        buildingCollider = GetComponent<BoxCollider>();
        b = GetComponent<Building>();
        buildingCollider.isTrigger = true;
        counter = 0;
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
                    if (Physics.Raycast(point.position, Vector3.down, out rh, bm.MaxDeltaY, bm.TerrainMask) || Physics.Raycast(point.position, Vector3.up, out rh, bm.MaxDeltaY, bm.TerrainMask))
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
                        //Debug.Log("here 1");
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
                        //Debug.Log("here 2");
                        bm.Placeable = false;
                        break;
                    }
                }
            }

            if (min != null)
            {
                if ((float)max - (float)min > bm.MaxDeltaY)
                {
                    bm.Placeable = false;
                    //Debug.Log("here 3");
                }
                else
                {
                    //Debug.Log(counter);
                    if (counter <= 0)
                    {
                        if (b.Gold <= rm.GoldAmmount)
                            bm.Placeable = true;
                        else
                            bm.Placeable = false;
                        //Debug.Log("Placeable: " + bm.Placeable);
                    }
                    else
                    {
                        bm.Placeable = false;
                        //Debug.Log("here 4");
                    }
                }
                min = max = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (!other.gameObject.GetComponent<Terrain>() && other is BoxCollider)
        {
            counter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {   
        if (!other.gameObject.GetComponent<Terrain>() && other is BoxCollider)
        {
            counter--;
        }
    }

    public void OnDestroy()
    {
        if (buildingCollider)
            buildingCollider.isTrigger = false;
    }
}