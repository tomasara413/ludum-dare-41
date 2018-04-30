using Buildings;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Buildings
{
    public class Castle : Building
    {
        bool playNinjaAnim = false;
        //Ninja n = null;
        Animator a;
        private void OnTriggerEnter(Collider other)
        {
            Entity e;
            if ((e = other.GetComponent<Entity>()) && e is Ninja)
            {
                playNinjaAnim = true;
                a = e.GetComponent<Animator>();
            }
        }

        protected override void Start()
        {
            base.Start();
            if (a != null && playNinjaAnim)
                a.SetBool(2, true);
        }
    }
}