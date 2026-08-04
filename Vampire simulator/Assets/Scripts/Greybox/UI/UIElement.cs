using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    protected EventBus _eventBus;

    public virtual void Initialize(UIContext context) //here we subscribe
    {
        _eventBus = context.eventBus;
    }

    //but where we unsubscribe?
}
