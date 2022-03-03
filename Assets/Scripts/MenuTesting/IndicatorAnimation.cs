using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IndicatorAnimation : MonoBehaviour
{

    public RectTransform rect;
    private Image img;
    private Vector2 origSize;
    [Space]
    public float duration;
    public float delay;

    void Start()
    {
        img = rect.GetComponent<Image>();
        img.DOFade(0.0f, 0.0f);

        origSize = rect.sizeDelta;
        rect.sizeDelta = origSize / 4.0f;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        Animate();
    }

    public void Animate()
    {
        Sequence s = DOTween.Sequence();
        s.Append(rect.DOSizeDelta(origSize, duration).SetEase(Ease.OutCirc));
        s.Join(img.DOFade(1.0f, duration / 3.0f));
        s.Join(img.DOFade(0.0f, duration / 4.0f).SetDelay(duration / 1.5f));
        s.SetLoops(-1);
    }
}
