using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneData
{
    public ESceneType sceneType;
    [ScenePath] public string scene;
}

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private List<SceneData> _sceneDatas;

    private Dictionary<ESceneType, string> _dict;

    public static SceneHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Start()
    {
        _dict = new Dictionary<ESceneType, string>();
        foreach (SceneData sceneData in _sceneDatas)
        {
            _dict.Add(sceneData.sceneType, sceneData.scene);
        }
    }
    
    public string GetScenePath(ESceneType sceneType)
    {
        return _dict[sceneType];
    }

    public void LoadScene(ESceneType sceneType)
    {
        SceneManager.LoadScene(_dict[sceneType]);
    }
}
