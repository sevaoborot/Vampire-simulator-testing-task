using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    protected EventBus _eventBus;

    public virtual void Initialize(UIContext context) 
    {
        _eventBus = context.eventBus;
    }

    public virtual void OnDestroy() { }
}
