using TMPro;

public class LevelElement: UIElement
{
    private TextMeshProUGUI _levelText;

    public override void Initialize(UIContext context)
    {
        base.Initialize(context);

        _levelText = GetComponent<TextMeshProUGUI>();
        _eventBus.Subscribe<LevelChangedSignal>(GetLevelInfo);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        _eventBus.Unsubscribe<LevelChangedSignal>(GetLevelInfo);
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
