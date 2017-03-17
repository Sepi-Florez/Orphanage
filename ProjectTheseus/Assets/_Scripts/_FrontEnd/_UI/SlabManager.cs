using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabManager : MonoBehaviour {
    public static SlabManager thisManager;
    

    Transform currentMenu;
    public GameObject[] menus;

    bool input;

    void Awake() {
        thisManager = this;
        menus = GameObject.FindGameObjectsWithTag("Menu");
        currentMenu = menus[0].transform;
        for (int a = 1; menus.Length > a; a++) {
            menus[a].GetComponent<CanvasGroup>().interactable = false;
            menus[a].GetComponent<CanvasGroup>().alpha = 0;
            menus[a].GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        
    }
    void Update() {
        if (input) {
            _Input();
        }
    }
    //Activated by a button to switch between menus
    public void ChangeMenu(Transform menu) {
        if (currentMenu != null) {
            currentMenu.GetComponent<CanvasGroup>().interactable = false;
            currentMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
            currentMenu.GetComponent<CanvasGroup>().alpha = 0;
        }
        currentMenu = menu;
        currentMenu.GetComponent<CanvasGroup>().interactable = true;
        currentMenu.GetComponent<CanvasGroup>().alpha = 1;
        currentMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
    public void SlabToggle() {

    }
    private void _Input() {
        if (Input.GetButtonDown("Use")) {
            InventoryManager.thisManager.Use();
        }
        //if(Input.GetButtonDown("Fire1")) {
        //    InventoryManager.thisManager.Equip(0);
        //}
        //if (Input.GetButtonDown("Fire2")) {
        //    InventoryManager.thisManager.Equip(1);
        //}
    }
}
