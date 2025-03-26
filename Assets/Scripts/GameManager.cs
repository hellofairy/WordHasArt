using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<WordRow> wordRows;
    public TextMeshProUGUI lifeText;
    public int lives = 5;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UpdateLifeUI();
    }

    public void CheckAnswer(WordRow row, string answer)
    {
        if (row.correctWord == answer)
        {
            row.RevealPixels();
        }
        else
        {
            lives--;
            UpdateLifeUI();
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }

    void UpdateLifeUI()
    {
        lifeText.text = "❤ " + lives;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }
}

public class WordRow : MonoBehaviour
{
    public string correctWord;
    public TextMeshProUGUI inputText;
    public List<Image> pixelImages;
    private string currentInput = "";

    public void AddLetter(char letter)
    {
        if (currentInput.Length < correctWord.Length)
        {
            currentInput += letter;
            inputText.text = currentInput;
        }
    }

    public void RemoveLetter()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            inputText.text = currentInput;
        }
    }

    public void SubmitWord()
    {
        GameManager.Instance.CheckAnswer(this, currentInput);
    }

    public void RevealPixels()
    {
        foreach (var pixel in pixelImages)
        {
            pixel.color = Color.white;
        }
    }
}

public class UIManager : MonoBehaviour
{
    public List<Button> keyboardButtons;
    public WordRow currentRow;

    void Start()
    {
        foreach (Button btn in keyboardButtons)
        {
            btn.onClick.AddListener(() => OnKeyPress(btn.GetComponentInChildren<TextMeshProUGUI>().text[0]));
        }
    }

    public void OnKeyPress(char letter)
    {
        currentRow.AddLetter(letter);
    }

    public void OnBackspace()
    {
        currentRow.RemoveLetter();
    }

    public void OnSubmit()
    {
        currentRow.SubmitWord();
    }
}
