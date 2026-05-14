using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampfireCookingUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SurvivalInventory inventory;

    [Header("Panel")]
    [SerializeField] GameObject cookingPanel;
    [SerializeField] Button     cookButton;
    [SerializeField] TMP_Text   statusText;
    [SerializeField] Image      progressBar;

    CampfireObject activeFireplace;
    Coroutine      progressRoutine;

    void Start()
    {
        cookButton.onClick.AddListener(OnCookPressed);

        if (progressBar != null)
            progressBar.fillAmount = 0f;

        cookingPanel.SetActive(false);
    }


    public void Open(CampfireObject campfire)
    {
        if (PlacementManager.IsPlacing) return;

        activeFireplace = campfire;

        bool opening = !cookingPanel.activeSelf;
        SetOpen(opening);
    }

    void SetOpen(bool open)
    {
        bool wasOpen = cookingPanel.activeSelf;
        cookingPanel.SetActive(open);

        if (open && !wasOpen)  MenuCursor.OnOpen();
        if (!open && wasOpen)  MenuCursor.OnClose();

        if (open) Refresh();
    }

    public void Close()
    {
        SetOpen(false);
    }


    void OnCookPressed()
    {
        if (activeFireplace == null) return;
        activeFireplace.StartCooking(inventory, this);
        cookButton.interactable = false;
        statusText.text = "Cooking...";
    }


    void Refresh()
    {
        if (progressBar != null) progressBar.fillAmount = 0f;

        bool hasMeat = inventory.HasItem(ItemType.RawMeat, 1);
        cookButton.interactable = hasMeat;
        statusText.text = hasMeat ? "Cook raw meat?" : "No raw meat in inventory";
    }

    public void ShowProgress(float duration)
    {
        if (progressRoutine != null) StopCoroutine(progressRoutine);
        progressRoutine = StartCoroutine(FillProgress(duration));
    }

    IEnumerator FillProgress(float duration)
    {
        if (progressBar == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            progressBar.fillAmount = elapsed / duration;
            yield return null;
        }
        progressBar.fillAmount = 1f;
    }

    public void CookingFinished()
    {
        statusText.text = "Done! Cooked meat added.";
        cookButton.interactable = true;
        if (progressBar != null) progressBar.fillAmount = 0f;
    }
}