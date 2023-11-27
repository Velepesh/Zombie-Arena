using UnityEngine;
using System;

public class Sensitivity
{
    public Sensitivity(float sensitivity)
    {
        if (sensitivity < 0)
            throw new ArgumentException(nameof(sensitivity));

        Value = sensitivity;
        UpdateSensitivityVector(sensitivity);
    }

    public Vector2 SensitivityVector { get; private set; }
    public float Value { get; private set; }

    public void UpdateSensitivityVector(float value)
    {
        SensitivityVector = new Vector2(value, value);
    }
}