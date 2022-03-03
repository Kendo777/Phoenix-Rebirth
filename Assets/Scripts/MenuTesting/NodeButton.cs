using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class NodeButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public Image rect;

    public Color textColorWhenSelected;
    public Color rectColorMouseOver;

    private void Start()
    {
        rect.color = Color.clear;
        text.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        rect.DOColor(Color.clear, 0.1f);
        text.DOColor(Color.white, 0.1f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        rect.DOColor(Color.white, 0.1f);
        text.DOColor(textColorWhenSelected, 0.1f);

        rect.transform.DOComplete();
        rect.transform.DOPunchScale(Vector3.one / 3.0f, 0.2f, 20, 1.0f);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            rect.DOColor(rectColorMouseOver, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            rect.DOColor(Color.clear, 0.2f);
        }
    }
}
