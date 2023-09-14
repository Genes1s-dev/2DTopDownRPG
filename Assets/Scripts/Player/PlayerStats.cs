using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    private Player player;
    private int playerLevel = 1;
    private int maxLevel = 20;
    private readonly Dictionary<int, int> experienceRequiredPerLVL = new Dictionary<int, int> {
        {1, 0}, {2, 100}, {3, 150}, {4, 200}, {5, 250}, {6, 300}, {7, 350}, {8, 400}, {9, 450}, {10, 500},
        {11, 600}, {12, 700}, {13, 800}, {14, 900}, {15, 1000}, {16, 1200}, {17, 1400}, {18, 1600}, {19, 1800}, {20, 2000},
    };
    private int currentEXP = 0;
    private int requiredEXP;
    public int maxHealth {get; private set;}
    public float maxEnergy {get; private set;}
    public int Health {get; private set;}
    public float Energy {get; private set;}
    private float energyRegenerationRate = 1f;
    private float energyRegenTimer = 0f;

    public class StatChangedEventArgs<T> : EventArgs  //делаем дженерик класс для наших передаваемых аргументов для большей гибкости и во избежания дублирования кода
    {
        public T currentValue;
        public T maxValue;
        public int level;
    }


    public event EventHandler<StatChangedEventArgs<int>> OnHPChanged;
    public event EventHandler<StatChangedEventArgs<float>> OnEnergyChanged;
    public event EventHandler<StatChangedEventArgs<int>> OnExpGained;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        UpdateMaxStats();
        Health = maxHealth;
        Energy = maxEnergy;
        currentEXP = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))//for testing
        {
            Health -= 20;
            OnHPChanged?.Invoke(this, new StatChangedEventArgs<int> {currentValue = Health, maxValue = maxHealth});
        }
        if (Input.GetKeyDown(KeyCode.E))//for testing
        {
            receiveEXP(20);
        }
        if (Input.GetKeyDown(KeyCode.R))//for testing
        {
            UpdateEnergy(-50);
        }

        
        if (Energy < maxEnergy && !player.isCharging)
        {
            energyRegenTimer += Time.deltaTime;
            if (energyRegenTimer >= 1.0f)
            {
                UpdateEnergy(energyRegenerationRate);
                energyRegenTimer = 0.0f;
            }
        }
    }

    private void UpdateMaxStats()
    {
        maxHealth = 100 + (playerLevel - 1) * 50;
        maxEnergy = 70 + (playerLevel - 1) * 20;
        requiredEXP = experienceRequiredPerLVL[playerLevel + 1];
        energyRegenerationRate += 0.25f;
    }

    public void receiveEXP(int expAmount)
    {
        currentEXP += expAmount;
        if (currentEXP >= requiredEXP)
        {
            int surplusExp = currentEXP - requiredEXP;  //излишек опыта добавим к обнулённому опыту после получения нового уровня
            
            playerLevel++;
            UpdateMaxStats();

            currentEXP = surplusExp;
            Energy = maxEnergy;
            UpdateEnergy();
        }
        OnExpGained?.Invoke(this, new StatChangedEventArgs<int> {currentValue = currentEXP, maxValue = requiredEXP, level = playerLevel});
    }


    public void UpdateEnergy(float amount)
    {
        Energy += amount;
        if (Energy >= maxEnergy)
        {
            Energy = maxEnergy;
        }
        if (Energy < 0)
        {
            Energy = 0;
        }
        OnEnergyChanged?.Invoke(this, new StatChangedEventArgs<float> {currentValue = Energy, maxValue = maxEnergy});
    }


    //Перегруженный метод нужен в случаях, где необходимо обновить бары, когда количество энергии заранее устанавливается 
    //Например, когда игрок повышает уровень, значение текущей энергии устанавливается на максимум.
    public void UpdateEnergy()
    {
        OnEnergyChanged?.Invoke(this, new StatChangedEventArgs<float> {currentValue = Energy, maxValue = maxEnergy});
    }

    public void UpdateHealth(int amount)
    {
        Health += amount;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
        OnHPChanged?.Invoke(this, new StatChangedEventArgs<int> {currentValue = Health, maxValue = maxHealth});

        if (Health <= 0)
        {
            Health = 0;
            player.DeathSequence();
        }
    }
}
