using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelObjectiveHandler : MonoBehaviour
{
    ILevel level;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _objectiveText;
    [SerializeField] private string _defaultObjective = "Good luck.";
    private List<LevelMechanicSO> _mechanics;
    private string _startObjective = "OBJECTIVE: ";

    private void Start()
    {
        level = GetComponent<ILevel>();
        if (level == null)
            Debug.LogError("LevelObjectiveHandler requires a component that implements ILevel.");

        _mechanics = level.Mechanics;

        _levelText.text = "LEVEL " + (level.LevelIndex - 1).ToString();
    }

    private void Update()
    {
        _objectiveText.text = _startObjective;

        foreach (var mechanic in _mechanics)
            if (mechanic is IMechanicTextInfo)
            {
                if (!IsObjectiveDefault())
                    _objectiveText.text += ", ";
                _objectiveText.text += (mechanic as IMechanicTextInfo).GetObjectiveText();
            }

        if (IsObjectiveDefault())
            _objectiveText.text += _defaultObjective;
    }

    private bool IsObjectiveDefault()
    {
        return _objectiveText.text == _startObjective;
    }
}
