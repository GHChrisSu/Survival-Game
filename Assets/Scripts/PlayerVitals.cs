/*SCRIPTS WRITTEN BY SPEEDTUTOR - FREE TUTORIALS AT WWW.SPEED-TUTOR.COM*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerVitals : MonoBehaviour
{
    #region Slider Variables
    [Space(10)]
    public Slider healthSlider;
    public int maxHealth;
    public int healthFallRate;

    [Space(10)]
    public Slider thirstSlider;
    public int maxThirst;
    public int thirstFallRate;

    [Space(10)]
    public Slider hungerSlider;
    public int maxHunger;
    public int hungerFallRate;

    [Space(10)]
    public Slider fatigueSlider;
    public int maxfatigue;
    public int fatigueFallRate;
    public float fatigueValue;

    [Space(10)]
    public Slider staminaSlider;
    public float normMaxStamina;
    public float fatMaxStamina;
    private int staminaFallRate;
    public int staminaFallMult;
    private int staminaRegainRate;
    public int staminaRegainMult;

    [Space(10)]
    [Header("Temperature Settings")]
    public float freezingTemp;
    public float currentTemp;
    public float normalTemp;
    public float heatTemp;
    public Text tempNumber;
    public Image tempBG;
    #endregion

    [Space(10)]
    private CharacterController charController;
    private FirstPersonController playerController;

    void Start()
    {
        #region Starting Sliders
        fatigueSlider.maxValue = maxfatigue;
        fatigueSlider.value = maxfatigue;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = maxThirst;

        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = maxHunger;

        staminaSlider.maxValue = normMaxStamina;
        staminaSlider.value = normMaxStamina;

        staminaFallRate = 1;
        staminaRegainRate = 1;

        charController = GetComponent<CharacterController>();
        playerController = GetComponent<FirstPersonController>();
        #endregion
    }

    void UpdateTemp()
    {
        tempNumber.text = currentTemp.ToString("00.0");
    }

    void Update()
    {
        #region Temperature Region
        //TEMPERATURE SECTION
        if (currentTemp <= freezingTemp)
        {
            tempBG.color = Color.blue;
            UpdateTemp();
        }

        else if (currentTemp >= heatTemp - 0.1)
        {
            tempBG.color = Color.red;
            UpdateTemp();
        }

        else
        {
            tempBG.color = Color.green;
            UpdateTemp();
        }
        #endregion

        #region Fatigue Region
        //FATIGUE SECTION
        fatigueValue = Mathf.FloorToInt(fatigueSlider.value);

        if (fatigueValue <= 60)
        {
            fatMaxStamina = 80;
            staminaSlider.value = fatMaxStamina;
        }

        else if (fatigueValue <= 40)
        {
            fatMaxStamina = 60;
            staminaSlider.value = fatMaxStamina;
        }

        else if (fatigueValue <= 20)
        {
            fatMaxStamina = 40;
            staminaSlider.value = fatMaxStamina;
        }

        else if (fatigueValue <= 5)
        {
            fatMaxStamina = 20;
            staminaSlider.value = fatMaxStamina;
        }

        if (fatigueSlider.value >= 0)
        {
            fatigueSlider.value -= Time.deltaTime * fatigueFallRate;
        }

        else if (fatigueSlider.value <= 0)
        {
            fatigueSlider.value = 0;
        }

        else if (fatigueSlider.value >= maxfatigue)
        {
            fatigueSlider.value = maxfatigue;
        }
        #endregion

        #region Health Region
        //HEALTH CONTROL SECTION
        if (hungerSlider.value <= 0 && (thirstSlider.value <= 0))
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 2;
        }

        else if (hungerSlider.value <= 0 || thirstSlider.value <= 0 || currentTemp <= freezingTemp || currentTemp >= heatTemp)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate;
        }

        if (healthSlider.value <= 0)
        {
            CharacterDeath();
        }
        #endregion

        #region Hunger Region
        //HUNGER CONTROL SECTION
        if (hungerSlider.value >= 0)
        {
            hungerSlider.value -= Time.deltaTime / hungerFallRate;
        }

        else if (hungerSlider.value <= 0)
        {
            hungerSlider.value = 0;
        }

        else if (hungerSlider.value >= maxHunger)
        {
            hungerSlider.value = maxHunger;
        }
        #endregion

        #region Thirst Region
        //THIRST CONTROL SECTION
        if (thirstSlider.value >= 0)
        {
            thirstSlider.value -= Time.deltaTime / thirstFallRate;
        }

        else if (thirstSlider.value <= 0)
        {
            thirstSlider.value = 0;
        }

        else if (thirstSlider.value >= maxThirst)
        {
            thirstSlider.value = maxThirst;
        }
        #endregion

        #region Stamina Region
        //STAMINA CONTROL SECTION
        if (charController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            staminaSlider.value -= Time.deltaTime / staminaFallRate * staminaFallMult;

            if (staminaSlider.value > 0)
            {
                currentTemp += Time.deltaTime / 5;
            }
        }

        else
        {
            staminaSlider.value += Time.deltaTime / staminaRegainRate * staminaRegainMult;

            if (currentTemp >= normalTemp)
            {
                currentTemp -= Time.deltaTime / 10;
            }
        }

        if (staminaSlider.value >= fatMaxStamina)
        {
            staminaSlider.value = fatMaxStamina;
        }

        else if (staminaSlider.value <= 0)
        {
            staminaSlider.value = 0;
            playerController.m_RunSpeed = playerController.m_WalkSpeed;
        }

        else if (staminaSlider.value >= 0)
        {
            playerController.m_RunSpeed = playerController.m_RunSpeedNorm;
        }
    }
    #endregion

    #region Death Region
    void CharacterDeath()
    {
        //DEATH
    }
    #endregion
}

