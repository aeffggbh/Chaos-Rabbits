using System;
using TMPro;
using UnityEngine;

public class MessageLogger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private float _timeToBanish;
    [SerializeField] private float _currentTime;
    private string _text;
    private bool _isTextShowing;

    private void Awake()
    {
        if (!logText)
        {
            Debug.LogError(nameof(logText) + " is not assigned in the inspector.");
            return;
        }

        logText.text = "";
        _text = "";

        if (_timeToBanish < 0.1)
            Debug.LogWarning("timeToBanish is set to a very low value, it might not work as expected.");

        EventProvider.Subscribe<ILogMessageEvent>(RequestText);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<ILogMessageEvent>(RequestText);
    }


    private void Update()
    {
        if (_isTextShowing && _currentTime < _timeToBanish)
        {
            _currentTime += Time.deltaTime;
            logText.text = _text;
            if (_currentTime > _timeToBanish)
            {
                _currentTime = 0f;
                logText.text = "";
                _isTextShowing = false;
            }
        }
    }

    private void RequestText(ILogMessageEvent logMessage)
    {
        _text = logMessage.Text;
        _isTextShowing = true;

        if (logMessage.IsToggle)
        {
            if (logMessage.Toggle)
                _text += " ON!";
            else
                _text += " OFF!";
        }
        else
            _text += "!";

        _isTextShowing = true;
    }
}
