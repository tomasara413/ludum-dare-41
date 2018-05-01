using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Building : TeamObject
    {
        public bool Placed = false;

        protected BuildingManager bm;
        private SphereCollider rangeCollider;

        protected override void Start()
        {
            base.Start();
            bm = managers.GetComponent<BuildingManager>();
            
            if (Placed)
                om.GetTeamObjectList().AddGameObjectToList(team, gameObject);
            else
            {
                rangeCollider = GetComponent<SphereCollider>();
                if (rangeCollider)
                    rangeCollider.enabled = false;
            }
        }

        protected override void ObjectLiving()
        {
            if (Placed)
            {
                base.ObjectLiving();
                BuildingPlaced();
            }
        }

        protected virtual void BuildingPlaced()
        {
            if (rangeCollider)
                rangeCollider.enabled = true;
        }
    }
}