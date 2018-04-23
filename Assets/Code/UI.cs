using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    ResourcesManager rm;
    BuildingManager bm;
    public Text[] Texts;
    public Button[] Buttons;
    public GameObject[] Panels;

    public Camera Cam;
    public RaycastHit Hit;
    private int Mask;

    void Start () {
        //vyhledá ostatní scripty
        rm = GameObject.FindGameObjectWithTag("Managers").GetComponent<ResourcesManager>();
        bm = GameObject.FindGameObjectWithTag("Managers").GetComponent<BuildingManager>();
        //přiřadí do polí jednotlivý objekty
        Texts = GetComponentsInChildren<Text>();
        Buttons = GetComponentsInChildren<Button>();
        Panels = GameObject.FindGameObjectsWithTag("Panel");
        //menu věží a budov bude ze začátku hide
        Panels[0].gameObject.SetActive(false);
        Panels[1].gameObject.SetActive(false);
        Panels[2].gameObject.SetActive(false);
        //získá layer budov
        Mask = LayerMask.GetMask("Building");
    }
	void Update () {
        //Vypisuje do UI počet surovin
        Texts[0].text = rm.GoldAmmount.ToString() + "/" + rm.GoldMax.ToString();
        Texts[1].text = rm.FoodAmmount.ToString() + "/" + rm.FoodMax.ToString();
        Texts[2].text = rm.PopulationAmmount.ToString() + "/" + rm.PopulationMax.ToString();

        //vyšle raycast a zjistí jestli jsme zasáhli kasárnu
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit, Cam.farClipPlane, Mask))
            {
                if (Hit.collider.gameObject.GetComponent<Barracks>())
                {
                    Panels[2].gameObject.SetActive(true);
                    print("Hitlo to Barracks");
                }
                else Panels[2].gameObject.SetActive(false);
            }
            else Panels[2].gameObject.SetActive(false);
        }
    }

    //přiřazeno k talčítku a zobrazí menu výněru věží
    public void TowersMenu()
    {
        Panels[1].gameObject.SetActive(true);
        Panels[0].gameObject.SetActive(false);
    }

    //přiřazeno k talčítku a zobrazí menu výněru budov
    public void BuildingsMenu()
    {
        Panels[0].gameObject.SetActive(true);
        Panels[1].gameObject.SetActive(false);
    }

    //bude přiřazeno tlačítkům jednotlivých budov a věží. Poté můžeme postavit
    public void PlaceBuilding(GameObject Building)
    {
        bm.StartPlacing(Instantiate(Building));
    }
}
