using UnityEngine;

public static class Program {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Main() {
        Debug.Log("Hello, World!");
    }
}
