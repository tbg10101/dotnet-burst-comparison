using UnityEditor;
using UnityEditor.Build;

namespace Editor {
    public static class Build {
        private static readonly string[] Scenes = { "Assets/Scenes/Main.unity" };

        [MenuItem("Build/Windows")]
        private static void BuildWindows() {
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);

            BuildPipeline.BuildPlayer(
                Scenes,
                "Builds/Windows/Mono/UnityBench.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.StrictMode);

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.IL2CPP);

            BuildPipeline.BuildPlayer(
                Scenes,
                "Builds/Windows/IL2CPP/UnityBench.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.StrictMode);

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
        }
    }
}
