using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicColorAdjustment : MonoBehaviour
{
    [SerializeField] Volume volume;

    [Header("Color Settings")]
    [SerializeField] float maxContrast = 15f;
    [SerializeField] float maxSaturation = 20f;
    [SerializeField] float transitionDuration = 60f;

    ColorAdjustments colorAdjustments;
    float elapsedTime;

    void Start()
    {
        if (!volume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments not found.");
            enabled = false;
            return;
        }

        colorAdjustments.contrast.overrideState = true;
        colorAdjustments.saturation.overrideState = true;

        colorAdjustments.contrast.value = 0f;
        colorAdjustments.saturation.value = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(
            elapsedTime / transitionDuration
        );

        colorAdjustments.contrast.value =
            Mathf.Lerp(0f, maxContrast, progress);

        colorAdjustments.saturation.value =
            Mathf.Lerp(0f, maxSaturation, progress);
    }
}