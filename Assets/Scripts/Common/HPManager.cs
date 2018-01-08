using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public string label = "HP";
    public int minValue = 0;
    public int maxValue = 100;
    public int currentValue; // Set in inspector to initial value

    public Transform UIPrefab;


    private void Awake()
    {
        if (UIPrefab != null)
        {
            Instantiate(UIPrefab, transform);
        }
    }
}
