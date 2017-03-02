using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DynamicTransition : MonoBehaviour {
    public enum Trigger { NORMAL, HIGHLIGHTED, PRESSED };
    public Trigger trigger;
    Button button;
    private void Start()
    {
        this.button = GetComponent<Button>();
        if (this.button.transition != Selectable.Transition.Animation){
            Debug.Log("Not yet supported");
        }
    }
    public void setTransitionAnimation(string newTransition) {
        switch (trigger)
        {
            case Trigger.NORMAL:
                this.button.animationTriggers.normalTrigger = newTransition;
                break;
            case Trigger.HIGHLIGHTED:
                this.button.animationTriggers.highlightedTrigger = newTransition;
                break;
            case Trigger.PRESSED:
                this.button.animationTriggers.pressedTrigger = newTransition;
                break;
        }

    }
}
