using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip failSound;

    [Header("UI Elements")]
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text gameOverText;
    public TMP_Text answerFeedback;
    public Button restartButton;
    public Button mainMenuButton;
    public TMP_InputField inputField;
    public TMP_Text correctAnswerText; 

    private int score = 0;
    private int currentQuestionIndex = 0;
    private int currentLevel = 1;
    private float timer = 20f;

    private List<Question> questions = new List<Question>();
    private List<Question> level1Questions = new List<Question>();
    private List<Question> level2Questions = new List<Question>();
    private List<Question> level3Questions = new List<Question>();

    private void Start()
    {
        ValidateUI();
        InitializeQuestions();
        SetupUI();
        currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1); 
        LoadQuestionsForLevel();
    }

    private void Update()
    {
        if (currentQuestionIndex < questions.Count && currentLevel <= 3)
        {
            timer -= Time.deltaTime;
            timerText.text = $"Time: {Mathf.Ceil(timer)}";

            if (timer <= 0)
            {
                timer = 20f;
                NextQuestion();
            }
        }
    }

    private void ValidateUI()
    {
        if (answerButtons == null || answerButtons.Length == 0 || inputField == null || correctAnswerText == null)
        {
            Debug.LogError("UI components are not set properly!");
        }
    }

    private void SetupUI()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
        questionText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        answerFeedback.gameObject.SetActive(true);
        correctAnswerText.gameObject.SetActive(false); 

        answerFeedback.text = "";
    }

    private void InitializeQuestions()
    {
        level1Questions = new List<Question>
        {
            new Question("What is the capital of Japan?", new[] { "Seoul", "Beijing", "Tokyo", "Kyoto" }, 2),
            new Question("Which country has the largest land area?", new[] { "USA", "Canada", "China", "Russia" }, 3),
            new Question("What is the longest river in the world?", new[] { "Amazon", "Nile", "Yangtze", "Mississippi" }, 1),
            new Question("Which continent is Egypt located on?", new[] { "Asia", "Africa", "Europe", "North America" }, 1),
            new Question("Mount Everest is located in which country?", new[] { "India", "Nepal", "China", "Pakistan" }, 1)
        };

        level2Questions = new List<Question>
        {
            new Question("The Sahara Desert is located in Asia.", false),
            new Question("The Amazon River is located in South America.", true),
            new Question("The capital of France is Berlin.", false),
            new Question("The Great Wall of China is in Japan.", false),
            new Question("Australia is both a country and a continent.", true)
        };

        level3Questions = new List<Question>
        {
            new Question("ARTQA", "QATAR"),
            new Question("PEGYT", "EGYPT"),
            new Question("AIIND", "INDIA"),
            new Question("OKOTY", "KYOTO")
        };
    }

    private void LoadQuestionsForLevel()
    {
        questions.Clear();

        switch (currentLevel)
        {
            case 1:
                questions.AddRange(level1Questions);
                break;
            case 2:
                questions.AddRange(level2Questions);
                break;
            case 3:
                questions.AddRange(level3Questions);
                break;
        }

        timer = 20f;
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            currentLevel++;
            currentQuestionIndex = 0;

            if (currentLevel > 3)
            {
                EndGame();
                return;
            }

            LoadQuestionsForLevel();
            return;
        }

        Question current = questions[currentQuestionIndex];

        switch (current.Type)
        {
            case Question.QuestionType.MultipleChoice:
                SetupMultipleChoice(current);
                break;
            case Question.QuestionType.TrueFalse:
                SetupTrueFalse(current);
                break;
            case Question.QuestionType.ScrambledWord:
                SetupScrambled(current);
                break;
        }
    }

    private void SetupMultipleChoice(Question question)
    {
        ResetButtons();
        questionText.text = question.QuestionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(i < question.AnswerOptions.Length);

            if (i < question.AnswerOptions.Length)
            {
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = question.AnswerOptions[i];

                int choice = i;
                answerButtons[i].onClick.AddListener(() => EvaluateAnswer(question.AnswerOptions[choice], question.CorrectAnswer));
            }
        }
    }

    private void SetupTrueFalse(Question question)
    {
        ResetButtons();
        questionText.text = question.QuestionText;

        answerButtons[0].gameObject.SetActive(true);
        answerButtons[1].gameObject.SetActive(true);

        answerButtons[0].GetComponentInChildren<TMP_Text>().text = "True";
        answerButtons[1].GetComponentInChildren<TMP_Text>().text = "False";

        answerButtons[0].onClick.AddListener(() => EvaluateAnswer(true, question.CorrectAnswerBool == true));
        answerButtons[1].onClick.AddListener(() => EvaluateAnswer(false, question.CorrectAnswerBool == false));
    }

    private void SetupScrambled(Question question)
    {
        ResetButtons();
        inputField.gameObject.SetActive(true);
        questionText.text = $"Unscramble: {question.ScrambledWord}";

        answerButtons[0].gameObject.SetActive(true);
        answerButtons[0].GetComponentInChildren<TMP_Text>().text = "Submit";

        answerButtons[0].onClick.AddListener(() => EvaluateAnswer(inputField.text.Trim().ToUpper(), question.CorrectAnswer.ToUpper()));
    }

    private void EvaluateAnswer(object userAnswer, object correctAnswer)
    {
        bool isCorrect = userAnswer.Equals(correctAnswer);
        answerFeedback.text = isCorrect ? "Correct!" : "Wrong!";
        answerFeedback.color = isCorrect ? Color.green : Color.red;

        if (!isCorrect)
        {
            correctAnswerText.text = $"Correct Answer: {correctAnswer}";
            correctAnswerText.gameObject.SetActive(true);
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(isCorrect ? winSound : failSound);
        }

        if (isCorrect) score++;
        scoreText.text = $"Score: {score}";

        StartCoroutine(NextAfterDelay());
    }

    private IEnumerator NextAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        answerFeedback.text = "";
        correctAnswerText.gameObject.SetActive(false); 
        inputField.text = "";
        inputField.gameObject.SetActive(false);
        NextQuestion();
    }

    private void ResetButtons()
    {
        foreach (var btn in answerButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.interactable = true;
            btn.gameObject.SetActive(false);
        }
    }

    private void NextQuestion()
    {
        currentQuestionIndex++;
        timer = 20f;
        DisplayQuestion();
    }

    private void EndGame()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = $"Game Over! Final Score: {score}";

        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        questionText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        answerFeedback.gameObject.SetActive(false);
        correctAnswerText.gameObject.SetActive(false); 

        foreach (var btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        timer = 0f;
    }

    public void RestartGame()
    {
        score = 0;
        currentLevel = 1;
        currentQuestionIndex = 0;

        InitializeQuestions();
        SetupUI();
        LoadQuestionsForLevel();
        timer = 20f;
        DisplayQuestion();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
