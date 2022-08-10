using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICurrencyUpdater : MonoBehaviour
{
    public Action OnCurrencyChanged;

    private void Start()
    {
        OnCurrencyChanged?.Invoke();
    }
}
