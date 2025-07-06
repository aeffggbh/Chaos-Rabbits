using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadScene(IScene.Index newScene)
    {
        StartCoroutine(LoadSceneCoroutine(newScene));
    }

    public IScene.Index GetActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        return (IScene.Index)activeScene.buildIndex;
    }

    public void UnloadAll()
    {
        foreach (var scene in sceneList)
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.UnloadSceneAsync(scene);

        sceneList.Clear();
    }

    public IEnumerator LoadSceneCoroutine(IScene.Index newScene)
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
        }
        else
        {
            Debug.LogWarning($"Loading {newScene} failed");
        }
    }

    public bool SetActiveScene(IScene.Index newScene)
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

    public void UnloadScene(IScene.Index newScene)
    {
        Scene scene = FindInList(newScene);

        if (scene.buildIndex != (int)newScene)
            return;

        UnloadScene(scene);
    }

    public Scene FindInList(IScene.Index newScene)
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

    public bool IsSceneLoaded(IScene.Index index)
    {
        foreach (var scene in sceneList)
        {
            bool isSameScene = scene.buildIndex == (int)index;
            if (isSameScene)
                return true;
        }
        return false;
    }

    public bool IsTypeLoaded<T>() where T : IScene
    {
        bool IsLoaded = false;

        foreach (var scene in sceneList)
        {
            if (typeof(T) == typeof(MenuScene))
                IsLoaded = scene.buildIndex == (int)MenuScene.MenuIndex;
            else
                IsLoaded = IsGameplay(scene.buildIndex);

            if (IsLoaded)
                return true;
        }

        return IsLoaded;
    }

    public void UnloadGameplay()
    {
        if (!IsTypeLoaded<GameplayScene>())
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
        return index >= (int)GameplayScene.Level1Index && index <= (int)GameplayScene.FinalLevelIndex;
    }
}
