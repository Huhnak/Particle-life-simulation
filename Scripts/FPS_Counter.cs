using System.Collections;
using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FPSCounter());
    }

    private IEnumerator FPSCounter()
    {
        for (; ; )
        {
            var fps = 1 / Time.deltaTime;
            if (fps < 10) _text.color = Color.red;
            else if (fps < 30) _text.color = Color.yellow;
            else _text.color = Color.green;
            _text.text = $"FPS: {(fps).ToString("0.00")}";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
