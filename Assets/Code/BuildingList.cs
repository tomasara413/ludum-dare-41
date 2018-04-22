using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildingList
{
    Dictionary<byte, List<GameObject>> objects = new Dictionary<byte, List<GameObject>>();
    public List<GameObject> GetListOfTeamsObjects(byte team)
    {
        List<GameObject> list;
        objects.TryGetValue(team, out list);
        return list;
    }

    public void AddGameObjectToList(byte team, GameObject o)
    {
        List<GameObject> list = GetListOfTeamsObjects(team);
        if (list == null)
        {
            list = new List<GameObject>();
            list.Add(o);
            objects.Add(team, list);
        }
        else
            list.Add(o);
    }
}
