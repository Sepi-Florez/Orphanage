using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    public static CraftingManager thisManager;

    public GameObject recipePref;
    public List<RecipeButton> recipes = new List<RecipeButton>();
    public void Awake() {
        thisManager = this;
    }

    public void Start() {
        AddRecipe(1);
    }
    // adds a recipe to the list
    public void AddRecipe(int itemID) {
        Transform newRecipe = Instantiate(recipePref, Vector3.zero, Quaternion.identity).transform;
        newRecipe.SetParent(GameObject.FindGameObjectWithTag("CraftingContent").transform);
        InventoryManager.thisManager.helpArrange(newRecipe);
        recipes.Add(newRecipe.GetComponent<RecipeButton>());
        recipes[0].FillValues(DataBaseManager.thisManager.GetItem(itemID));
    }
    // Refreshes all recipes values
    public void RefreshRecipes() {
        foreach(RecipeButton r in recipes) {
            r.UpdateValues();
        }
    }
    // Tries to make the given item
    public void MakeRecipe(int itemID) {
        print("Making Recipe");
        Item newItem = DataBaseManager.thisManager.GetItem(itemID);
        List<Item> recipe = new List<Item>();
        List<int> recipeAmount = new List<int>();
        int a = 0;
        foreach (int comp in newItem.comps) {
            recipe.Add(DataBaseManager.thisManager.GetItem(comp));
            recipeAmount.Add(newItem.compsc[a]);
            a++;
        }
        a = 0;
        if(InventoryManager.thisManager.RecipeCheck(recipe, recipeAmount)) {
            
            foreach(Item comp in recipe) {
                InventoryManager.thisManager.Remove(comp, newItem.compsc[a]);
                a++;
            }
            InventoryManager.thisManager.InventoryAdd(newItem.ID,1);
            print("Recipe is go");
        }
    }
}
