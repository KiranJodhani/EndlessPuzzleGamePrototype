using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; set; }
    public Text currentScoreLabel, highScoreLabel, currentScoreGameOverLabel, highScoreGameOverLabel;

    public float currentScore, highScore;
    // Start is called before the first frame update

    bool counting;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    //init and load highscore
    void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetFloat("HighScore", 0);

        highScore = PlayerPrefs.GetFloat("HighScore");

        UpdateHighScore();
        ResetCurrentScore();
    }

    //save and update highscore
    void UpdateHighScore()
    {
        if (currentScore > highScore)
            highScore = currentScore;

        highScoreLabel.text = highScore.ToString("F1");
        PlayerPrefs.SetFloat("HighScore", highScore);
    }

    //update currentscore
    public void UpdateScore(int value)
    {
        currentScore += value;
        Round(currentScore, 1);
        currentScoreLabel.text = currentScore.ToString("F1");
    }

    //reset current score
    public void ResetCurrentScore()
    {
        currentScore = 0;
        UpdateScore(0);
    }

    //update gameover scores
    public void UpdateScoreGameover()
    {
        UpdateHighScore();

        currentScoreGameOverLabel.text = currentScore.ToString("F1");
        highScoreGameOverLabel.text = highScore.ToString("F1");
    }

    public void StartCounting()
    {
        counting = true;
        StartCoroutine(Counter());
    }

    public void StopCounting()
    {
        counting = false;
        StopCoroutine(Counter());
    }

    IEnumerator Counter()
    {
        while (counting)
        {
            currentScore += .1f;
            Round(currentScore, 1);
            currentScoreLabel.text = currentScore.ToString("F1");
            yield return new WaitForSeconds(.1f);
        }
    }

    //round on 1 decimal, because sometimes float get more than one decimal
    public float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
