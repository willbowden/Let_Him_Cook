using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CheckPlate : MonoBehaviour
{

    [SerializeField] private OrderController orderController; // Reference to OrderController
    private List<GameObject> PlatesInArea = new();

    public Collider checkArea;
    private int totalScore = 0;
    private List<Order> orders = new List<Order>(); // List of orders
    private List<string> OrderingredientList = new();


    void Start(){
        orderController = FindObjectOfType<OrderController>();
    }

    private void Update()
    {
        // if (Input.GetButtonDown(checkBtn) && activeOrders.Count > 0)
        // {
        //     CheckOrder();
        // }
    }

    private void OnTriggerEnter(Collider other){
        
        if (other.gameObject.name != "BurgerPlate") {
            return;
        }

        PlatesInArea.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other){

        if (other.gameObject.name != "BurgerPlate") {
            return;
        }

        PlatesInArea.Remove(other.gameObject);
    }

    private void CheckOneOrder(GameObject plateObject) {
        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        Stack<GameObject> plate_ingredients = plate.GetContents();

        List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);

        orders = orderController.GetOrders();

        foreach (GameObject ingredient in plate_ingredients)
        {
            // ingredient.name == order[i]
            Debug.Log($"The burger is made up of: {ingredient.name}");
        }

    


        foreach (Order order in orders){

        ScoreOrdering(plateObject, order);
            
        }

        // Scoring burger ? 
        // What matters?
        // Accuracy of Ingredients (Are the correct ingredients used?)
        // Order of Ingredients (Are the ingredients assembled in the right sequence?)
        // Completeness (Does the burger have all the required ingredients?)
        // Extra Ingredients (Are there ingredients that should not be present?)
    }

    public List<string> getOrderRecipeIngredients(Order order){

        OrderingredientList = new List<string>(
            order.recipe.recipeText.Split('\n').Select(ingredient => ingredient.Trim())
        );
        return OrderingredientList;

    }


    public int ScoreOrdering(GameObject plateObject, Order order){

        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        List<string> IngredientsToMatch = getOrderRecipeIngredients(order);
        int score = 0;
        Stack<GameObject> plate_ingredients = plate.GetContents();
        List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);


        int i = 0; // Start from the last index of IngredientsToMatch
        foreach (GameObject ingredient in plate_ingredients)
        {

            if (ingredient.name == IngredientsToMatch[i]){
                score += 10;
                Debug.Log($"Correct match: Ingredient is {ingredient.name}");
            }

            else
            {
                Debug.Log($"Incorrect match: Ingredient is {ingredient.name}, expected {IngredientsToMatch[i]}");
            }
            i++;
            if (i > IngredientsToMatch.Count)
            {
                Debug.LogWarning("No more ingredients to match in the recipe.");
                break; // Exit the loop if we've checked all recipe ingredients
            }
        }

        Debug.Log($"Score for ordering is {score}");

        return score;

    }

    public void CheckOrders(){
        CheckOneOrder(PlatesInArea[0]);
    }



}