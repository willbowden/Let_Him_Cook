using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OrderController : MonoBehaviour
{
    [Header("List of Orders")]
    [SerializeField] private List<Order> orders = new List<Order>(); // List of orders
    private bool isRestaurantOpen = false; // Tracks if the restaurant is open

    [Header("UI Elements")]
    [SerializeField] private TMP_Text orderNames; // Reference to the TMP_Text for the names
    [SerializeField] private TMP_Text orderTimes; // Reference to the TMP_Text for the times

    private AudioSource chimeSource;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        chimeSource = GetComponent<AudioSource>();
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
        chimeSource.Play();
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

            // Debug.Log($"update timeInSeconds {order.timeInSeconds}");
            order.timeInSeconds -= Time.deltaTime;

        }
    }

    // Update the UI to reflect the current orders
    private void UpdateUI()
    {
        // Debug.Log("update UI");
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
            int minutes = (int)Math.Floor(order.timeInSeconds / 60);
            int seconds = (int)Math.Floor(order.timeInSeconds % 60);
            // int minutes = Mathf.FloorToInt(order.timeInSeconds / 60);
            // int seconds = Mathf.FloorToInt(order.timeInSeconds % 60);
            if (order.timeInSeconds < 0)
            {
                timesText += $"<color=red>[{minutes:D2}:{seconds:D2}]</color>\n";
                namesText += $"<color=red>{i + 1}. {order.name}</color>\n";
            }
            else
            {
                timesText += $"[{minutes:D2}:{seconds:D2}]\n";
                namesText += $"{i + 1}. {order.name}\n";
            }
        }

        orderNames.text = namesText.TrimEnd();
        orderTimes.text = timesText.TrimEnd();
    }

    // Reset the UI
    private void ResetUI()
    {
        if (gameManager != null && gameManager.kitchenPrepDuration > 0)
        {
            int minutes = (int)Math.Floor(gameManager.kitchenPrepDuration / 60);
            int seconds = (int)Math.Floor(gameManager.kitchenPrepDuration % 60);
            orderNames.text = "Kitchen will open in:";
            orderTimes.text = $"[{minutes:D2}:{seconds:D2}]";
        }
        else
        {
            orderNames.text = "No orders.";
            orderTimes.text = "[--:--]";
        }
    }

    public List<Order> GetOrders()
    {
        return orders;
    }
}
