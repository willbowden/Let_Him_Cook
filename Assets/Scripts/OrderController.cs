using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderController : MonoBehaviour
{
    [Header("List of Orders")]
    [SerializeField] private List<Order> orders = new List<Order>(); // List of orders
    private bool isRestaurantOpen = false; // Tracks if the restaurant is open

    [Header("UI Elements")]
    [SerializeField] private TMP_Text orderNames; // Reference to the TMP_Text for the names
    [SerializeField] private TMP_Text orderTimes; // Reference to the TMP_Text for the times

    void Start()
    {
        // Restaurant starts closed, no orders loaded initially
        ResetUI();
        Open();
    }

    void Update()
    {
        if (!isRestaurantOpen) return; // Skip update if the restaurant is closed

        // Update the timers for each order
        UpdateOrderTimes();
        UpdateUI();
    }

    // Open the restaurant
    public void Open()
    {
        isRestaurantOpen = true;
        Debug.Log("Restaurant is now open!");
        UpdateUI();
    }

    // Close the restaurant
    public void Close()
    {
        isRestaurantOpen = false;
        Debug.Log("Restaurant is now closed!");
        ResetUI();
    }

    // Add a new order to the list
    public void AddOrder(Order order)
    {
        orders.Add(order);
        Debug.Log($"Order added: {order.name}");
        UpdateUI();
    }

    // Remove an order from the list
    public void RemoveOrder(Order order)
    {
        orders.Remove(order);
        Debug.Log($"Order removed: {order.name}");
        UpdateUI();
    }

    // Remove all orders and reset
    public void RemoveAllOrders()
    {
        orders.Clear();
        Debug.Log("All orders have been removed.");
        ResetUI();
    }

    // Update the timers for each order
    private void UpdateOrderTimes()
    {
        foreach (var order in orders)
        {
            if (order.timeInSeconds > 0)
            {
                order.timeInSeconds -= (int)Time.deltaTime;
            }
        }
    }

    // Update the UI to reflect the current orders
    private void UpdateUI()
    {
        if (orders.Count == 0)
        {
            ResetUI();
            return;
        }

        // Update order names
        string namesText = "";
        string timesText = "";

        for (int i = 0; i < orders.Count; i++)
        {
            var order = orders[i];
            namesText += $"{i + 1}. {order.name}\n";
            int minutes = Mathf.FloorToInt(order.timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(order.timeInSeconds % 60);
            timesText += $"[{minutes:D2}:{seconds:D2}]\n";
        }

        orderNames.text = namesText.TrimEnd();
        orderTimes.text = timesText.TrimEnd();
    }

    // Reset the UI
    private void ResetUI()
    {
        orderNames.text = "No orders.";
        orderTimes.text = "[--:--]";
    }

    public List<Order> GetOrders() {
        return orders;
    }
}
