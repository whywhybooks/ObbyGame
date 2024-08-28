using UnityEngine;

public static class CanvasGroup_Extention
{
    public static void Activate (this CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public static void Deactivate(this CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}