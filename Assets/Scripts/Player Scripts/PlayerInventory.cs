using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;

    public void GiveKey()
    {
        hasKey = true;
        Debug.Log("Picked up key.");
    }
}