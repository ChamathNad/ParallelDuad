using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool shuffle = true;    
    public RectTransform boardRect;
    public RectTransform modeRect;
    public GridLayoutGroup grid;
    public int seed = 10;

    public Sprite[] cardFronts;
    public Sprite cardBack;
    public GameModes[] modes;
    public GameObject CardPrefab;


    private List<Card> allCards = new ();
    private Queue<Card> flipQueue = new ();
    private List<Card> faceUpUnprocessed = new ();

    private int score = 0;
    private int flip = 0;
    private int combo = 0;
    private int currmode = 3;


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

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.UpdateMode(modes[currmode].name, modes[currmode].icon);
        setGameModeUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGameModeUI()
    {
        GameObject Template = modeRect.GetChild(0).gameObject;


        for (int i = 0; i < modes.Length; i++)
        {
            int modeIndex = i;
            var obj = Instantiate(Template, modeRect).GetComponent<Button>();
            obj.onClick.AddListener(() => SetMode(modeIndex));
            obj.targetGraphic.GetComponent<Image>().sprite = modes[modeIndex].icon;
        }

        Template.SetActive(false);
    }

    public void SetMode(int i)
    {
        currmode = i;
        UIManager.Instance.UpdateMode(modes[i].name, modes[i].icon);
    }


    public void StartNewGame()
    {
        seed = UnityEngine.Random.Range(1, 100);

        score = 0;
        flip = 0;
        combo = 0;

        UIManager.Instance.UpdateFlips(flip);
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);

        ApplyLayout(modes[currmode].row, modes[currmode].col);
        ClearBoard();
        CreateBoard();
    }

    public void ClearBoard()
    {
        foreach (Transform t in boardRect) 
            Destroy(t.gameObject);
        allCards.Clear();
        faceUpUnprocessed.Clear();
        flipQueue.Clear();
    }

    public void CreateBoard()
    {
        int total = modes[currmode].row * modes[currmode].col;
        if (total % 2 != 0)
        {
            Debug.LogError("Total must be even");
            return;
        }

        List<int> ids = new List<int>();
        int pairCount = total / 2;
        for (int i = 0; i < pairCount; i++) { ids.Add(i); ids.Add(i); }

        System.Random rng = new(seed);

        if (shuffle)
            ids = ids.OrderBy(x => rng.Next()).ToList();

        for (int i = 0; i < total; i++)
        {
            var card = Instantiate(CardPrefab, boardRect).GetComponent<Card>();
            
            int assignedId = ids[i];
            card.id = assignedId;
            var fcard = cardFronts[assignedId % cardFronts.Length];
            card.SetCard(fcard, cardBack, assignedId);

            card.OnFlipComplete += OnCardFaceUp;
            allCards.Add(card);
        }
    }
    void OnCardFaceUp(Card card)
    {        
        faceUpUnprocessed.Add(card);
        flip++;
        UIManager.Instance.UpdateFlips(flip);
        StartCoroutine(ProcessComparisons());
    }

    IEnumerator ProcessComparisons()
    {        
        while (true)
        {
            var candidates = faceUpUnprocessed.Where(c => c.State == CardState.FaceUp).ToList();
            if (candidates.Count >= 2)
            {
                var a = candidates[0];
                var b = candidates[1];
                a.MarkProcessing();
                b.MarkProcessing();

                if (a.id == b.id)
                {
                    AudioManager.Instance.PlayMatch();
                    a.SetMatched();
                    b.SetMatched();
                    score += CalculateScore();
                    UIManager.Instance.UpdateScore(score);
                    faceUpUnprocessed.Remove(a);
                    faceUpUnprocessed.Remove(b);
                }
                else
                {
                    AudioManager.Instance.PlayMismatch();
                    yield return new WaitForSeconds(0.5f);
                    a.FlipInvert();
                    b.FlipInvert();
                    faceUpUnprocessed.Remove(a);
                    faceUpUnprocessed.Remove(b);
                    combo = 0;
                    UIManager.Instance.UpdateCombo(combo);
                }

                yield return null;
            }
            else
            {
                var filtered = allCards.Where(c => c.State != CardState.Matched).ToList();
                if (filtered.Count == 0)
                {
                    yield return new WaitForSeconds(1f);
                    AudioManager.Instance.PlayGameOver();
                    UIManager.Instance.GameEndMessage(score, flip);
                }
                break;
            }
        }
    }

    private int CalculateScore()
    {
        combo++;
        UIManager.Instance.UpdateCombo(combo);
        return 100 * combo;
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveState(
            new SaveData {
               cardIds = allCards.Select(c => c.id).ToArray(),
               matched = allCards.Select(c => c.State == CardState.Matched).ToArray(),
               score = score,
               flips = flip,
               combo = combo,
               mode = currmode,
               seed = seed,
             }
         );

        UIManager.Instance.SendUIMessage("Game Saved Successfully!");
    }

    public void LoadGame()
    {
        var state = SaveManager.Instance.LoadState();
        if (state == null) return;

        seed = state.seed;
        score = state.score;
        flip = state.flips;
        combo = state.combo;
        UIManager.Instance.UpdateFlips(flip);
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
        currmode = state.mode;
        ApplyLayout(modes[currmode].row, modes[currmode].col);
        ClearBoard();
        CreateBoard();

        for (int i = 0; i < Mathf.Min(allCards.Count, state.matched.Length); i++)
            if (state.matched[i]) allCards[i].SetMatched(true);

        UIManager.Instance.SendUIMessage("Game Loaded Successfully!");
    }

    public void ApplyLayout(int r, int c)
    {
        float paddingX = grid.padding.left + grid.padding.right;
        float paddingY = grid.padding.top + grid.padding.bottom;
        float spacingX = grid.spacing.x * (modes[currmode].col - 1);
        float spacingY = grid.spacing.y * (modes[currmode].row - 1);
        float cellW = (boardRect.rect.width - paddingX - spacingX) / modes[currmode].col;
        float cellH = (boardRect.rect.height - paddingY - spacingY) / modes[currmode].row;
        float size = Mathf.Floor(Mathf.Min(cellW, cellH));
        grid.cellSize = new Vector2(size * 70/100f, size);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = modes[currmode].col;
    }
}

[Serializable]
public struct GameModes
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int row;
    [SerializeField]
    public int col;
    [SerializeField]
    public Sprite icon;
}