using UnityEngine;

public class Quitter : MonoBehaviour  {
    private void Update() {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
