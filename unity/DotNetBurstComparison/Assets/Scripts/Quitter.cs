using UnityEngine;

/// <summary>
/// Exits the player or exits edit mode during the first frame.
///
/// This is useful for creating a console application which is supposed to exit when the main thread runs out of work to do.
/// </summary>
public class Quitter : MonoBehaviour  {
    private void LateUpdate() {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
