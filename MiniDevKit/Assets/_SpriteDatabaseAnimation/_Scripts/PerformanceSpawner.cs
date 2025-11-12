using UnityEngine;
using System.Collections.Generic;
using SpriteDatabaseAnimation;
public class PerformanceSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject[] prefabs;
    [SerializeField] int waveCount = 10;
    [SerializeField] float waveInterval = 2f;
    [SerializeField] int maxObjects = 100;

    [Header("Spawn Area")]
    [SerializeField] float spawnRadius = 10f;
    [SerializeField] Transform spawnCenter;

    [Header("UI Settings")]
    [SerializeField] bool showStats = true;
    [SerializeField] int fontSize = 30;
    [SerializeField] Color textColor = Color.white;
    [SerializeField] Vector2 position = new Vector2(10, 10);

    List<GameObject> spawnedObjects = new List<GameObject>();
    float waveTimer;
    bool isSpawning = false;

    // FPS hesaplama
    float deltaTime = 0f;

    void Start()
    {
        if (spawnCenter == null)
            spawnCenter = transform;
    }

    void Update()
    {
        // FPS hesapla
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (!isSpawning)
            return;

        waveTimer += Time.deltaTime;

        if (waveTimer >= waveInterval)
        {
            waveTimer = 0;
            SpawnWave();
        }
    }

    void OnGUI()
    {
        if (!showStats)
            return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = fontSize;
        style.normal.textColor = textColor;

        // Gölge efekti için
        GUIStyle shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = Color.black;

        float fps = 1.0f / deltaTime;
        string text = $"FPS: {Mathf.Ceil(fps)}\nObjects: {spawnedObjects.Count}";

        Rect rect = new Rect(position.x, position.y, w, h);
        Rect shadowRect = new Rect(position.x + 2, position.y + 2, w, h);

        // Önce gölge
        GUI.Label(shadowRect, text, shadowStyle);
        // Sonra yazı
        GUI.Label(rect, text, style);
    }

    public void StartSpawning()
    {
        isSpawning = true;
        waveTimer = 0;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ClearAll()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();
        isSpawning = false;
    }

    void SpawnWave()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("Prefab listesi boş!");
            return;
        }

        int toSpawn = Mathf.Min(waveCount, maxObjects - spawnedObjects.Count);

        if (toSpawn <= 0)
        {
            Debug.Log("Maximum obje sayısına ulaşıldı!");
            StopSpawning();
            return;
        }

        for (int i = 0; i < toSpawn; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = spawnCenter.position + new Vector3(randomPos.x, randomPos.y, 0);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            spawnedObjects.Add(obj);

            var animator = obj.GetComponent<SpriteDatabaseAnimator>();
            if (animator != null)
            {
                string[] availableCategories = animator.GetAvailableCategories();
                if (availableCategories != null && availableCategories.Length > 0)
                {
                    string randomCategory = availableCategories[Random.Range(0, availableCategories.Length)];
                    animator.SetCategory(randomCategory);
                }
            }
        }

        Debug.Log($"Dalga spawn edildi! Toplam: {spawnedObjects.Count}/{maxObjects}");
    }

    void OnDrawGizmosSelected()
    {
        Transform center = spawnCenter != null ? spawnCenter : transform;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center.position, spawnRadius);
    }
    public void ChangeAllStates(string newState)
    {
        int changedCount = 0;

        foreach (var obj in spawnedObjects)
        {
            if (obj == null)
                continue;

            var animator = obj.GetComponent<SpriteDatabaseAnimator>();
            if (animator != null)
            {
                animator.SetCategory(newState);
                changedCount++;
            }
        }

        Debug.Log($"{changedCount} objenin state'i '{newState}' olarak değiştirildi!");
    }


    public void ChangeAllStatesToRandom()
    {
        int changedCount = 0;

        foreach (var obj in spawnedObjects)
        {
            if (obj == null)
                continue;

            var animator = obj.GetComponent<SpriteDatabaseAnimator>();
            if (animator != null)
            {
                string[] availableCategories = animator.GetAvailableCategories();
                if (availableCategories != null && availableCategories.Length > 0)
                {
                    string randomCategory = availableCategories[Random.Range(0, availableCategories.Length)];
                    animator.SetCategory(randomCategory);
                    changedCount++;
                }
            }
        }

        Debug.Log($"{changedCount} objenin state'i rastgele değiştirildi!");
    }

}
