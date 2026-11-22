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

    private Vector3 initialPos = new(58.67f, -3.28f, 0);

    private Animator animator;

    private void Awake()
    {
        name = "Mike Wazowski";

        inputHandler = GetComponent<InputHandler>();
        nearbyObjects = GetComponent<NearbyObjects>();

        inventory.Init(playerType);

        transform.position = initialPos;

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

        if (!inventory.Add(boxType))
            return;

        SetAnim(boxType);

        if (actualBox.gameObject != null)
            Destroy(actualBox.gameObject);
    }

    private void SetAnim(BoxType boxType)
    {
        switch (boxType)
        {
            case BoxType.None:
                animator.SetInteger("BoxType", 0);
                break;

            case BoxType.Normal:
                animator.SetInteger("BoxType", 1);
                break;

            case BoxType.Green:
                animator.SetInteger("BoxType", 2);
                break;

            case BoxType.Red:
                animator.SetInteger("BoxType", 3);
                break;

            case BoxType.Blue:
                animator.SetInteger("BoxType", 4);
                break;

            case BoxType.DragonBallNormalBox:
                animator.SetInteger("BoxType", 5);
                break;

            case BoxType.DragonBallGreenBox:
                animator.SetInteger("BoxType", 6);
                break;

            case BoxType.DragonBallRedBox:
                animator.SetInteger("BoxType", 7);
                break;

            case BoxType.DragonBallBlueBox:
                animator.SetInteger("BoxType", 8);
                break;

            case BoxType.NormalDuckBox:
                animator.SetInteger("BoxType", 9);
                break;

            case BoxType.DuckGreenBox:
                animator.SetInteger("BoxType", 10);
                break;

            case BoxType.DuckRedBox:
                animator.SetInteger("BoxType", 11);
                break;

            case BoxType.DuckBlueBox:
                animator.SetInteger("BoxType", 12);
                break;

            default:
                animator.SetInteger("BoxType", 0);
                break;
        }
    }

    public void GiveBoxToTruck()
    {
        BoxType actualBox = inventory.GetBox(0);
        Truck truck = nearbyObjects.truck;

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

        if (truck.isClosed)
            return;

        if (!inventory.Remove(actualBox))
        {
            Debug.LogWarning($"{actualBox} failed to remove from player inventory");
            return;
        }

        TruckResult truckResult = truck.TruckResult(actualBox);

        if (!truckResult.added)
        {
            Debug.LogWarning($"{actualBox} failed to add to truck inventory");
            return;
        }

        animator.SetInteger("BoxType", 0);

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