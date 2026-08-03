using UnityEngine;

public class LevelUpMenuElement : UIElement
{
    [SerializeField] private GameObject _mainPanel;

    public override void Initialize(EventBus eventBus)
    {
        base.Initialize(eventBus);

        _eventBus.Subscribe<LevelChangedSignal>(ActivateLevelUpMenu);
    }

    private void ActivateLevelUpMenu(LevelChangedSignal signal)
    {
        _mainPanel.SetActive(true);
        SetTimeScale(0f);
    }

    public void DeactivateLevelUpMenu()
    {
        _mainPanel.SetActive(false);
        SetTimeScale(1f);
    }

    private void SetTimeScale(float timeScale) => Time.timeScale = timeScale; //should I change it to bool
}
