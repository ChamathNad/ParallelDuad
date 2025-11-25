using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int[] cardIds;
    public bool[] matched;
    public float score;
    public int flips;
    public int combo;
    public int mode;
    public int seed;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    string filePath => Path.Combine(Application.persistentDataPath, "save.json");

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveState(SaveData data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public SaveData LoadState()
    {
        if (!File.Exists(filePath)) 
            return null;
        var json = File.ReadAllText(filePath);
        try
        {
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch
        {
            UIManager.Instance.SendUIMessage("File Cannot be Loaded!"); 
            return null;
        }
    }
}