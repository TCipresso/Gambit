using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public enum GameState
    {
        PlayerTurn,
        AITurn,
        RoundEnd
    }

    //State machine stuff

    //UI variables
    public GameState currentState;
    public Button rollButton;
    public Button higherButton;
    public Button lowerButton;
    public Button betButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerRollText;
    public TextMeshProUGUI aiRollText;
    public TextMeshProUGUI playerTokensText;
    public TextMeshProUGUI aiTokensText;
    public TextMeshProUGUI currentBetText;

    //basic variables
    private int playerDiceRoll;
    private int aiDiceRoll;
    private int playerTokens = 10;
    private int aiTokens = 10;
    private int currentBet = 1;

    //references
    public DiceRoller diceRoller;

    private void Start()
    {
        rollButton.onClick.AddListener(diceRoller.RollDice);
        rollButton.onClick.AddListener(OnRollDice);
        higherButton.onClick.AddListener(() => OnPlayerGuess(true));
        lowerButton.onClick.AddListener(() => OnPlayerGuess(false));
        betButton.onClick.AddListener(OnBet);

        currentState = GameState.PlayerTurn;
        ResetRollTexts();
        DisableGuessButtons();
        DisableBetButton();
        UpdateTokenDisplays();
        currentBetText.text = "Current Bet: " + currentBet;
    }

    private void OnRollDice()
    {
        RollDice();
        currentBet = 1;
        resultText.text = "Guess Higher or Lower.";
        EnableGuessButtons();
        EnableBetButton();
        UpdateTokenDisplays();
        currentBetText.text = "Current Bet: " + currentBet;
    }



    private void OnPlayerGuess(bool isGuessHigher)
    {
        UpdateRollTexts(playerDiceRoll.ToString(), aiDiceRoll.ToString());
        bool isGuessCorrect = EvaluateGuess(isGuessHigher);
        resultText.text = isGuessCorrect ? "You guessed right!" : "You guessed wrong!";
        ResolveBet(isGuessCorrect);
        DisableButtons();
        currentState = GameState.AITurn;
        StartCoroutine(HandleAITurn());
    }

    private void OnBet()
    {
        if (currentBet < 3 && playerTokens > 0 && aiTokens > 0)
        {
            currentBet++;
            currentBetText.text = "Current Bet: " + currentBet;
        }
    }
    private void DeductBetFromTokens()
    {
        // Deduct the bet from both player and AI
        playerTokens -= currentBet;
        aiTokens -= currentBet;
        currentBetText.text = "Current Bet: " + currentBet;
    }


    private IEnumerator HandleAITurn()
    {
        currentBet = 1; // Initialize the bet to 1 for the buy-in

        RollDice();
        yield return new WaitForSeconds(3); // AI "thinking" delay

        // AI decides whether to increase the bet
        if (currentBet < 3 && aiTokens > 0)
        {
            currentBet++;
            UpdateTokenDisplays();
        }

        bool aiGuessIsHigher = Random.value > 0.5f;
        UpdateRollTexts(playerDiceRoll.ToString(), aiDiceRoll.ToString());
        bool aiGuessCorrect = EvaluateGuess(aiGuessIsHigher);

        resultText.text = aiGuessCorrect ? "AI guessed right!" : "AI guessed wrong!";
        ResolveBet(aiGuessCorrect);

        currentState = GameState.RoundEnd;
        StartCoroutine(HandleRoundEnd());
    }

    private void DeductInitialBet()
    {
        // Deduct the initial bet from both player and AI
        playerTokens--;
        aiTokens--;
    }

    private IEnumerator HandleRoundEnd()
    {
        yield return new WaitForSeconds(2);
        ResetGame();
    }

    private void ResetGame()
    {
        resultText.text = "Player's turn. Roll the dice.";
        EnableButtons();
        currentState = GameState.PlayerTurn;
        ResetRollTexts();
    }

    private void RollDice()
    {
        playerDiceRoll = Random.Range(1, 21);
        aiDiceRoll = Random.Range(1, 21);
    }

    private bool EvaluateGuess(bool isGuessHigher)
    {
        return (isGuessHigher && playerDiceRoll > aiDiceRoll) || (!isGuessHigher && playerDiceRoll < aiDiceRoll);
    }

    private void UpdateRollTexts(string playerRoll, string aiRoll)
    {
        playerRollText.text = "Your Roll: " + playerRoll;
        aiRollText.text = "AI Roll: " + aiRoll;
    }

    private void ResetRollTexts()
    {
        playerRollText.text = "Your Roll: ?";
        aiRollText.text = "AI Roll: ?";
    }

    private void ResolveBet(bool playerWon)
    {
        int pot = currentBet * 2;
        if (playerWon)
        {
            playerTokens += pot - currentBet;
            aiTokens -= currentBet;
            Debug.Log("AI lost the round. Pot was " + pot + " tokens.");
        }
        else
        {
            aiTokens += pot - currentBet;
            playerTokens -= currentBet;
            Debug.Log("AI won the round. Pot was " + pot + " tokens.");
        }

        currentBet = 1;
        UpdateTokenDisplays();
        currentBetText.text = "Current Bet: " + currentBet;
    }


    private void UpdateTokenDisplays()
    {
        playerTokensText.text = "Your Tokens: " + playerTokens;
        aiTokensText.text = "AI Tokens: " + aiTokens;
    }

    private void EnableGuessButtons()
    {
        higherButton.gameObject.SetActive(true);
        lowerButton.gameObject.SetActive(true);
    }

    private void DisableGuessButtons()
    {
        higherButton.gameObject.SetActive(false);
        lowerButton.gameObject.SetActive(false);
    }

    private void EnableBetButton()
    {
        betButton.gameObject.SetActive(true);
    }

    private void DisableBetButton()
    {
        betButton.gameObject.SetActive(false);
    }

    private void DisableButtons()
    {
        rollButton.gameObject.SetActive(false);
        DisableGuessButtons();
        DisableBetButton();
    }

    private void EnableButtons()
    {
        rollButton.gameObject.SetActive(true);
    }
}
