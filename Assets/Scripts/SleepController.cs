using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepController : MonoBehaviour
{
    [SerializeField] private GameObject sleepUI;
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private Text sleepNumber;

    [SerializeField] private float hourlyRegen;
    [SerializeField] private DisableManager disableManager;

    private void Start()
    {
        disableManager = GameObject.FindGameObjectWithTag("DisableController").GetComponent<DisableManager>();
    }

    public void EnableSleepUI()
    {
        sleepUI.SetActive(true);
        disableManager.DisablePlayer();
    }

    public void UpdateSlider()
    {
        sleepNumber.text = sleepSlider.value.ToString("0");
    }

    public void SleepBtn(PlayerVitals playerVitals)
    {
        playerVitals.fatigueSlider.value = sleepSlider.value * hourlyRegen;
        playerVitals.fatMaxStamina = playerVitals.fatigueSlider.value;
        playerVitals.staminaSlider.value = playerVitals.normMaxStamina;
        sleepSlider.value = 1;
        disableManager.EnablePlayer();
        sleepUI.SetActive(false);
    }
}