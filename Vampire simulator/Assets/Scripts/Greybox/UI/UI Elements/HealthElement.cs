using TMPro;

public class HealthElement : UIElement
{
    private TextMeshProUGUI _healthText;

    public override void Initialize(UIContext context)
    {
        base.Initialize(context);

        _healthText = GetComponent<TextMeshProUGUI>();
        _eventBus.Subscribe<HealthChangedSignal>(GetHealthInfo);
    }

    private void GetHealthInfo(HealthChangedSignal signal)
    {
        UpdateElement(signal.Health);
    }

    private void UpdateElement(float health)
    {
        _healthText.text = $"HP: {health}";
    }
}
