using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public TankStats tankStats;
    public GameObject heartPrefab;
    public GameObject emptyHeartPrefab;

    private List<GameObject> hearts = new List<GameObject>();
    private List<GameObject> emptyHearts = new List<GameObject>();

    private void Start()
    {
        //Separate for-loops so that the hearts are separated (otherwise it would be 'empty, full, empty, full...')
        for (int i = 0; i < tankStats.maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);
        }
        for (int i = 0; i < tankStats.maxHealth; i++)
        {
            GameObject emptyHeart = Instantiate(emptyHeartPrefab, transform);
            emptyHearts.Add(emptyHeart);
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
            emptyHearts[i].SetActive(i >= health);
        }
    }
}
