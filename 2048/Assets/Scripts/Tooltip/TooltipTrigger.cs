using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
  
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Time.timeScale = 1f;
        StartCoroutine(DelayCoroutine());
    }

    // Update is called once per frame
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.75f);
        
        TooltipSystem.Show(content);
        //Time.timeScale = 0f;
    }
} 