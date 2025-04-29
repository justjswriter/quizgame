using System;
using UnityEngine;

[Serializable]
public class Question
{
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ScrambledWord
    }

    public QuestionType Type;
    public string QuestionText;
    public string[] AnswerOptions;
    public string CorrectAnswer;
    public bool? CorrectAnswerBool;
    public string ScrambledWord;

    // Constructor for Multiple Choice
    public Question(string questionText, string[] answerOptions, int correctAnswerIndex)
    {
        Type = QuestionType.MultipleChoice;
        QuestionText = questionText;
        AnswerOptions = answerOptions;
        CorrectAnswer = answerOptions[correctAnswerIndex];
    }

    // Constructor for True/False
    public Question(string questionText, bool correctAnswer)
    {
        Type = QuestionType.TrueFalse;
        QuestionText = questionText;
        CorrectAnswerBool = correctAnswer;
    }

    // Constructor for Scrambled Word
    public Question(string scrambledWord, string correctAnswer)
    {
        Type = QuestionType.ScrambledWord;
        ScrambledWord = scrambledWord;
        CorrectAnswer = correctAnswer;
    }
}
