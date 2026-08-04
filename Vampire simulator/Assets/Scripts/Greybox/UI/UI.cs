using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private List<UIElement> _uiElements = new List<UIElement>();
    
    public void Initialize(EventBus eventBus, IPlayerWeaponsReader weapons)
    {
        UIContext context = new UIContext(eventBus, weapons);

        foreach (UIElement element in _uiElements) 
            element.Initialize(context);
    }
}

public readonly struct UIContext
{
    public readonly EventBus eventBus; //btw should I make eventbus readonly everywhere?
    public readonly IPlayerWeaponsReader weapons;

    public UIContext(EventBus eventBus, IPlayerWeaponsReader weapons)
    {
        this.eventBus = eventBus;
        this.weapons = weapons;
    }
}
