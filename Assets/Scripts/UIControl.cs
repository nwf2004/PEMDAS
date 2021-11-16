using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{

    public GameObject Menu;
    public GameObject GunSpotUI1;
    public GameObject GunSpotUI2;
    public GameObject GunSpotUI3;
    public bool menuOpen = false;
    public Button Gun1;
    public Button Gun2;
    public Button Gun3;
    public int activeGun = 1;


    // Start is called before the first frame update
    void Start()
    {
        activeGun = 1;
         menuOpen = false;
        Menu.SetActive(false);
        Button button1 = Gun1.GetComponent<Button>();
        Button button2 = Gun2.GetComponent<Button>();
        Button button3 = Gun3.GetComponent<Button>();
        button1.onClick.AddListener(TaskOnClick1);
        button2.onClick.AddListener(TaskOnClick2);
        button3.onClick.AddListener(TaskOnClick3);
    }

    // Update is called once per frame
    void Update()
    {
        checkKeys();
        checkCurrentGun();
        
    }

    void checkCurrentGun() {
        if (activeGun == 1) {
            GunSpotUI1.SetActive(true);
            GunSpotUI2.SetActive(false);
            GunSpotUI3.SetActive(false);
        }
        if (activeGun == 2)
        {
            GunSpotUI1.SetActive(false);
            GunSpotUI2.SetActive(true);
            GunSpotUI3.SetActive(false);

        }
        if (activeGun == 3)
        {
            GunSpotUI1.SetActive(false);
            GunSpotUI2.SetActive(false); 
            GunSpotUI3.SetActive(true);
        }
    }
    void checkKeys() {
        if (Input.GetKeyDown(KeyCode.Tab) && menuOpen == false)
        {
            GameObject.Find("Player").GetComponent<PlayerControl>().canMove = false;
            GameObject.Find("Player").GetComponent<PlayerControl>().canLook = false;
            menuOpen = true;
            Menu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && menuOpen == true)
        {
            GameObject.Find("Player").GetComponent<PlayerControl>().canMove = true;
            GameObject.Find("Player").GetComponent<PlayerControl>().canLook = true;
            menuOpen = false;
            Menu.SetActive(false);
        }
    }
    void TaskOnClick1()
    {
       //GunBTN1.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
        activeGun = 1;    
    }
    void TaskOnClick2()
    {
        //
        activeGun = 2;
    }
    void TaskOnClick3()
    {
        
        activeGun = 3;
    }
}
