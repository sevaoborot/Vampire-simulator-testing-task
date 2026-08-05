using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenuElement : UIElement
{
    [Header("Level menu elements")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private LevelUpMenuButtons[] _weaponButtons;
    [Header("Weapons")]
    [SerializeField] private List<WeaponData> _weaponOptions = new List<WeaponData>(); //all possible weapons

    private NewWeaponSelector _newWeaponSelector;

    public override void Initialize(UIContext context)
    {
        base.Initialize(context);

        _eventBus.Subscribe<LevelChangedSignal>(ActivateLevelUpMenu);
        _newWeaponSelector = new NewWeaponSelector(context.eventBus, context.weapons);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        _eventBus.Unsubscribe<LevelChangedSignal>(ActivateLevelUpMenu);
    }

    private void ActivateLevelUpMenu(LevelChangedSignal signal)
    {
        SetTimeScale(0f);

        _mainPanel.SetActive(true);
        SetButtons();
    }

    public void DeactivateLevelUpMenu()
    {
        _mainPanel.SetActive(false);
        SetTimeScale(1f);
    }

    private void SetButtons()
    {
        for (int i = 0;  i < _weaponButtons.Length; i++)
        {
            WeaponData newButtonInfo = _newWeaponSelector.RandomData(_weaponOptions, out int level);
            _weaponButtons[i].description.text = newButtonInfo.weaponID;
            _weaponButtons[i].title.text = newButtonInfo.weaponLevels[level].levelDescription;
            _weaponButtons[i].button.onClick.RemoveAllListeners();
            _weaponButtons[i].button.onClick.AddListener(() => _newWeaponSelector.RegisterChosenWeapon(newButtonInfo));
        }
    }

    private void SetTimeScale(float timeScale) => Time.timeScale = timeScale; //should I change it to bool
}

[System.Serializable]
public struct LevelUpMenuButtons
{
    public Button button;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
}
