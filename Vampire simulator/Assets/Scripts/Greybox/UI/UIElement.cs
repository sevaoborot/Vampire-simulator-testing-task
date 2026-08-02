using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    protected EventBus _eventBus;

    public virtual void Initialize(EventBus eventBus) //here we subscribe
    {
        _eventBus = eventBus;
    }

    //but where we unsubscribe?
}
