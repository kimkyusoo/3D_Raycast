using UnityEngine;

public enum InteractableType
{
    None,
    NPC,
    Chest,
    Hub
};

public class InteractableController : MonoBehaviour
{
    public InteractableType InteractableType;
    public bool isInteractable = false;

    public bool CheckInteract()
    {
        if (isInteractable) return false;
        else return true;
    }

    public bool UseInteract()
    {
        if (isInteractable) return false;

        isInteractable = true;
        return true;
    }
}
