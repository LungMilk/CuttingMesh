using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{
    public UnityEvent cutfinished;

    public void called()
    {
        //Debug.Log("Cutting finished");
        cutfinished.Invoke();
    }

}

