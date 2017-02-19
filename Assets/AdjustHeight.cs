using UnityEngine;
using UnityEngine.UI;

public class AdjustHeight : MonoBehaviour {
    private int spacing;
    RectTransform rectTransform;
    private void Start()
    {
        VerticalLayoutGroup vlg = GetComponent<VerticalLayoutGroup>();
        spacing = (int)vlg.spacing;
        rectTransform = GetComponent<RectTransform>();
    }
    private void LateUpdate ()
    {

        int updateHeight = transform.childCount * spacing;
        rectTransform.sizeDelta = new Vector2(0, updateHeight);
    }
}
