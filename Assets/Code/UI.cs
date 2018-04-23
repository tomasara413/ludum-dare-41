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
    //public GameObject[] Buildings;


    void Start () {
        rm = GameObject.FindGameObjectWithTag("Managers").GetComponent<ResourcesManager>();
        bm = GameObject.FindGameObjectWithTag("Managers").GetComponent<BuildingManager>();
        Texts = GetComponentsInChildren<Text>();
        Buttons = GetComponentsInChildren<Button>();
        Panels = GameObject.FindGameObjectsWithTag("Panel");
        Panels[0].gameObject.SetActive(false);
        Panels[1].gameObject.SetActive(false);
    }
	void Update () {
        //Texts[0].text = rm.MoneyAmount.ToString() + "/" + rm.MaxMoney.ToString();
        Texts[1].text = rm.FoodAmount.ToString() + "/" + rm.FoodMax.ToString();
        //Texts[2].text = rm.PopulationAmount.ToString() + "/" + rm.PopulationMax.ToString();
    }
    public void TowersMenu()
    {
        Panels[0].gameObject.SetActive(true);
        Panels[1].gameObject.SetActive(false);
    }
    public void BuildingsMenu()
    {
        Panels[1].gameObject.SetActive(true);
        Panels[0].gameObject.SetActive(false);
    }
    public void PlaceBuilding(GameObject Building)
    {
        bm.StartPlacing(Instantiate(Building));
    }
}
