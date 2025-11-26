using UnityEngine;
using UnityEngine.UI;

public class DynamicUILayoutUpdater : MonoBehaviour
{
    [System.Serializable]
    public struct RectPreset
    {
        [SerializeField]
        public Vector2 AnchorMin;
        [SerializeField]
        public Vector2 AnchorMax;
        [SerializeField]
        public Vector2 Pivot;
        [SerializeField]
        public Vector2 Position;
        [SerializeField]
        public Vector2 Size;
        [SerializeField]
        public Vector2 PaddingTop;
        [SerializeField]
        public Vector2 PaddingBottom;
        [SerializeField]
        public Vector2 scale;


    }

    public RectPreset Landscape;
    public RectPreset Portrait;

    private RectTransform boardRect;
    private float oldValue;

    private void Awake()
    {
        boardRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateMyCustomLayouts();
    }


    private void FixedUpdate()
    {
        if (boardRect == null)
            return;

        var newvalue = (float)Screen.width / Screen.height;

        if (oldValue != newvalue)
        {
            oldValue = newvalue;
            UpdateMyCustomLayouts();
        }
    }

    private void UpdateMyCustomLayouts()
    {
        if(Screen.width / Screen.height > 0)
        {
            boardRect.anchorMin = Landscape.AnchorMin;
            boardRect.anchorMax = Landscape.AnchorMax;
            boardRect.pivot = Landscape.Pivot;
            boardRect.anchoredPosition = Landscape.Position;
            boardRect.sizeDelta = Landscape.Size;

            boardRect.offsetMin += Landscape.PaddingTop;
            boardRect.offsetMax += Landscape.PaddingBottom;
            //boardRect.offsetMin = new Vector2(boardRect.offsetMin.x, Landscape.PaddingTop.y);
            //boardRect.offsetMax = new Vector2(boardRect.offsetMax.x, Landscape.PaddingBottom.y);
            boardRect.localScale = Landscape.scale;
        }
        else
        {
            // --- Apply Anchors:
            boardRect.anchorMin = Portrait.AnchorMin;
            boardRect.anchorMax = Portrait.AnchorMax;
            boardRect.pivot = Portrait.Pivot;
            boardRect.anchoredPosition = Portrait.Position;
            boardRect.sizeDelta = Portrait.Size;
            boardRect.offsetMin += Portrait.PaddingTop;
            boardRect.offsetMax += Portrait.PaddingBottom;
            //boardRect.offsetMin = new Vector2(Portrait.PaddingTop.x, boardRect.offsetMin.y);
            //boardRect.offsetMax = new Vector2(Portrait.PaddingBottom.x, boardRect.offsetMax.y);
            boardRect.localScale = Portrait.scale;

        }
    }

}