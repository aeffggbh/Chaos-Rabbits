using TMPro;
using UnityEngine;

public class CheatsLogger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cheatLogText;
    [SerializeField] private float _timeToBanish;
    [SerializeField] private float _currentTime;
    private string _cheatText;
    private bool _isTextShowing;

    private void Awake()
    {
        if (!_cheatLogText)
        {
            Debug.LogError(nameof(_cheatLogText) + " is not assigned in the inspector.");
            return;
        }

        _cheatLogText.text = "";
        _cheatText = "";

        if (_timeToBanish < 0.1)
            Debug.LogWarning("timeToBanish is set to a very low value, it might not work as expected.");
    }

    private void Update()
    {
        if (_isTextShowing && _currentTime < _timeToBanish)
        {
            _currentTime += Time.deltaTime;
            _cheatLogText.text = _cheatText;
            if (_currentTime > _timeToBanish)
            {
                _currentTime = 0f;
                _cheatLogText.text = "";
                _isTextShowing = false;
            }
        }
    }

    public void RequestText(string whatActive)
    {
        _cheatText = whatActive;
        _isTextShowing = true;
    }

    public void RequestText(string whatActive, bool toggle)
    {
        _cheatText = whatActive;

        if (toggle)
            _cheatText += " ON!";
        else
            _cheatText += " OFF!";

        _isTextShowing = true;
    }
}
