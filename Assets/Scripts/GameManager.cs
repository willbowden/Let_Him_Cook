using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    [Header("Order Controller Reference")]
    [SerializeField] private OrderController orderController; // Reference to OrderController

    private List<Recipe> recipeList; // List of Recipe ScriptableObjects
    
    
    
    // Game variables
    int score;
    public int maxConcurrentOrders = 5;
    public float gameDuration = 600f; // Total game time in seconds
    public float minTimeBetweenOrders = 5f; // Minimum interval for new orders
    public float maxTimeBetweenOrders = 15f; // Maximum interval for new orders
    private float gameTimeRemaining;
    private List<List<string>> burgerRecipes;


    // Events
    public static event Action<string, DateTime> OnOrderAdded;
    public static event Action<bool> OnOrderRemoved;
    public static event Action<int> OnScoreAdded;

    private bool isGameRunning = false;
    private List<Tuple<string, DateTime>> orders = new List<Tuple<string, DateTime>>(); // Recipe name and timestamp.

    // Unity Methods
    void Start()
    {
        orderController = FindObjectOfType<OrderController>();
        GameStart();
    }

    void Update()
    {
        if (!isGameRunning) return;

        gameTimeRemaining -= Time.deltaTime;

        if (gameTimeRemaining <= 0)
        {
            GameEnd();
        }
    }

    // Start the game
    public void GameStart()
    {
        isGameRunning = true;
        gameTimeRemaining = gameDuration;
        score = 0;
        LoadRecipes();
        orders.Clear();
        StartCoroutine(OrderTimer());
        PopulateOrders();

    }

    // End the game
    public void GameEnd()
    {
        isGameRunning = false;

        // TODO: Add end-game logic
    }

    // Timer for creating orders
    IEnumerator OrderTimer()
    {
        while (isGameRunning)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenOrders, maxTimeBetweenOrders));
            if (orders.Count < maxConcurrentOrders)
            {
                CreateRandomOrder();
            }
        }
    }

    // Add a new order
    public void AddOrder(string recipeName)
    {
        var order = new Tuple<string, DateTime>(recipeName, DateTime.Now);
        orders.Add(order);
        OnOrderAdded?.Invoke(recipeName, DateTime.Now);
    }

    // Add to score
    public void AddScore(int addedScore)
    {
        score += addedScore;
        OnScoreAdded?.Invoke(addedScore);
    }

    // Submit an order
    public void OrderSubmitted(int addedScore, bool wasSuccessful)
    {
        if (orders.Count == 0)
        {
            return;
        }

        orders.RemoveAt(0);
        // CheckPlate.Instance.CheckOrder(); // how do i use this. Make it so this returns true or false?

        if (wasSuccessful)
        {
            AddScore(addedScore);
        }

        OnOrderRemoved?.Invoke(wasSuccessful);
    }

    // Create a random order
    void CreateRandomOrder()
    {
        if (orders.Count >= maxConcurrentOrders)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, recipeList.Count);
        Recipe randomRecipe = recipeList[randomIndex];

        // Create a new order using the randomly selected recipe
        Order newOrder = new Order
        {
            name = randomRecipe.name, 
            recipe = randomRecipe, 
            timeInSeconds = (int)UnityEngine.Random.Range(5f, 10f)
        };

        orderController.AddOrder(newOrder);
        Debug.Log($"New order created: {newOrder.name}");
    }

    private void PopulateOrders()
    {

        if (orderController == null)
        {
            Debug.LogError("OrderController is null. Assign it in the Inspector in unity or find it at runtime.");
            return;
        }
        
        foreach (var recipe in recipeList)
        {
            // Create a new Order using the Recipe
            Order newOrder = new Order
            {
                name = recipe.name, 
                recipe = recipe,
                timeInSeconds = (int)UnityEngine.Random.Range(5f, 10f)
            };

            // Add the Order to the OrderController
            Debug.Log($"Created order {newOrder}");

            Debug.Log("Attempting to call AddOrder()...");

            orderController.AddOrder(newOrder);

            Debug.Log("AddOrder() was called successfully.");
        }

        Debug.Log("Orders have been populated.");
    }

    private void LoadRecipes()
    {
        Recipe[] recipes = Resources.LoadAll<Recipe>("Recipes");
        if (recipes == null || recipes.Length == 0)
        {
            Debug.LogError("No recipes found in Resources/Recipes!");
            return;
        }
        recipeList = new List<Recipe>(recipes);
        Debug.Log($"Loaded {recipeList.Count} recipes.");
}
}
