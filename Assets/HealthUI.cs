using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public TankStats tankStats;
    public GameObject heartPrefab;

    private List<GameObject> hearts = new List<GameObject>();

    private void Start()
    {
        Debug.Log(tankStats.maxHealth);
        for (int i = 0; i < tankStats.maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);
            Debug.Log("Added Heart");
        }
        
        UpdateHealth(tankStats.health);
    }

    private void FixedUpdate()
    {
        UpdateHealth(tankStats.health);
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < health);    //Only sets heart active if it's place in the list is below health
        }
    }
}
