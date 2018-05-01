using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    //Only a temporary class, this will be rewritten into a different form
    public class FriendlyUnit : Entity
    {
        protected override void ObjectDead()
        {
            rm.SoldierAmount--;

            base.ObjectDead();
        }
    }
}
