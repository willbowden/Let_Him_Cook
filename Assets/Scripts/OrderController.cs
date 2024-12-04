using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderController : MonoBehaviour
{
    [Header("List of Orders")]
    [SerializeField] private List<Order> orders = new List<Order>(); // List of orders
    private int orderIndex = 0; // Current order index
    private bool isRestaurantOpen = false; // Tracks if the restaurant is open
    private bool isHandlingOrder = false; // Tracks if there is an active order

    [Header("UI Elements")]
    [SerializeField] private Image imageDisplay;  // Reference to the UI Image component
    [SerializeField] private TMP_Text timerDisplay; // Reference to the TMP_Text for the timer

    private float currentTime; // Current remaining time for the order

    void Start()
    {
        // Restaurant starts closed, no orders loaded initially
        ResetUI();
        Open();

        
    }

    void Update()
    {
        if (!isRestaurantOpen) return; // Skip update if the restaurant is closed

        if (isHandlingOrder && currentTime > 0)
        {
            // Countdown the timer for the current order
            currentTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerDisplay.text = $"{minutes}:{seconds:D2}";
        }
        else if (isHandlingOrder && currentTime <= 0)
        {
            // Timer for the current order has run out
            CompleteCurrentOrder();
        }
        else if (orders.Count == 0)
        {
            // No orders left but the restaurant is open
            ResetUI();
        }
    }

    // Open the restaurant
    public void Open()
    {
        isRestaurantOpen = true;
        Debug.Log("Restaurant is now open!");
        if (orders.Count > 0)
        {
            LoadOrder();
        }
    }

    // Close the restaurant
    public void Close()
    {
        isRestaurantOpen = false;
        isHandlingOrder = false;
        Debug.Log("Restaurant is now closed!");
        ResetUI();
    }

    // Load the current order details into the UI
    private void LoadOrder()
    {
        if (orders.Count > 0 && orderIndex >= 0 && orderIndex < orders.Count)
        {
            isHandlingOrder = true;
            Order currentOrder = orders[orderIndex];
            imageDisplay.sprite = currentOrder.recipe.recipeImage;
            currentTime = currentOrder.timeInSeconds;
        }
        else
        {
            Debug.LogError("Failed to load order: Invalid index or no orders available.");
            ResetUI();
        }
    }

    // Complete the current order and move to the next
    private void CompleteCurrentOrder()
    {
        Debug.Log($"Order completed: {orders[orderIndex].name}");
        orders.RemoveAt(orderIndex); // Remove the completed order

        if (orders.Count > 0)
        {
            // Load the next order if available
            LoadOrder();
        }
        else
        {
            // No orders left to handle
            isHandlingOrder = false;
            Debug.Log("No more orders left.");
        }
    }

    // Add a new order to the list
    public void AddOrder(Order order)
    {
        orders.Add(order);
        Debug.Log($"Order added: {order.name}");
        if (!isHandlingOrder && isRestaurantOpen)
        {
            LoadOrder(); // Start handling the new order if none is active
        }
    }

    // Remove all orders and reset
    public void RemoveAllOrders()
    {
        orders.Clear();
        orderIndex = 0;
        isHandlingOrder = false;
        Debug.Log("All orders have been removed.");
        ResetUI();
    }

    // Reset the UI and timers
    private void ResetUI()
    {
        imageDisplay.sprite = null; // Clear the image
        timerDisplay.text = "--:--"; // Reset the timer display
        currentTime = 0;
        isHandlingOrder = false;
    }

    public List<Order> GetOrders(){
        return orders;
    }
}
