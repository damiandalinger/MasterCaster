using UnityEngine;

public class SizeModCalculator : MonoBehaviour
{
    [Header("Input Parameters")]
    public float listenerCount = 500f;   // x-Wert
    public float steepness = 0.005f;     // a-Wert

    [Header("Output")]
    public float sizeModResult;          // y = SizeMod(x)

    void Update()
    {
        sizeModResult = CalculateSizeMod(listenerCount);
    }

    /// <summary>
    /// Berechnet den SizeMod-Wert für eine gegebene Zuhörerzahl und Steilheit.
    /// </summary>
    float CalculateSizeMod(float x)
    {
        if (x < 1000f) return 1f; // oder eigene Boost-Funktion

        float c = 0.2f;
        float r = 0.00000208f;
        return c + (1f - c) * Mathf.Exp(-r * (x - 1000f));
    }
}
