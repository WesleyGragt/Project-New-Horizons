using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public UnityEvent enterTriggerEvent;
    public UnityEvent exitTriggerEvent;

    public void EnterTrigger()
    {
        enterTriggerEvent.Invoke();
    }

    public void exitTrigger()
    {
        exitTriggerEvent.Invoke();
    }

}
