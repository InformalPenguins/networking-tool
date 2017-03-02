using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform target;
    private bool isMouseDown = false;
    private Vector3 startMousePosition;
    private Vector3 startPosition;

    public void OnPointerDown(PointerEventData dt)
    {
        isMouseDown = true;

        startPosition = target.position;
        startMousePosition = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData dt)
    {
        isMouseDown = false;
    }

    void Update()
    {
        if (isMouseDown)
        {
            Vector3 currentPosition = Input.mousePosition;
            Vector3 diff = currentPosition - startMousePosition;
            Vector3 pos = startPosition + diff;
            target.position = pos;
        }
    }
}