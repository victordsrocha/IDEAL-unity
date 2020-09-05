using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PotGenerator : MonoBehaviour
{
    public Pot potPrefab;
    private Pot pot;

    void Start()
    {
        pot = null;
    }

    private void LateUpdate()
    {
        if (pot == null)
        {
            pot = InstantiateNewPot();
        }
    }

    public Pot InstantiateNewPot()
    {
        Pot newPot = null;
        
        float x = Random.Range(-15f, 15f);
        float y = Random.Range(-7f, 8f);
        Vector2 birthSpot = new Vector2(x, y);

        Collider2D overlapCircle = Physics2D.OverlapCircle(birthSpot, 1f);
        if (overlapCircle == null)
        {
            newPot = Instantiate(potPrefab, birthSpot, Quaternion.identity);
        }

        return newPot;
    }
}