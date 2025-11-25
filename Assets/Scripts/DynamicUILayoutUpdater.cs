using UnityEngine;
using UnityEngine.UI;

public class DynamicUILayoutUpdater : MonoBehaviour
{
    public RectTransform boardRect;
    public RectTransform MenuRect;

    private void OnRectTransformDimensionsChange()
    {
        UpdateMyCustomLayouts();
    }

    private void Start()
    {
        UpdateMyCustomLayouts();
    }

    private void UpdateMyCustomLayouts()
    {
        if(Screen.width / Screen.height > 0)
        {
            // --- Apply Anchors
            MenuRect.anchorMin = new Vector2(0.5f, 0f);
            MenuRect.anchorMax = new Vector2(0.5f, 1f);
            // --- Apply Pivot
            MenuRect.pivot = new Vector2(0.5f, 0.5f);
            // --- Apply Position and Size ---
            MenuRect.anchoredPosition = new Vector2(-550f, 0f);
            MenuRect.sizeDelta = new Vector2(730f, 0f);
            // Setting Top and Bottom margins 
            MenuRect.offsetMin = new Vector2(MenuRect.offsetMin.x, 0f);
            MenuRect.offsetMax = new Vector2(MenuRect.offsetMax.x, 0f);


            // --- Apply Anchors
            boardRect.anchorMin = new Vector2(0.5f, 0f);
            boardRect.anchorMax = new Vector2(0.5f, 1f);
            // --- Apply Pivot
            boardRect.pivot = new Vector2(0.5f, 0.5f);
            // --- Apply Position and Size ---
            boardRect.anchoredPosition = new Vector2(375f, 0f);
            boardRect.sizeDelta = new Vector2(1030f, 0f);
            // Setting Top and Bottom margins 
            boardRect.offsetMin = new Vector2(boardRect.offsetMin.x, 50f);
            boardRect.offsetMax = new Vector2(boardRect.offsetMax.x, -50f);
        }
        else
        {
            // --- Apply Anchors
            MenuRect.anchorMin = new Vector2(0f, 0.5f);
            MenuRect.anchorMax = new Vector2(1f, 0.5f);
            // --- Apply Pivot
            MenuRect.pivot = new Vector2(0.5f, 0.5f);
            // --- Apply Position and Size ---
            MenuRect.anchoredPosition = new Vector2(0f, -900f);
            MenuRect.sizeDelta = new Vector2(0f, 1680f);
            // Setting Top and Bottom margins 
            MenuRect.offsetMin = new Vector2(0f, MenuRect.offsetMin.y);
            MenuRect.offsetMax = new Vector2(0f, MenuRect.offsetMax.y);



            // --- Apply Anchors:
            boardRect.anchorMin = new Vector2(0f, 0.5f);
            boardRect.anchorMax = new Vector2(1f, 0.5f);
            // --- Apply Pivot: Center (0.5, 0.5) ---
            boardRect.pivot = new Vector2(0.5f, 0.5f);
            // --- Apply Position and Size ---
            boardRect.anchoredPosition = new Vector2(0f, 850f);
            boardRect.sizeDelta = new Vector2(0f, 1680f);
            boardRect.offsetMin = new Vector2(90f, boardRect.offsetMin.y);
            boardRect.offsetMax = new Vector2(-90f, boardRect.offsetMax.y);

        }
    }
}