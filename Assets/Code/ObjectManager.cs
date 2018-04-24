using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    ObjectList list = new ObjectList();

    public ObjectList GetTeamObjectList()
    {
        return list;
    }
}
