// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// public class CheckPlate : MonoBehaviour
// {

//     [SerializeField] private OrderController orderController; // Reference to OrderController
//     public List<GameObject> PlatesInArea = new ();

//     public Collider checkArea;
//     private int totalScore = 0;
//     private List<Order> orders = new List<Order>(); // List of orders
//     private List<string> OrderingredientList = new();


//     void Start(){
//         orderController = FindObjectOfType<OrderController>();
//     }

//     private void OnTriggerEnter(Collider other){
        
//         // Debug.Log(other.gameObject.name.Contains("BurgerPlate"));

//         if (!other.gameObject.name.Contains("BurgerPlate")) {
//             return;
//         }

//         PlatesInArea.Add(other.gameObject);
//     }

//     private void OnTriggerExit(Collider other){

//         // Debug.Log(other.gameObject.name);

//         if (other.gameObject.name.Contains("BurgerPlate")) {
//             return;
//         }

//         PlatesInArea.Remove(other.gameObject);
//     }

//     public int CheckOrders(){
//         int score = 0;
//         Debug.Log("Check orders was called");
                
//         foreach (GameObject plateObject in PlatesInArea){
//             score += CheckOneOrder(plateObject);

//             DestroyPlate(plateObject);
//         }
//         Debug.Log($"From Checkorders Trying to add score {score}");

//         return score;
//     }

//     public void DestroyPlate(GameObject plateObject)
//     {
//         foreach (Transform child in plateObject.transform)
//         {
//             Destroy(child.gameObject);
//         }
//         // Destroy(plateObject);

//     }

//     private int CheckOneOrder(GameObject plateObject) {
//         if (plateObject == null) {
//             return 0;
//         }
//         BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
//         Stack<GameObject> plate_ingredients = plate.GetContents();
//         int score = 0;
//         int highestScore = -10000;
//         Order orderToRemove = new();
//         List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);

//         orders = orderController.GetOrders();    
//         if (orders.Count == 0){
//                 return 0;
//         }
//         // Need to get the order which gave the highest score and remove it
//         foreach (Order order in orders){

//             score = ScoreOrdering(plateObject, order);
        
//             if (score > highestScore){
//                 highestScore = score;
//                 orderToRemove = order;
//             }
//         }
//         orderController.RemoveOrder(orderToRemove);
//         Debug.Log($"From CheckOneOrder trying to add score {highestScore}");
//         return highestScore;

//     }

//     public List<string> getOrderRecipeIngredients(Order order){

//         OrderingredientList = new List<string>(
//             order.recipe.recipeText.Split('\n').Select(ingredient => ingredient.Trim())
//         );
//         return OrderingredientList;

//     }


//     public int ScoreOrdering(GameObject plateObject, Order order){

//         BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
//         List<string> IngredientsToMatch = getOrderRecipeIngredients(order);
//         int correctIngredients = 0;
//         int weight = 100;
//         Stack<GameObject> plate_ingredients = plate.GetContents();
//         List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);


//         int i = 0; // Start from the last index of IngredientsToMatch
//         foreach (GameObject ingredient in plate_ingredients)
//         {
//             if (ingredient.name.Contains(IngredientsToMatch[i])){
//                 correctIngredients += 1;
//                 Debug.Log($"Correct match: Ingredient is {ingredient.name}");
//             }

//             else
//             {
//                 Debug.Log($"Incorrect match: Ingredient is {ingredient.name}, expected {IngredientsToMatch[i]}");
//             }
//             i++;
//             if (i > IngredientsToMatch.Count)
//             {
//                 Debug.LogWarning("No more ingredients to match in the recipe.");
//                 break;
//             }
//         }

//         if (i >= IngredientsToMatch.Count)
//         {
//             Debug.LogWarning("There are more ingredients on the plate than in the recipe.");
//             correctIngredients -= (i - IngredientsToMatch.Count);
//         }

//         if (i < IngredientsToMatch.Count)
//         {
//             Debug.LogWarning("The burger is missing ingredients.");
//             correctIngredients -= (IngredientsToMatch.Count - i);
//         }

//         if (i == IngredientsToMatch.Count && correctIngredients == IngredientsToMatch.Count) {
//             weight += 20;
//         }

//         if (order.timeInSeconds == 0) {
//             weight -= 30;
//         }

//         float score = ((float)correctIngredients / IngredientsToMatch.Count) * weight;
//         int end_score = Mathf.RoundToInt(score);
//         Debug.Log($"Score based on ordering is {end_score}");
//         return end_score;  // Round the score to the nearest integer
//     }

//     public int ScoreRegardlessOrdering(GameObject plateObject, Order order)
//     {
//         BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
//         List<string> IngredientsToMatch = getOrderRecipeIngredients(order);
//         int correctIngredients = 0;
//         int weight = 100;

//         Stack<GameObject> plate_ingredients = plate.GetContents();
//         List<GameObject> PlateIngredientsList = new List<GameObject>(plate_ingredients);

//         List<string> ingredientsOnPlate = PlateIngredientsList.Select(ingredient => ingredient.name).ToList();

//         foreach (string ingredient in IngredientsToMatch)
//         {
//             if (ingredientsOnPlate.Contains(ingredient))
//             {
//                 correctIngredients += 1;
//                 ingredientsOnPlate.Remove(ingredient); 
//                 Debug.Log($"Correct match: Ingredient is {ingredient}");
//             }
//             else
//             {
//                 Debug.Log($"Incorrect match: Missing {ingredient} on the plate");
//             }
//         }

//         if (ingredientsOnPlate.Count >= IngredientsToMatch.Count)
//         {
//             Debug.LogWarning("There are more ingredients on the plate than in the recipe.");
//             correctIngredients -= (ingredientsOnPlate.Count - IngredientsToMatch.Count);
//         }

//         if (ingredientsOnPlate.Count < IngredientsToMatch.Count)
//         {
//             Debug.LogWarning("The burger is missing ingredients.");
//             correctIngredients -= (IngredientsToMatch.Count - ingredientsOnPlate.Count);
//         }

//         if (ingredientsOnPlate.Count == IngredientsToMatch.Count && correctIngredients == IngredientsToMatch.Count) {
//             weight += 20;
//         }

//         float score = ((float)correctIngredients / IngredientsToMatch.Count) * weight;

//         Debug.Log($"Score based on matching ingredients is {score}");

//         return Mathf.RoundToInt(score);
//     }






// }
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPlate : MonoBehaviour
{
    [SerializeField] private OrderController orderController; // Reference to OrderController
    public List<GameObject> PlatesInArea = new(); // List to store plates in the trigger area
    public Collider checkArea;
    private int totalScore = 0;
    private List<Order> orders = new(); // List of orders
    private List<string> OrderIngredientList = new();

    void Start()
    {
        orderController = FindObjectOfType<OrderController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}");

        // Check if the object is a plate by verifying the BurgerPlate component
        BurgerPlate plate = other.GetComponent<BurgerPlate>();
        if (plate != null)
        {
            if (!PlatesInArea.Contains(other.gameObject))
            {
                PlatesInArea.Add(other.gameObject);
                Debug.Log($"Plate added: {other.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited by: {other.name}");

        // Check if the object is a plate by verifying the BurgerPlate component
        BurgerPlate plate = other.GetComponent<BurgerPlate>();
        if (plate != null)
        {
            PlatesInArea.Remove(other.gameObject);
            Debug.Log($"Plate removed: {other.name}");
        }
    }

    public int CheckOrders()
    {
        int score = 0;
        Debug.Log("CheckOrders called");

        // Use a copy of PlatesInArea to safely modify it during iteration
        foreach (GameObject plateObject in PlatesInArea.ToList())
        {
            if (plateObject == null)
            {
                Debug.LogWarning("PlateObject is null, skipping...");
                continue;
            }

            score += CheckOneOrder(plateObject);
            DestroyPlate(plateObject); // Destroy the plate after processing
            PlatesInArea.Remove(plateObject); // Remove it from the list
        }

        Debug.Log($"Total score after checking orders: {score}");
        return score;
    }

    public void DestroyPlate(GameObject plateObject)
    {
        if (plateObject != null)
        {
            // Loop through children in reverse order
            for (int i = plateObject.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = plateObject.transform.GetChild(i);
                Destroy(child.gameObject); // Destroy the child object
            }
            Destroy(plateObject); // Destroy the plate itself
            Debug.Log($"Plate destroyed in reverse order: {plateObject.name}");
        }
    }


    private int CheckOneOrder(GameObject plateObject)
    {
        if (plateObject == null) return 0;

        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        if (plate == null) return 0;

        Stack<GameObject> plateIngredients = plate.GetContents();
        if (plateIngredients.Count == 0) return 0;
        int score = 0;
        int highestScore = -10000;
        Order orderToRemove = new();
        List<GameObject> PlateIngredientsList = new(plateIngredients);


        orders = orderController.GetOrders();
        if (orders.Count == 0)
        {
            Debug.LogWarning("No orders available to check.");
            return 0;
        }
        

        // Find the order with the highest score and remove it
        foreach (Order order in orders)
        {
            score = ScoreOrdering(plateObject, order);

            if (score > highestScore)
            {
                highestScore = score;
                orderToRemove = order;
            }
        }

        orderController.RemoveOrder(orderToRemove);
        Debug.Log($"Order removed: {orderToRemove.name}, Score: {highestScore}");
        return highestScore;
    }

    public List<string> GetOrderRecipeIngredients(Order order)
    {
        OrderIngredientList = new List<string>(
            order.recipe.recipeText.Split('\n').Select(ingredient => ingredient.Trim())
        );
        return OrderIngredientList;
    }

    public int ScoreOrdering(GameObject plateObject, Order order)
    {
        BurgerPlate plate = plateObject.GetComponent<BurgerPlate>();
        List<string> IngredientsToMatch = GetOrderRecipeIngredients(order);
        int correctIngredients = 0;
        int weight = 100;
        Stack<GameObject> plateIngredients = plate.GetContents();
        List<GameObject> PlateIngredientsList = new(plateIngredients);

        int i = 0; // Index for ingredients in the order
        foreach (GameObject ingredient in plateIngredients)
        {
            if (i < IngredientsToMatch.Count && ingredient.name.Contains(IngredientsToMatch[i]))
            {
                correctIngredients++;
                Debug.Log($"Correct match: {ingredient.name}");
            }
            else
            {
                Debug.Log($"Incorrect match: {ingredient.name}");
            }
            i++;
        }

        if (i > IngredientsToMatch.Count)
        {
            Debug.LogWarning("More ingredients on the plate than in the recipe.");
            correctIngredients -= (i - IngredientsToMatch.Count);
        }

        if (i < IngredientsToMatch.Count)
        {
            Debug.LogWarning("Missing ingredients on the plate.");
            correctIngredients -= (IngredientsToMatch.Count - i);
        }

        bool hasBurgerBurnt = PlateIngredientsList.Any(ingredient => ingredient.name == "BurgerBurnt");
        bool hasBurgerRaw = PlateIngredientsList.Any(ingredient => ingredient.name == "BurgerRaw");
        
        
        if (hasBurgerBurnt) weight -= 20;
        if (hasBurgerRaw) weight -= 20;

        if (correctIngredients == IngredientsToMatch.Count) weight += 20; // Bonus for perfect match
        if (order.timeInSeconds <= 0) weight -= 30; // Penalty for late order

        float score = ((float)correctIngredients / IngredientsToMatch.Count) * weight;
        int finalScore = Mathf.RoundToInt(score);
        Debug.Log($"Score: {finalScore}");
        return finalScore;
    }
}
