using TMPro;

public class LevelElement: UIElement
{
    private TextMeshProUGUI _levelText;

    public override void Initialize(EventBus eventBus)
    {
        base.Initialize(eventBus);

        _levelText = GetComponent<TextMeshProUGUI>();
        _eventBus.Subscribe<LevelChangedSignal>(GetLevelInfo);
    }

    private void GetLevelInfo(LevelChangedSignal signal)
    {
        UpdateElement(signal.Level);
    }

    private void UpdateElement(int level)
    {
        _levelText.text = $"Level: {level}";
    }
}
