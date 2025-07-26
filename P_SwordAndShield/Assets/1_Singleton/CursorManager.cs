using UnityEngine;

public static class CursorManager
{
    public static Camera Main;
    public static Vector2 GetCursorPositionInWorldSpace()
    {
        if (Main == null)
            Main = Camera.main;
        if (Main == null) return Vector2.zero;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Main.ScreenToWorldPoint(mousePosition);
        return new Vector2(worldPosition.x, worldPosition.y);
    }
}
