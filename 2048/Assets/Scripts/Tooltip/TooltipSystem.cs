using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem tSystem;

    public Tooltip tooltip;
    public void Awake()
    {
        tSystem = this;
    }

    public static void Show(string text)
    {        
        tSystem.tooltip.SetText(text);
        tSystem.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        tSystem.tooltip.gameObject.SetActive(false);
    }

}
