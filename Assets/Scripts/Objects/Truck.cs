using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Truck : MonoBehaviour
{
    private PlayerType playerType = PlayerType.Truck;
    public TruckType truckType;

    private Inventory inventory = new();

    private HashSet<BoxType> boxTypes = new();

    public void Awake()
    {
        inventory.Init(playerType);

        boxTypes.AddRange(FillTrucks());
    }

    public TruckResult TruckResult(BoxType boxType)
    {
        return new TruckResult(CorrectBox(), inventory.Add(boxType));
    }

    public BoxType[] CorrectBox()
    {
        return boxTypes.ToArray();
    }

    private BoxType[] FillTrucks()
    {
        HashSet<BoxType> boxTypes = new();

        int boxesCount = Random.Range(1, 3 + 1);

        for (int i = 0; i < boxesCount; i++)
        {
            BoxType randomBox = GetRandomBox(Random.Range(0, 100));

            if (!boxTypes.Add(randomBox))
            {
                i--;
                continue;
            }
        }

        return boxTypes.ToArray();
    }

    private BoxType GetRandomBox(int prob)
    {
        return prob switch
        {
            < 20 => BoxType.Normal,
            < 40 => BoxType.Blue,
            < 60 => BoxType.Green,
            < 80 => BoxType.Red,
            _ => BoxType.None
        };
    }
}

public struct TruckResult
{
    public BoxType[] boxTypesAdded;
    public bool added;

    public TruckResult(BoxType[] boxTypes, bool added)
    {
        this.boxTypesAdded = boxTypes;
        this.added = added;
    }
}

public enum TruckType
{
    A1,
    B2,
    C3,
    D4
}