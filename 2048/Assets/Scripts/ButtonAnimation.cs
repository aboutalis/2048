using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;
    private Button button;

    void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        // Add an event listener to the button's onClick event
        //button.onClick.AddListener(ResetAnimation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.Play("Highlighted");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.Play("Normal");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.Play("Pressed");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.Play("Normal");
        }
    }

    //void ResetAnimation()
    //{
    //    // Reset the animation to the "Normal" state
    //    animator.Play("Normal");
    //}
}
