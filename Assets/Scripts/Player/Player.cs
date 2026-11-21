using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerControlls))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(NearbyObjects))]
public class Player : MonoBehaviour
{
    private InputHandler inputHandler;
    private NearbyObjects nearbyObjects;

    private Inventory inventory = new();

    private float salary = 12f;

    public PlayerType playerType = PlayerType.Player;

    public NearbyObjects NearbyObjects => nearbyObjects;

    public Inventory Inventory => inventory;

    public event Action OnSalaryChanged;

    public event Action OnBoxDelivered;

    //Add this line
    private Animator animator;

    private void Awake()
    {
        name = "Mike Wazowski";

        inputHandler = GetComponent<InputHandler>();
        nearbyObjects = GetComponent<NearbyObjects>();

        inventory.Init(playerType);

        //Add this line
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        inputHandler.HandleInput();
    }

    public void SaveBox()
    {
        Box actualBox = nearbyObjects.actualBox;

        if (!actualBox)
        {
            Debug.LogError("Actual Box is null");
            return;
        }

        BoxType boxType = actualBox.BoxType;

        Debug.Log($"Box type is {boxType}");

        if (!inventory.Add(boxType))
            return;

        if (actualBox.gameObject != null)
            Destroy(actualBox.gameObject);
    }

    public void GiveBoxToTruck()
    {
        BoxType actualBox = inventory.GetBox(0);
        Truck truck = nearbyObjects.truck;

        foreach (BoxType item in inventory.boxes)
        {
            Debug.Log($"{item} on inventory");
        }

        if (actualBox == BoxType.None)
        {
            Debug.LogError("actual box is none");
            return;
        }

        if (truck == null)
        {
            Debug.LogError("truck is null");
            return;
        }

        if (!inventory.Remove(actualBox))
        {
            Debug.LogWarning($"{actualBox} failed to remove from player inventory");
            return;
        }

        //Add this line
        animator.SetInteger("BoxType", 0);

        TruckResult truckResult = truck.TruckResult(actualBox);

        if (!truckResult.added)
        {
            Debug.LogWarning($"{actualBox} failed to add to truck inventory");
            return;
        }

        if (!truck.CorrectBox().Contains(actualBox))
        {
            ReduceSalaryBy(3f);
        }

        OnBoxDelivered?.Invoke();
    }

    public void ReduceSalaryBy(float num)
    {
        Mathf.Max(0, salary -= num);

        OnSalaryChanged?.Invoke();
    }

    public float GetSalary()
    {
        return salary;
    }
}

public enum PlayerType
{
    Player, 
    Truck
}