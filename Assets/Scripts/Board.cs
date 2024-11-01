using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]{
        KeyCode.A,KeyCode.B,KeyCode.C,KeyCode.D,KeyCode.E,KeyCode.F,
        KeyCode.G,KeyCode.H,KeyCode.I,KeyCode.J,KeyCode.K,KeyCode.L,
        KeyCode.M,KeyCode.N,KeyCode.O,KeyCode.P,KeyCode.Q,KeyCode.R,
        KeyCode.S,KeyCode.T,KeyCode.U,KeyCode.V,KeyCode.X,KeyCode.Y,
        KeyCode.Z,KeyCode.W,
    };

    private Row[] rows;
    private int rowIndex;
    private int columnIndex;
    private ResourcesLoader.WordData wordData;
    public string word { get; set; }

    [Header("States")]
    public Tile.State emptyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;

    [Header("UI")]
    public TextMeshProUGUI invalidWordText;
    public Button tryAgainButton;
    public Button newGameButton;
    public ScoreKeeper scoreKeeper;
    public Suggestion suggestion;
    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        Tile.InitializeState(emptyState, occupiedState, correctState, wrongSpotState, correctState);
    }
    private IEnumerator Start()
    {
        HideGameUIElements();
        wordData = new ResourcesLoader.WordData();
        yield return StartCoroutine(LoadData());
        NewGame();
    }
    void Update()
    {

        HandleRowInput();
    }
    public void NewGame()
    {
        ClearBoard();
        SetRandomWord();
        ResetGame();
        enabled = true;
    }
    public void TryAgain()
    {
        scoreKeeper.SetScore(0);
        ResetGame();
        ClearBoard();
        enabled = true;
    }
    public void ResetGame()
    {
        scoreKeeper.GetScore();
        suggestion.SuggestTextClear();
        rowIndex = 0;
        columnIndex = 0;
    }
    void HideGameUIElements()
    {
        tryAgainButton.gameObject.SetActive(false);
        invalidWordText.gameObject.SetActive(false);
    }
    public IEnumerator LoadData()
    {
        yield return StartCoroutine(ResourcesLoader.LoadDataCoroutine(wordData));
    }
    public void SuggestCharacter()
    {
        suggestion.RandomCharacter(word);
    }
    public void SetRandomWord()
    {
        word = wordData.solutions[Random.Range(0, wordData.solutions.Length)];
        word = word.ToLower().Trim();
    }

    private void HandleRowInput()
    {
        Row currentRow = rows[rowIndex];
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleBackspaceInput(currentRow);
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    columnIndex++;
                    break;
                }
            }
        }
    }
    private void HandleBackspaceInput(Row currentRow)
    {
        columnIndex = Mathf.Max(columnIndex - 1, 0);

        currentRow.tiles[columnIndex].SetLetter('\0');
        currentRow.tiles[columnIndex].SetState(emptyState);

        invalidWordText.gameObject.SetActive(false);
    }
    public void ClearBoard()
    {
        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].tiles.Length; col++)
            {
                rows[row].tiles[col].SetLetter('\0');
                rows[row].tiles[col].SetState(emptyState);
            }
        }
    }
    public bool IsvalidWord(string word)
    {
        for (int i = 0; i < wordData.validWords.Length; i++)
        {
            if (wordData.validWords[i] == word)
            {
                return true;
            }
        }
        return false;
    }
    public bool HasWon(Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].state != correctState)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator WaitForSeconds(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        NewGame();
    }
    public void SubmitRow(Row row)
    {
        if (!IsvalidWord(row.word))
        {
            invalidWordText.gameObject.SetActive(true);
            return;
        }
        EvaluateRow(row);

        IncreaseRow();

        if (HasWon(row))
        {
            scoreKeeper.IncrementScore();
            StartCoroutine(WaitForSeconds(1.0f));
        }

        if (rowIndex >= rows.Length)
        {
            enabled = false;
        }
    }
    private void IncreaseRow()
    {
        rowIndex++;
        columnIndex = 0;
    }
    private void EvaluateRow(Row row)
    {
        string remaining = word;

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            ProcessTile(tile, i, word);

            if (word[i] == tile.letter)
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1).Insert(i, " ");
            }
            else if (!word.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
        }

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            if (tile.state != correctState && tile.state != incorrectState && remaining.Contains(tile.letter))
            {
                tile.SetState(wrongSpotState);
                int index = remaining.IndexOf(tile.letter);
                remaining = remaining.Remove(index, 1).Insert(index, " ");
            }
        }
    }
    private void ProcessTile(Tile tile, int index, string word)
    {
        char userGuess = tile.letter;
        char actualChar = word[index];

        TileAnimator animator = tile.gameObject.GetComponent<TileAnimator>();
        StartCoroutine(animator.FlipTile(userGuess, actualChar));
    }
    private void OnEnable()
    {
        tryAgainButton.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        tryAgainButton.gameObject.SetActive(true);
    }
}