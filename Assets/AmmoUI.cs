using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public TankStats tankStats;
    public GameObject ammoPrefab;
    public GameObject emptyAmmoPrefab;

    private List<GameObject> ammos = new List<GameObject>();
    private List<GameObject> emptyAmmos = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < tankStats.maxAmmo; i++)
        {
            GameObject ammo = Instantiate(ammoPrefab, transform);
            ammos.Add(ammo);
        }
        for (int i = 0; i < tankStats.maxAmmo; i++)
        {
            GameObject emptyAmmo = Instantiate(emptyAmmoPrefab, transform);
            emptyAmmos.Add(emptyAmmo);
        }
        
        UpdateAmmo(tankStats.ammo);
    }

    private void FixedUpdate()
    {
        UpdateAmmo(tankStats.ammo);
    }

    public void UpdateAmmo(int ammo)
    {
        for (int i = 0; i < ammos.Count; i++)
        {
            ammos[i].SetActive(i < ammo);    //Only sets heart active if it's place in the list is below health
            emptyAmmos[i].SetActive(i >= ammo);
        }
    }
}