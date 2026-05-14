using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHungerUI : MonoBehaviour
{
    [Header("Hunger Display")]
    public Image    hungerImage;
    public TMP_Text hungerPercentText;

    public void UpdateHunger(float currentHunger, float maxHunger)
    {
        float percent = currentHunger / maxHunger;

        if (hungerImage != null)
            hungerImage.fillAmount = percent;

        if (hungerPercentText != null)
            hungerPercentText.text = Mathf.RoundToInt(percent * 100f) + "%";
    }
}