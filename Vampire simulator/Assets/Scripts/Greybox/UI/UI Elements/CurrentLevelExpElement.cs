using TMPro;

public class CurrentLevelExpElement : UIElement
{
    private TextMeshProUGUI _currentExpText;

    public override void Initialize(UIContext context)
    {
        base.Initialize(context);

        _currentExpText = GetComponent<TextMeshProUGUI>();
        _eventBus.Subscribe<CurrentLevelExpChangedSignal>(GetExpInfo);
    }

    private void GetExpInfo(CurrentLevelExpChangedSignal signal)
    {
        UpdateElement(signal.CurrentExp);
    }

    private void UpdateElement(float exp)
    {
        _currentExpText.text = $"EXP: {exp}";
    }
}
