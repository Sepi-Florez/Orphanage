using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabManager : MonoBehaviour {
    Transform currentMenu;
    public GameObject[] menus;

    public void Awake() {
        menus = GameObject.FindGameObjectsWithTag("Menu");
        for (int a = 1; menus.Length > a; a++) {
            menus[a].GetComponent<CanvasGroup>().interactable = false;
            menus[a].GetComponent<CanvasGroup>().alpha = 0;
        }

        
    }
    public void ChangeMenu(Transform menu) {
        if (currentMenu != null) {
            currentMenu.GetComponent<CanvasGroup>().interactable = false;
            currentMenu.GetComponent<CanvasGroup>().alpha = 0;
        }
        currentMenu = menu;
        currentMenu.GetComponent<CanvasGroup>().interactable = true;
        currentMenu.GetComponent<CanvasGroup>().alpha = 1;

    }
}
