using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Transform toTrack;

    [SerializeField]
    private List<Biome> biomes;
    private int atBiome = 0;

    private List<Level> toLoadLevels = new List<Level>(); // These are prefabs
    private List<Level> currentLevels = new List<Level>(); // These are instantiated objects

    [SerializeField]
    private float levelChangeDistance = 10.0f;

    [SerializeField]
    private float levelDeleteDistance = 10.0f;

    public static LevelLoader Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Already anohter level loader");
        }
        Instance = this;
    }

    public void Begin(Transform player)
    {
        enabled = true;
        toTrack = player;
        atBiome = 0;
        LoadNextLevel();
    }

    public void DeleteAllLevels()
    {
        foreach (Level level in currentLevels)
        {
            Destroy(level.gameObject);
        }
        currentLevels.Clear();
    }

    private void LoadNextLevel()
    {
        if (toLoadLevels.Count == 0)
        {
            LoadNextBiome();
            return;
        }

        Level lastLevel = currentLevels.Count == 0 ? null : currentLevels[0];
        Level levelPrefab = toLoadLevels[0];

        Vector3 pos;
        if (lastLevel != null)
        {
            pos = lastLevel.transform.position + new Vector3(0, lastLevel.Size.y, 0);
        }
        else
        {
            pos = toTrack.position + new Vector3(0.0f, 20.0f, 0.0f);
        }

        Level createdLevel = Instantiate(levelPrefab, pos, Quaternion.identity);
        toLoadLevels.RemoveAt(0);
        currentLevels.Insert(0, createdLevel);
    }

    private void LoadNextBiome()
    {
        if (atBiome >= biomes.Count)
        {
            Debug.Log("No more biomes");
            enabled = false;
            return;
        }
        Debug.Log("Loading biome " + atBiome);

        toLoadLevels.AddRange(biomes[atBiome].Levels);
        RandomUtil.Shuffle(toLoadLevels);
        atBiome++;

        LoadNextLevel();
    }

    private void Update()
    {
        if (toTrack == null)
        {
            return;
        }

        CheckLevels();
    }

    private void CheckLevels()
    {
        if (currentLevels.Count == 0)
        {
            return;
        }

        Level maxLevel = currentLevels.Count == 0 ? null : currentLevels[0];
        float maxLevelPosY = maxLevel.transform.position.y + maxLevel.Size.y;
        if (maxLevelPosY - toTrack.position.y < levelChangeDistance)
        {
            LoadNextLevel();
        }

        Level minLevel = currentLevels[currentLevels.Count - 1];
        float minLevelPosY = minLevel.transform.position.y + minLevel.Size.y;
        if (minLevelPosY - toTrack.position.y < -levelDeleteDistance)
        {
            currentLevels.RemoveAt(currentLevels.Count - 1);
            Destroy(minLevel.gameObject);
        }
    }
}
