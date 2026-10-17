using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI bestText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject spawner;

    private int score;
    private int lives = 3;
    private int bestScore;
    private const string BestKey = "best_score";
    
    void Awake()
    {
        Instance = this;
        bestScore = PlayerPrefs.GetInt(BestKey, 0);
        bestText.text = "History Best: " + bestScore;
        gameOverPanel.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    public void OnCatch()
    {
        scoreText.text = "Current Score: " + ++score;
        if (score > bestScore)
        {
            bestScore = score;
            bestText.text = "History Best: " + bestScore;
            PlayerPrefs.SetInt(BestKey, bestScore);
            PlayerPrefs.Save();
        }
    }

    public void OnBlockDestroy()
    {
        // Debug.Log("Block Destroyed");
        livesText.text = "Remain Lives: " + --lives;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnRestart()
    {
        Debug.Log("[GameManager] OnRestart");
        for (int i = 0; i < spawner.transform.childCount; i++)
        {
            Destroy(spawner.transform.GetChild(i).gameObject);
        }
        score = 0;
        lives = 3;
        scoreText.text = "Current Score: 0";
        livesText.text = "Remain Lives: 3";
        gameOverPanel.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        spawner.GetComponent<Spawner>().OnRestart();
        Time.timeScale = 1;
    }
}
