using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDayLeftTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    void Update()
    {
        var nextDay = DateTime.Today.AddDays(1);
        var currDay = DateTime.Now;

        var timeSpan = nextDay - currDay;
        
        _text.SetText($"{timeSpan:hh}:{timeSpan:mm}:{timeSpan:ss}");
    }
}
