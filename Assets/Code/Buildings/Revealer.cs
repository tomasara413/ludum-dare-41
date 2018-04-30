using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Revealer : Building
    {
        Ninja n;
        void OnTriggerEnter(Collider collision)
        {
            if (Placed && (n = collision.gameObject.GetComponent<Ninja>()))
                n.Stealthed = false;
        }

        private void OnTriggerExit(Collider collision)
        {
            if (Placed && (n = collision.gameObject.GetComponent<Ninja>()))
                n.Stealthed = true;
        }
    }
}