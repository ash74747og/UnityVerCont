using UnityEngine;
using System.IO;
using System.Collections.Generic; // Required for List
using System.Linq; // Required for LINQ operations like OrderByDescending and Take

public class MainData : MonoBehaviour
{
    public static MainData Instance { get; private set; }
    public string PlayerName { get; private set; }

    // Class to hold a single player's name and their high score
    [System.Serializable]
    public class PlayerScoreEntry
    {
        public string PlayerName;
        public int Score;

        public PlayerScoreEntry(string name, int score)
        {
            PlayerName = name;
            Score = score;
        }
    }

    // Serializable class to hold the list of high scores
    [System.Serializable]
    class SaveData
    {
        public List<PlayerScoreEntry> HighScores = new List<PlayerScoreEntry>();
    }

    private List<PlayerScoreEntry> m_HighScores = new List<PlayerScoreEntry>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScoreData(); // Load high score when MainData is initialized
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public void AddOrUpdatePlayerHighScore(int newScore)
    {
        PlayerScoreEntry existingEntry = m_HighScores.FirstOrDefault(entry => entry.PlayerName == PlayerName);

        if (existingEntry != null)
        {
            if (newScore > existingEntry.Score)
            {
                existingEntry.Score = newScore;
            }
        }
        else
        {
            m_HighScores.Add(new PlayerScoreEntry(PlayerName, newScore));
        }

        m_HighScores = m_HighScores.OrderByDescending(entry => entry.Score).ToList();
        // Optionally, limit the number of stored high scores
        if (m_HighScores.Count > 10) // Store top 10 scores
        {
            m_HighScores = m_HighScores.Take(10).ToList();
        }

        SaveHighScoreData(); // Save high score when it's updated
    }

    public List<PlayerScoreEntry> GetTopScores(int count)
    {
        return m_HighScores.Take(count).ToList();
    }

    void SaveHighScoreData()
    {
        SaveData data = new SaveData();
        data.HighScores = m_HighScores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    void LoadHighScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_HighScores = data.HighScores;
        }
    }
}
