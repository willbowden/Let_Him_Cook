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

    public int CheckOrders(){
        int score = 0;
        
        foreach (GameObject plateObject in PlatesInArea){
            score += CheckOneOrder(plateObject);
        }

        return score;
    }

    private int CheckOneOrder(GameObject plateObject) {
        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        Stack<GameObject> plate_ingredients = plate.GetContents();
        int score = 0;
        int highestScore = 0;
        List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);

        orders = orderController.GetOrders();    
        foreach (Order order in orders){
    
            score = ScoreOrdering(plateObject, order);
        
            if (score > highestScore){
                highestScore = score;
            }
        }

        return highestScore;

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
        int correctIngredients = 0;
        int weight = 100;
        Stack<GameObject> plate_ingredients = plate.GetContents();
        List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);


        int i = 0; // Start from the last index of IngredientsToMatch
        foreach (GameObject ingredient in plate_ingredients)
        {
            if (ingredient.name == IngredientsToMatch[i]){
                correctIngredients += 1;
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
                break;
            }
        }

        if (i >= IngredientsToMatch.Count)
        {
            Debug.LogWarning("There are more ingredients on the plate than in the recipe.");
            correctIngredients -= (i - IngredientsToMatch.Count);
        }

        if (i < IngredientsToMatch.Count)
        {
            Debug.LogWarning("The burger is missing ingredients.");
            correctIngredients -= (IngredientsToMatch.Count - i);
        }

        if (i == IngredientsToMatch.Count && correctIngredients == IngredientsToMatch.Count) {
            weight += 20;
        }

        float score = ((float)correctIngredients / IngredientsToMatch.Count) * weight;
        int end_score = Mathf.RoundToInt(score);
        Debug.Log($"Score based on ordering is {end_score}");
        return end_score;  // Round the score to the nearest integer
    }

    public int ScoreRegardlessOrdering(GameObject plateObject, Order order)
    {
        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        List<string> IngredientsToMatch = getOrderRecipeIngredients(order);
        int correctIngredients = 0;
        int weight = 100;

        Stack<GameObject> plate_ingredients = plate.GetContents();
        List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);

        List<string> ingredientsOnPlate = PlateIngredientsList.Select(ingredient => ingredient.name).ToList();

        foreach (string ingredient in IngredientsToMatch)
        {
            if (ingredientsOnPlate.Contains(ingredient))
            {
                correctIngredients += 1;
                ingredientsOnPlate.Remove(ingredient); 
                Debug.Log($"Correct match: Ingredient is {ingredient}");
            }
            else
            {
                Debug.Log($"Incorrect match: Missing {ingredient} on the plate");
            }
        }

        if (ingredientsOnPlate.Count >= IngredientsToMatch.Count)
        {
            Debug.LogWarning("There are more ingredients on the plate than in the recipe.");
            correctIngredients -= (ingredientsOnPlate.Count - IngredientsToMatch.Count);
        }

        if (ingredientsOnPlate.Count < IngredientsToMatch.Count)
        {
            Debug.LogWarning("The burger is missing ingredients.");
            correctIngredients -= (IngredientsToMatch.Count - ingredientsOnPlate.Count);
        }

        if (ingredientsOnPlate.Count == IngredientsToMatch.Count && correctIngredients == IngredientsToMatch.Count) {
            weight += 20;
        }

        float score = ((float)correctIngredients / IngredientsToMatch.Count) * weight;

        Debug.Log($"Score based on matching ingredients is {score}");

        return Mathf.RoundToInt(score);
    }






}