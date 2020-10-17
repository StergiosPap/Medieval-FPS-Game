using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutPanel : MonoBehaviour
{
    private bool isFaded = false;
    public float Duration = 0.4f;
    public GameObject panel;

    void Start()
    {
        panel.SetActive(true);
        Fade();
    }

    public void Fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, isFaded ? 1 : 0));

        isFaded = true;
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);

            yield return null;
        }

       panel.SetActive(false);

    }
}
