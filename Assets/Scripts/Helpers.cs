using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }


    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsCursorOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }
}