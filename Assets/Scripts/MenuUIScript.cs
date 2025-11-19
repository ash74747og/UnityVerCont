using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic; // Added for List

public class MenuUIScript : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text highScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MainData.Instance != null)
        {
            List<MainData.PlayerScoreEntry> topScores = MainData.Instance.GetTopScores(3);
            string highScoresDisplay = "";

            foreach (MainData.PlayerScoreEntry entry in topScores)
            {
                highScoresDisplay += $"{entry.PlayerName} : {entry.Score}\n";
            }
            highScoreText.text = highScoresDisplay;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Guest"; // Default player name if input is empty
        }
        MainData.Instance.SetPlayerName(playerName);
        SceneManager.LoadScene(1); // Assuming scene 1 is the main game scene
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetPlayerName(string name)
    {
        MainData.Instance.SetPlayerName(name);
    }
}
