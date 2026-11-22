using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Editor;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Truck : MonoBehaviour
{
    private PlayerType playerType = PlayerType.Truck;
    public TruckType truckType;

    private Inventory inventory = new();

    private HashSet<BoxType> boxTypes = new();

    private HashSet<BoxType> validBoxes = new();

    private readonly Sprite[] truckSprites = new Sprite[2];

    public bool isClosed = true;

    private SpriteRenderer spriteRender;

    public void Awake()
    {
        inventory.Init(playerType);

        validBoxes.AddRange(InitalValidItems());
        boxTypes.AddRange(FillTrucks());

        spriteRender = transform.Find("TruckRender").GetComponent<SpriteRenderer>();

        truckSprites[0] = Resources.Load<Sprite>("Prefabs/ClosedTruck");
        truckSprites[1] = Resources.Load<Sprite>("Prefabs/OpenedTruck");

        spriteRender.sprite = truckSprites[0];
    }

    public void SetSprite()
    {
        StartCoroutine(WaitForSprites());
    }

    private IEnumerator WaitForSprites()
    {
        yield return new WaitUntil(() => spriteRender != null && spriteRender.sprite != null);

        Sprite newSprite = isClosed switch
        {
            true => truckSprites[0],
            _ => truckSprites[1]
        };

        spriteRender.sprite = newSprite;
    }

    private HashSet<BoxType> InitalValidItems()
    {
        return new HashSet<BoxType>() { BoxType.Blue, BoxType.Green, BoxType.Normal, BoxType.Red };
    }

    public void SecondValidItems()
    {
        validBoxes.AddRange(new HashSet<BoxType>() { BoxType.DragonBallNormalBox,
            BoxType.DragonBallGreenBox,
            BoxType.DragonBallRedBox,
            BoxType.DragonBallBlueBox });
    }

    public void ThirdValidItems()
    {
        validBoxes.AddRange(new HashSet<BoxType>() { BoxType.NormalDuckBox,
            BoxType.DuckGreenBox,
            BoxType.DuckRedBox,
            BoxType.DuckBlueBox});
    }

    public void ChangeTruckRules()
    {
        boxTypes.Clear();
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
            BoxType randomBox = GetRandomWeightedItem(validBoxes.ToArray());

            if (!boxTypes.Add(randomBox))
            {
                i--;
                continue;
            }
        }

        return boxTypes.ToArray();
    }

    private int GetRandomBox(BoxType boxType)
    {
        return boxType switch
        {
            BoxType.Normal => 100,
            BoxType.Blue => 70,
            BoxType.Green => 50,
            BoxType.Red => 30,

            BoxType.DragonBallNormalBox => 20,
            BoxType.DragonBallGreenBox => 15,
            BoxType.DragonBallRedBox => 10,
            BoxType.DragonBallBlueBox => 5,

            BoxType.NormalDuckBox => 20,
            BoxType.DuckGreenBox => 15,
            BoxType.DuckRedBox => 10,
            BoxType.DuckBlueBox => 5,

            _ => 0
        };
    }

    public BoxType GetRandomWeightedItem(BoxType[] validBoxes)
    {
        int totalWeight = 0;

        foreach (BoxType item in validBoxes)
            totalWeight += GetRandomBox(item);

        if (totalWeight <= 0)
            return BoxType.None;

        int randomValue = Random.Range(0, totalWeight);

        foreach (BoxType item in validBoxes)
        {
            randomValue -= GetRandomBox(item);

            if (randomValue < 0)
                return item;
        }

        return BoxType.None;
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
}