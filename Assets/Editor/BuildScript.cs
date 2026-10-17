using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>命令行构建入口：Unity.exe -batchmode -quit -executeMethod BuildScript.BuildWindows</summary>
public static class BuildScript
{
    public static void BuildWindows()
    {
        string[] scenes =
        {
            "Assets/Scenes/StartPage.unity",
            "Assets/Scenes/GameScene.unity",
        };

        BuildPlayerOptions options = new()
        {
            scenes = scenes,
            locationPathName = "Builds/Windows/FallingBlock.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        Debug.Log($"[Build] result={report.summary.result}, size={report.summary.totalSize} bytes, " +
                  $"errors={report.summary.totalErrors}, warnings={report.summary.totalWarnings}");

        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
