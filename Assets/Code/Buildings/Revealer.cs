using Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class Revealer : Building
    {
        public float RevealRange = 0;
        Collider[] objectsToReveal, previouslyRevealed;
        GameObject workObject;
        TeamObject workTeamObject;
        Collider workCollider;
        protected override void BuildingPlaced()
        {
            base.BuildingPlaced();

            objectsToReveal = Physics.OverlapSphere(transform.position, RevealRange);

            for (int i = 0; i < objectsToReveal.Length; i++)
            {
                workObject = objectsToReveal[i].gameObject;
                if ((workTeamObject = workObject.GetComponent<TeamObject>()) && workTeamObject is Ninja)
                {
                    (workTeamObject as Ninja).AddRevealingSource(this);
                }
            }

            if (previouslyRevealed != null)
            {
                for (int i = 0; i < previouslyRevealed.Length; i++)
                {
                    workCollider = previouslyRevealed[i];
                    if (workCollider && !objectsToReveal.Contains(workCollider))
                    {
                        workObject = workCollider.gameObject;
                        if ((workTeamObject = workObject.GetComponent<TeamObject>()) && workTeamObject is Ninja)
                        {
                            (workTeamObject as Ninja).RemoveRevealingSource(this);
                        }
                    }
                }
            }
            previouslyRevealed = objectsToReveal;
        }
        protected override void ObjectDead()
        {
            if (previouslyRevealed != null)
            {
                for (int i = 0; i < previouslyRevealed.Length; i++)
                {
                    workCollider = previouslyRevealed[i];
                    if (workCollider)
                    {
                        workObject = workCollider.gameObject;
                        if ((workTeamObject = workObject.GetComponent<TeamObject>()) && workTeamObject is Ninja)
                        {
                            (workTeamObject as Ninja).RemoveRevealingSource(this);
                        }
                    }
                }
            }

            base.ObjectDead();
        }
    }
}