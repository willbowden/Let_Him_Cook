using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
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
        orders.Clear();

        StartCoroutine(OrderTimer());
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
                CreateOrder();
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
    void CreateOrder()
    {
        if (orders.Count >= maxConcurrentOrders)
        {
            return;
        }

        string recipeName = GenerateRandomRecipe();
        AddOrder(recipeName);
    }

    // Generate a random recipe
    private string GenerateRandomRecipe()
    {
        string[] recipes = { "Cheeseburger", "Veggie Burger", "Chicken Burger", "Double Cheeseburger", "Bacon Burger" };
        return recipes[UnityEngine.Random.Range(0, recipes.Length)];
    }

    private void burgerstuff()
    {
        // Burger stuff
        burgerRecipes = new List<List<string>>();
        List<string> cheeseburger = new List<string> { "Bun", "Beef Patty", "Cheese", "Lettuce", "Tomato", "Bun" };
        List<string> triplecheese = new List<string> { "Bun", "Beef Patty", "Cheese", "Cheese", "Cheese", "Tomato","Pickles", "Bun" };
        List<string> spicyburger = new List<string> { "Bun", "Beef Patty", "Lettuce", "Red Pepper", "Onion", "Bun"};
        List<string> avocadoburger = new List<string> { "Bun", "Beef Patty", "Lettuce", "Avocado", "Tomato", "Onion", "Bun"};
        List<string> chesseburger_pickles = new List<string> { "Bun", "Beef Patty", "Cheese", "Lettuce", "Tomato", "Pickles", "Bun" };
        List<string> abomination = new List<string> { "Bun", "Beef Patty", "Lettuce", "Avocado", "Tomato", "Onion", "Cheese", "Red Pepper", "Pickle", "Bun"};
        List<string> sigma = new List<string> { "Bun", "Pickle", "Pickle", "Pickle", "Pickle", "Pickle", "Bun"};

        burgerRecipes.Add(cheeseburger);
        burgerRecipes.Add(triplecheese);
        burgerRecipes.Add(spicyburger);
        burgerRecipes.Add(avocadoburger);
        burgerRecipes.Add(chesseburger_pickles);
        burgerRecipes.Add(abomination);
        burgerRecipes.Add(sigma);
    }
}
