using UnityEngine;

public class Sensitivity
{
    public Vector2 SensitivityVector { get; private set; }

    public void UpdateSensitivityVector(float value)
    {
        SensitivityVector = new Vector2(value, value);
    }
}