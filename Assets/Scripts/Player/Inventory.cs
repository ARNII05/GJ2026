using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    public List<BoxType> boxes = new();
    private int maxCapacity = 0;

    private PlayerType playerType;

    public void Init(PlayerType playerType)
    {
        this.playerType = playerType;

        maxCapacity = playerType switch
        {
            PlayerType.Player => 1,
            PlayerType.Truck => 10,
            _ => 0
        };
    }

    public bool Add(BoxType boxType)
    {
        if (boxType == BoxType.None)
        {
            Debug.Log("Trying to add a null box");
            return false;
        }

        if (boxes.Count >= maxCapacity)
        {
            Debug.LogWarning("Inventory is full");
            return false;
        }
        
        boxes.Add(boxType);

        Debug.Log($"{boxType} added to {playerType} inventory");

        return true;
    }

    public bool Remove(BoxType boxType)
    {
        if (boxType == BoxType.None)
            return false;

        if (!boxes.Remove(boxType))
            return false;

        Debug.Log($"{boxType} removed from {playerType} inventory");

        return true;
    }

    public BoxType GetBox(int index)
    {
        if (index < 0 || index >= boxes.Count)
            return BoxType.None;

        return boxes[index];
    }

    public bool HasBox()
    {
        return boxes.Count > 0;
    }
}
