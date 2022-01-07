using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class InputValuesUI : MonoBehaviour
{
    TextMeshProUGUI textInput;/*

    public string key;*/
    //InstrumentSystem instrumentSystem = new InstrumentSystem();
    private void Awake()
    {
        textInput = GetComponent<TextMeshProUGUI>();
    }
    
    public void DisplayText(string value)
    {
        textInput.text = value;
    }

}
