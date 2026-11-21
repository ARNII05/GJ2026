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

    private float actualTime = maxTimePerBox;

    private TMP_Text timeText, dayText, salaryText, boxesDeliveredText;

    private Player player;

    private Box spawnedBox;

    private const string boxPath = "Prefabs/Box";

    private Vector3 boxPosition = new(8.91f, 0.29f, 0);

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

        player = FindAnyObjectByType<Player>();

        player.OnSalaryChanged += GameOver;
        player.OnSalaryChanged += ChangeSalaryText;

        player.OnBoxDelivered += ChangeDayStatus;

        trucks = FindObjectsByType<Truck>();

        ChangeDay(false);
    }

    private void Update()
    {
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
            1 => 1,
            2 => 1,
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
            BoxType.Blue => "BlueBox",
            BoxType.Green => "GreenBox",
            BoxType.Normal => "NormalBox",
            BoxType.Red => "RedBox",
            _ => "None"
        };
    }

    private void SetTrucksPerDay()
    {
        foreach (Truck truck in trucks)
            truck.gameObject.SetActive(false);

        for (int i = 0; i < day * 2; i++)
        {
            trucks[i].gameObject.SetActive(true);
        }
    }

    private void ChangeDayStatus()
    {
        if (gameEnded)
            return;

        boxesDelivered++;

        if (boxesDelivered >= boxesInDay)
        {
            ChangeDay();
            return;
        }

        boxesDeliveredText.text = $"Caixes entregades: {boxesDelivered}";

        SpawnBox();
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
        boxesDeliveredText.text = $"Caixes entregades: {boxesDelivered}";

        SetTrucksPerDay();

        SetBoxesPerDay();

        SpawnBox();

        ChangeSalaryText();
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
