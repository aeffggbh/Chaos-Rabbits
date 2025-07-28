using UnityEngine;

internal class LogMessageEvent : ILogMessageEvent
{
    private string _text;
    private GameObject _source;
    public string Text => _text;
    private bool _toggle;
    private bool _isToggle;

    public GameObject TriggeredByGO => _source;
    public bool IsToggle => _isToggle;
    public bool Toggle => _toggle;

    public LogMessageEvent(GameObject source, string text, bool toggle)
    {
        _text = text;
        _source = source;
        _toggle = toggle;
        _isToggle = true;
    }

    public LogMessageEvent(GameObject source, string text)
    {
        _text = text;
        _source = source;
        _toggle = false;
        _isToggle = false;
    }
}