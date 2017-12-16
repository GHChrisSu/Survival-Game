using UnityEngine;
using System.Collections;

public class ItemProperties : MonoBehaviour
{
    [Header("Your Consumables")]
    public string itemName;

    [SerializeField] private bool food;
    [SerializeField] private bool water;
    [SerializeField] private bool health;
    [SerializeField] private float value;

    [SerializeField] private bool sleepingBag;
    [SerializeField] private SleepController sleepController;

    private void Start()
    {
        sleepController = GameObject.FindGameObjectWithTag("SleepController").GetComponent<SleepController>();
    }

    public void Interaction(PlayerVitals playerVitals)
    {
        if (food)
        {
            playerVitals.hungerSlider.value += value;
            this.gameObject.SetActive(false);
        }

        else if (water)
        {
            playerVitals.thirstSlider.value += value;
            this.gameObject.SetActive(false);
        }

        else if (health)
        {
            playerVitals.healthSlider.value += value;
            this.gameObject.SetActive(false);
        }

        else if(sleepingBag)
        {
            sleepController.EnableSleepUI();
        }
    }
}
