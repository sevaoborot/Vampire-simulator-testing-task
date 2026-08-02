using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private List<UIElement> _uiElements = new List<UIElement>();

    public void Initialize(EventBus eventBus)
    {
        foreach (UIElement element in _uiElements) 
            element.Initialize(eventBus);
    }
}
