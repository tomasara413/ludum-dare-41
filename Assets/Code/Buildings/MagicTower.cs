using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Buildings
{
    public class MagicTower : Tower
    {
        public ElectricityProjectile projectile;
        public float InitialAOERange = 3f;
        protected override void Shoot()
        {
            ElectricityProjectile proj = Instantiate(projectile, currentTarget.transform.position, Quaternion.identity);
            proj.Damage = Damage;
            proj.Range = InitialAOERange;
        }
    }
}
