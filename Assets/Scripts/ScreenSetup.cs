using UnityEngine;

/// <summary>
/// 运行时强制窗口化。不依赖 Player Settings，防止构建或导入时设置被重置。
/// RuntimeInitializeOnLoadMethod 会在场景加载前自动执行，不需要挂到任何对象上。
/// </summary>
public static class ScreenSetup
{
    private const int Width = 1280;
    private const int Height = 720;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Apply()
    {
#if !UNITY_EDITOR
        Screen.SetResolution(Width, Height, FullScreenMode.Windowed);
#endif
    }
}
