using Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public class Ninja : Entity
    {
        HashSet<Revealer> revealingSources = new HashSet<Revealer>();

        public bool Stealthed
        {
            get { return revealingSources.Count == 0; }
        }

        private bool previousStealthed = false;
        protected override void ObjectLiving()
        {
            base.ObjectLiving();
            if (Stealthed != previousStealthed)
            {
                if (Stealthed)
                    Hide();
                else
                    Unreaveal();

                previousStealthed = Stealthed;
            }
        }

        List<Color> previousClr = new List<Color>();
        private bool backupColor = true;
        void Hide()
        {
            Renderer rend;
            int j = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (rend = transform.GetChild(i).GetComponent<Renderer>())
                {
                    foreach (Material material in rend.materials)
                    {
                        if (backupColor)
                            previousClr.Add(material.color);
                        // Možná se tohle bude muset nastavit v původních materálech 
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.DisableKeyword("_ALPHABLEND_ON");
                        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = 3000;

                        material.color = Color.Lerp(previousClr[j], new Color(0f, 0f, 0f, 0f), 0.8f);
                        j++;
                    }
                }
            }
            backupColor = false;
        }

        void Unreaveal()
        {
            Renderer rend;
            int j = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (rend = transform.GetChild(i).GetComponent<Renderer>())
                {
                    foreach (Material material in rend.materials)
                    {
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        material.SetInt("_ZWrite", 1);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.DisableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = -1;

                        material.color = previousClr[j];
                        j++;
                    }
                }
            }

            previousClr.Clear();
            backupColor = true;
        }

        public void AddRevealingSource(Revealer r)
        {
            revealingSources.Add(r);
        }

        public void RemoveRevealingSource(Revealer r)
        {
            revealingSources.Remove(r);
        }
    }
}