using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    public GameObject recipePref;
    public void Start() {
        AddRecipe(1);
    }
    public void AddRecipe(int ItemID) {
        Transform newRecipe = Instantiate(recipePref, Vector3.zero, Quaternion.identity).transform;
        newRecipe.SetParent(GameObject.FindGameObjectWithTag("CraftingContent").transform);
        InventoryManager.thisManager.helpArrange(newRecipe);
        newRecipe.GetComponent<RecipeButton>().FillValues(DataBaseManager.thisManager.GetItem(ItemID));
    }
}
