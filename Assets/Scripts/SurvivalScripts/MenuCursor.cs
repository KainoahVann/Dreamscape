public static class MenuCursor
{
    static int openCount = 0;

    public static void OnOpen()
    {
        openCount++;
        Apply();
    }

    public static void OnClose()
    {
        openCount = openCount > 0 ? openCount - 1 : 0;
        Apply();
    }

    public static void ForceClose()
    {
        openCount = openCount > 0 ? openCount - 1 : 0;
        Apply();
    }

    public static void Reset()
    {
        openCount = 0;
        Apply();
    }

    static void Apply()
    {
        bool anyOpen = openCount > 0;
        UnityEngine.Cursor.visible   = anyOpen;
        UnityEngine.Cursor.lockState = anyOpen
            ? UnityEngine.CursorLockMode.None
            : UnityEngine.CursorLockMode.Locked;
    }
}