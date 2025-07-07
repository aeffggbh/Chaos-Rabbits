using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ISceneData;

/// <summary>
/// Controller for managing scene loading and unloading
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private List<Scene> sceneList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        EventProvider.Subscribe<IExitGameEvent>(ExitGame);

        sceneList = new()
        {
            SceneManager.GetActiveScene()
        };
    }

    private void OnDestroy()
    {
        UnloadAll();
        EventProvider.Unsubscribe<IExitGameEvent>(ExitGame);
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public static void ExitGame(IExitGameEvent exit)
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public void LoadScene(int newScene)
    {
        StartCoroutine(LoadSceneCoroutine(newScene));
    }

    public int GetActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        return (int)activeScene.buildIndex;
    }

    public void UnloadAll()
    {
        foreach (var scene in sceneList)
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.UnloadSceneAsync(scene);

        sceneList.Clear();
    }

    public IEnumerator LoadSceneCoroutine(int newScene)
    {
        if (IsSceneLoaded(newScene))
            yield break;

        var loadScene = SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive);

        while (!loadScene.isDone)
        {
            Debug.Log($"Loading {newScene}...");
            Time.timeScale = 0f;
            yield return null;
        }

        Time.timeScale = 1f;

        var scene = SceneManager.GetSceneByBuildIndex((int)newScene);

        if (scene.isLoaded)
        {
            Debug.Log($"Loaded {newScene} successfully");

            sceneList.Add(scene);

            SceneLoader.Instance.SetActiveScene(newScene);
        }
        else
        {
            Debug.LogWarning($"Loading {newScene} failed");
        }
    }

    public bool SetActiveScene(int newScene)
    {
        Scene scene = FindInList(newScene);

        if (scene.buildIndex != (int)newScene)
            return false;

        SceneManager.SetActiveScene(scene);
        return true;
    }

    private void UnloadScene(Scene scene)
    {
        sceneList.Remove(scene);

        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Tried to unload a scene that is already unloaded. Did not do it. Scene state: {scene}");
            return;
        }

        SceneManager.UnloadSceneAsync(scene);
    }

    public void UnloadScene(int newScene)
    {
        Scene scene = FindInList(newScene);

        if (scene.buildIndex != (int)newScene)
            return;

        UnloadScene(scene);
    }

    public Scene FindInList(int newScene)
    {
        Scene _scene = new();

        foreach (var scene in sceneList)
        {
            _scene = scene;
            bool isSameScene = scene.buildIndex == (int)newScene;
            if (isSameScene)
                return scene;
        }

        Debug.LogWarning($"{newScene} is not loaded yet");
        return _scene;
    }

    public bool IsSceneLoaded(int index)
    {
        foreach (var scene in sceneList)
        {
            bool isSameScene = scene.buildIndex == (int)index;
            if (isSameScene)
                return true;
        }
        return false;
    }

    public bool IsTypeLoaded<T>() where T : ISceneData
    {
        bool IsLoaded = false;

        foreach (var scene in sceneList)
        {
            if (typeof(T) == typeof(MenuSceneData))
                IsLoaded = scene.buildIndex == MenuSceneData.Index;
            else
                IsLoaded = IsGameplay(scene.buildIndex);

            if (IsLoaded)
                return true;
        }

        return IsLoaded;
    }

    public void UnloadGameplay()
    {
        if (!IsTypeLoaded<GameplaySceneData>())
            return;

        var toUnloadList = new List<Scene>();

        foreach (var scene in sceneList)
        {
            if (IsGameplay(scene.buildIndex))
                toUnloadList.Add(scene);
        }

        for (int i = toUnloadList.Count - 1; i >= 0; i--)
        {
            UnloadScene(toUnloadList[i]);
            toUnloadList.Remove(toUnloadList[i]);
        }
    }

    public bool IsGameplay(int index)
    {
        return index >= (int)GameplaySceneData.Level1Index && index <= (int)GameplaySceneData.FinalLevelIndex;
    }

#if UNITY_EDITOR    
    public static int GetIndex(SceneAsset asset)
    {
        if (!asset)
            return 0;

        return SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(asset));
    }
#endif
}
