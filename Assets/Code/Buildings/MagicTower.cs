using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Buildings
{
    public class MagicTower : Tower
    {
        public float InitialAOERange = 3f;
        protected override void Shoot()
        {
            ElectricityProjectile proj = Instantiate(ProjectilePrefab, currentTarget.transform.position, Quaternion.identity).GetComponent<ElectricityProjectile>();
            proj.gameObject.SetActive(true);
            proj.Damage = Damage;
            proj.Range = InitialAOERange;
        }
    }
}
