using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ObjectList
{
    Dictionary<byte, List<GameObject>> allObjects = new Dictionary<byte, List<GameObject>>();
    Dictionary<byte, List<GameObject>> visionObjects = new Dictionary<byte, List<GameObject>>();
    public List<GameObject> GetListOfTeamsObjects(byte team)
    {
        List<GameObject> list;
        allObjects.TryGetValue(team, out list);
        return list;
    }

    public List<GameObject> GetListOfVisionObjects(byte team)
    {
        List<GameObject> list;
        visionObjects.TryGetValue(team, out list);
        return list;
    }

    public void AddGameObjectToList(byte team, GameObject o)
    {
        List<GameObject> list = GetListOfTeamsObjects(team);
        if (list == null)
        {
            list = new List<GameObject>();
            list.Add(o);
            allObjects.Add(team, list);
        }
        else
            list.Add(o);
    }

    public int AddVisionObjectToList(byte team, GameObject o)
    {
        int output;
        List<GameObject> list = GetListOfVisionObjects(team);
        if (list == null)
        {
            list = new List<GameObject>();
            list.Add(o);
            output = 0;
            visionObjects.Add(team, list);
        }
        else
        {
            output = list.Count;
            list.Add(o);
        }

        return output;
    }

    public bool RemoveVisionObject(byte team, int id)
    {
        if (id < 0)
            return false;
        List<GameObject> list = GetListOfVisionObjects(team);
        if (list != null)
        {
            list.RemoveAt(id);
            return true;
        }

        return false;
    }
}
