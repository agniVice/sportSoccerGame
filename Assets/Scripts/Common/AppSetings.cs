using UnityEngine;

public class AppSetings : MonoBehaviour
{
    public static AppSetings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
