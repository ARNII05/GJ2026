using UnityEngine;
using TMPro;
using System;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const float maxTimePerBox = 10f;

    private const int boxsesUpgraderInterval = 1;

    private float actualBoxsesUpgrader = 0;

    private float actualTime = maxTimePerBox;

    private const int maxAddingIndex = 3;

    private int actualAddingIndex = 1;


    private TMP_Text timeText, dayText, salaryText, boxesDeliveredText,
        instructionsText;

    private Player player;

    private Box spawnedBox;

    private Vector3 boxPosition = new(61.96f, -1.02f, 0);

    private int day = 1;

    private const int maxDays = 2;

    private int boxesInDay = 0;

    private int boxesDelivered = 0;

    private Truck[] trucks;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        timeText = GameObject.Find("TimeText").GetComponent<TMP_Text>();
        dayText = GameObject.Find("DayText").GetComponent<TMP_Text>();
        salaryText = GameObject.Find("SalaryText").GetComponent<TMP_Text>();
        boxesDeliveredText = GameObject.Find("BoxesDeliveredText").GetComponent<TMP_Text>();
        instructionsText = GameObject.Find("InstructionsText").GetComponent<TMP_Text>();
        
        player = FindAnyObjectByType<Player>();

        player.OnSalaryChanged += GameOver;
        player.OnSalaryChanged += ChangeSalaryText;

        player.OnBoxDelivered += ChangeDayStatus;

        trucks = FindObjectsByType<Truck>();

        ChangeDay(false);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        actualTime -= Time.deltaTime;

        timeText.text = $"Temps: {(int)actualTime} segons";

        if (actualTime <= 0)
        {
            CheckForBox();
            actualTime = maxTimePerBox;
        }
    }

    private void SetBoxesPerDay()
    {
        boxesInDay = day switch
        {
            1 => 15,
            2 => 20,
            _ => 0
        };
    }

    private string GetBoxPath()
    {
        Truck[] activeTrucks = trucks.Where(truck => truck.gameObject.activeSelf).ToArray();

        int randomIndex = Random.Range(0, activeTrucks.Length);

        BoxType[] correctBox = activeTrucks[randomIndex].CorrectBox();

        int randomBox = Random.Range(0, correctBox.Length);

        return GetRandomBox(correctBox[randomBox]);
    }

    private string GetRandomBox(BoxType boxType)
    {
        return boxType switch
        {
            BoxType.Normal => "NormalBox",
            BoxType.Blue => "BlueBox",
            BoxType.Green => "GreenBox",
            BoxType.Red => "RedBox",

            BoxType.DragonBallNormalBox => "DragonBallNormalBox",
            BoxType.DragonBallGreenBox => "DragonBallGreenBox",
            BoxType.DragonBallRedBox => "DragonBallRedBox",
            BoxType.DragonBallBlueBox => "DragonBallBlueBox",

            BoxType.NormalDuckBox => "NormalDuckBox",
            BoxType.DuckGreenBox => "DuckGreenBox",
            BoxType.DuckRedBox => "DuckRedBox",
            BoxType.DuckBlueBox => "DuckBlueBox",

            _ => "None"
        };
    }

    private void SetTrucksPerDay()
    {
        foreach (Truck truck in trucks)
            truck.gameObject.SetActive(false);

        int trucksCount = day == 1 ? 2 : 3;

        for (int i = 0; i < trucksCount; i++)
        {
            trucks[i].gameObject.SetActive(true);
        }
    }

    private void ChangeDayStatus()
    {
        if (gameEnded)
            return;

        boxesDelivered++;

        AddNewBoxType();

        actualBoxsesUpgrader++;

        if (actualBoxsesUpgrader == boxsesUpgraderInterval)
        {
            UpgrateBoxes();
        }

        if (boxesDelivered >= boxesInDay)
        {
            ChangeDay();
            return;
        }

        boxesDeliveredText.text = $"Caixes entregades: {boxesDelivered} / {boxesInDay}";

        SpawnBox();
    }

    private void UpgrateBoxes()
    {
        Truck[] activeTrucks = trucks.Where(truck => truck.gameObject.activeSelf).ToArray();

        foreach (Truck truck in activeTrucks)
        {
            truck.ChangeTruckRules();
        }

        FillCorrectBoxText();

        actualBoxsesUpgrader = 0;
    }

    private void AddNewBoxType()
    {
        if (actualAddingIndex >= maxAddingIndex)
            return;

        switch (actualAddingIndex)
        {
            case 2:
                foreach (Truck truck in trucks)
                    truck.SecondValidItems();
                break;
            case 3:
                foreach (Truck truck in trucks)
                    truck.ThirdValidItems();
                break;
        }

        actualAddingIndex++;
    }

    private void ChangeDay(bool sumDay = true)
    {
        if (day >= maxDays)
        {
            GameWon();
            return;
        }

        actualTime = maxTimePerBox;

        day = sumDay ? day + 1 : day;

        boxesDelivered = 0;

        dayText.text = $"Dia: {day}";

        SetTrucksPerDay();

        SetBoxesPerDay();

        SpawnBox();

        FillCorrectBoxText();

        ChangeSalaryText();

        boxesDeliveredText.text = $"Caixes entregades: {boxesDelivered} / {boxesInDay}";
    }

    private void FillCorrectBoxText()
    {
        instructionsText.text = string.Empty;

        Truck[] activeTrucks = trucks.Where(truck => truck.gameObject.activeSelf).ToArray();

        foreach (Truck truck in activeTrucks)
        {
            BoxType[] correctBoxes = truck.CorrectBox();

            instructionsText.text += $"{truck.truckType} => ";

            for (int i = 0; i < correctBoxes.Length; i++)
            {
                instructionsText.text += $"{BoxNames(correctBoxes[i])}";

                instructionsText.text += i == correctBoxes.Length - 1 ? ".\n" : ", " ;
            }
        }
    }

    public string BoxNames(BoxType boxType)
    {
        return boxType switch
        {
            BoxType.Normal => "Normals",
            BoxType.Blue => "Blaves",
            BoxType.Green => "Verdes",
            BoxType.Red => "Vermelles",

            BoxType.DragonBallNormalBox => "Dragon Ball Normals",
            BoxType.DragonBallGreenBox => "Dragon Ball Verdes",
            BoxType.DragonBallRedBox => "Dragon Ball Vermelles",
            BoxType.DragonBallBlueBox => "Dragon Ball Blaves",

            BoxType.NormalDuckBox => "Ànec Normals",
            BoxType.DuckGreenBox => "Ànec Verdes",
            BoxType.DuckRedBox => "Ànec Vermelles",
            BoxType.DuckBlueBox => "Ànec Blaves",

            _ => "None"
        };
    }

    private void ChangeSalaryText()
    {
        float playerSalary = player.GetSalary();

        salaryText.text = $"Sou: {playerSalary}";
    }

    private void SpawnBox()
    {
        string boxPath = $"Prefabs/Boxes/{GetBoxPath()}";

        Debug.Log($"Box path is: {boxPath}");

        GameObject boxToInstantiate = Resources.Load<GameObject>(boxPath);

        spawnedBox = Instantiate(boxToInstantiate, boxPosition, Quaternion.identity).GetComponent<Box>();

        Debug.Log("Box has spawned");

        actualTime = maxTimePerBox;
    }

    private void CheckForBox()
    {
        if (spawnedBox || player.Inventory.HasBox())
        {
            player.ReduceSalaryBy(3f);

            return;
        }
    }

    private void GameOver()
    {
        float playerSalary = player.GetSalary();

        if (playerSalary <= 0)
        {
            Debug.Log("You have been fired. End game");
            gameEnded = true;
            //Panel de despido
            return;
        }
    }

    private void GameWon()
    {
        Debug.Log("You finished the game!!");
        gameEnded = true;
    }

    private void OnDestroy()
    {
        player.OnSalaryChanged -= GameOver;
    }
}
