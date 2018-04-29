﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using Buildings;

public class UI : MonoBehaviour {

    private ResourcesManager rm;
    private BuildingManager bm;
    public EventSystem es;
    public OnUi onui;

    public Text ErrorText;
    public Text[] Texts;
    public GameObject[] Panels;



    
    public GameObject BarrackOB = null, ErrorTextOBJ;
    public bool ClickOnBarracks = false;
    public Camera Cam;
    public RaycastHit Hit;
    private int Mask;

    public bool GameIsPaused = false;
    void Start () {
        GameObject managers = GameObject.FindGameObjectWithTag("Managers");
        bm = managers.GetComponent<BuildingManager>();
        rm = managers.GetComponent<ResourcesManager>();
        es = transform.GetChild(9).GetComponent<EventSystem>();
        //menu budou ze začátku hide
        Panels[0].gameObject.SetActive(false);
        Panels[1].gameObject.SetActive(false);
        Panels[2].gameObject.SetActive(false);
        Panels[3].gameObject.SetActive(false);

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
            if (onui.OnUI == false)
            {
                ClickOnBarracks = false;
                BarrackOB = null;
            }

            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit, Cam.farClipPlane, Mask))
            {
                if (Hit.collider.gameObject.GetComponent<Barracks>())
                {
                    ClickOnBarracks = true;
                    BarrackOB = Hit.collider.gameObject;
                }
                
            } 
            if(ClickOnBarracks == true)
            {
                Panels[1].gameObject.SetActive(true);
            }
            else Panels[1].gameObject.SetActive(false);
        }
        //Pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //přiřazeno k talčítku a zobrazí menu výněru věží
    public void TowersMenu()
    {
        Panels[0].gameObject.SetActive(true);
        Panels[2].gameObject.SetActive(false);
    }

    //přiřazeno k talčítku a zobrazí menu výněru budov
    public void BuildingsMenu()
    {
        Panels[2].gameObject.SetActive(true);
        Panels[0].gameObject.SetActive(false);
    }

    //bude přiřazeno tlačítkům jednotlivých budov a věží. Poté můžeme postavit
    public void PlaceBuilding(GameObject Building)
    {
        if (rm.GoldAmmount > Building.GetComponent<Building>().Gold)
        {
            bm.StartPlacing(Instantiate(Building));
        }
        else
        {
            ErrorText.text = "You haven't enought money!";
            StartCoroutine(TextShow());
        }
    }

    public void Recruit(GameObject Unit)
    {
        if (rm.PopulationAmmount < rm.PopulationMax)
        {
            if (rm.FoodAmmount > 0)
            {
                if(rm.GoldAmmount > 10)
                {
                    BarrackOB.GetComponent<Barracks>().Recruit(Unit);
                }
                else
                {
                    ErrorText.text = "You haven't enought money!";
                    StartCoroutine(TextShow());
                }
             
            }
            else
            {
                ErrorText.text = "You haven't enought food!";
                StartCoroutine(TextShow());
            }
        }
        else
        {
            ErrorText.text = "You need more space for units!";
            StartCoroutine(TextShow());
        }
    }

    IEnumerator TextShow ()
    {
        ErrorTextOBJ.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorTextOBJ.SetActive(false);
    }

    public void Resume()
    {
        Panels[3].gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Panels[3].gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
