using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Order Controller Reference")]
    [SerializeField] private OrderController orderController; // Reference to OrderController
    [SerializeField] private CheckPlate CheckPlateController;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private EndController endController;
    [SerializeField] private List<Order> orders = new List<Order>(); // List of orders

    private Announcer announcer;

    private List<Recipe> recipeList;

    // Game variables
    public int maxConcurrentOrders = 5;
    public float gameDuration = 600f;
    public float minTimeBetweenOrders = 30f;
    public float maxTimeBetweenOrders = 60f;
    public float orderMinDuration = 30f;
    public float orderMaxDuration = 120f;
    public float kitchenPrepDuration = 30f;
    private float gameTimeRemaining;

    // Events
    public static event Action<string, DateTime> OnOrderAdded;
    public static event Action<bool> OnOrderRemoved;
    public static event Action<int> OnScoreAdded;

    private bool isGameRunning = false;
    // private int score = 0;

    public int totalScore = 0;

    // Unity Methods
    void Start()
    {
        orderController = FindObjectOfType<OrderController>();
        CheckPlateController = FindObjectOfType<CheckPlate>();
        scoreController = FindObjectOfType<ScoreController>();
        endController = FindObjectOfType<EndController>();
        announcer = FindObjectOfType<Announcer>();

        announcer.QueueVoiceLine("GameStart");
    }

    void Update()
    {
        while (kitchenPrepDuration > 0)
        {
            kitchenPrepDuration -= Time.deltaTime;
            return;
        }

        if (!isGameRunning)
        {
            GameStart();
        }

        if (gameTimeRemaining / gameDuration > 0.25f && (gameTimeRemaining - Time.deltaTime) / gameDuration <= 0.25f)
        {
            announcer.QueueVoiceLine("LowTimeRemaining");
        }

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
        LoadRecipes();
        orders.Clear();
        CreateRandomOrder();
        StartCoroutine(OrderTimer());
    }

    // End the game
    public void GameEnd()
    {
        isGameRunning = false;

        if (totalScore > 500)
        {
            announcer.QueueVoiceLine("GameOverHighScore");
        }
        else if (totalScore <= 0)
        {
            announcer.QueueVoiceLine("GameOverNegativeScore");
        }
        else if (totalScore < 500)
        {
            announcer.QueueVoiceLine("GameOverLowScore");
        }

        // TODO: Add end-game logic
        endController.Appear();
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

    // Submit an order
    public void OrderSubmitted()
    {
        // if (orders.Count == 0)
        // {
        //     return;
        // }

        // Need to be able to remove one order
        // orders.RemoveAt(0);
        Debug.Log("I was called succesfully");
        int score = CheckPlateController.CheckOrders();
        Debug.Log($"From Gamemanager trying to add score {score} ");
        scoreController.UpdateScore(score);
        totalScore += score;
        // OnOrderRemoved?.Invoke(wasSuccessful); JUNK?
    }


    // Create a random order
    void CreateRandomOrder()
    {

        if (!isGameRunning)
        {
            return;
        }
        List<Order> orders = orderController.GetOrders();

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
            timeInSeconds = (int)UnityEngine.Random.Range(orderMinDuration, orderMaxDuration)
        };

        orderController.AddOrder(newOrder);
        Debug.Log($"New order created: {newOrder.name}");
    }

    // This is not gonna be used
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
                timeInSeconds = (int)UnityEngine.Random.Range(orderMinDuration, orderMaxDuration)
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


    // Add to score
    // public void AddScore(int addedScore)
    // {
    //     score += addedScore;
    //     OnScoreAdded?.Invoke(addedScore);
    // }
}
