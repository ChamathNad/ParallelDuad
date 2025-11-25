using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool shuffle = true;
    public int rows = 4;
    public int cols = 4;
    public RectTransform boardRect;
    public GridLayoutGroup grid;

    public Sprite[] cardFronts;
    public Sprite cardBack;
    public GameObject CardPrefab;


    private List<Card> allCards = new ();
    private Queue<Card> flipQueue = new ();
    private List<Card> faceUpUnprocessed = new ();

    private int score = 0;


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
        StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame(int? seed = null)
    {
        score = 0;
        //RESET UI SCORE HERE
        ApplyLayout(rows, cols);
        ClearBoard();
        CreateBoard(seed);
    }

    public void ClearBoard()
    {
        foreach (Transform t in boardRect) 
            Destroy(t.gameObject);
        allCards.Clear();
        faceUpUnprocessed.Clear();
        flipQueue.Clear();
    }

    public void CreateBoard(int? seed)
    {
        int total = rows * cols;
        if (total % 2 != 0)
        {
            Debug.LogError("Total must be even");
            return;
        }

        List<int> ids = new List<int>();
        int pairCount = total / 2;
        for (int i = 0; i < pairCount; i++) { ids.Add(i); ids.Add(i); }

        System.Random rng = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
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
                    //SFX;
                    a.SetMatched();
                    b.SetMatched();
                    score += CalculateScore(true);
                    //UPDATE SCORE
                    faceUpUnprocessed.Remove(a);
                    faceUpUnprocessed.Remove(b);
                }
                else
                {
                    //SFX;
                    yield return new WaitForSeconds(0.5f);
                    a.FlipInvert();
                    b.FlipInvert();
                    faceUpUnprocessed.Remove(a);
                    faceUpUnprocessed.Remove(b);
                    //COMBO RESET
                    score += CalculateScore(false);
                    //UPDATE SCORE
                }

                yield return null;
            }
            else
            {
                break;
            }
        }
    }

    int CalculateScore(bool matched)
    {
        //SOME COMBO LOGIC
        if (matched)
            return 100; 
        else 
            return 0;
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }

    public void ApplyLayout(int r, int c)
    {
        rows = r; cols = c;
        float paddingX = grid.padding.left + grid.padding.right;
        float paddingY = grid.padding.top + grid.padding.bottom;
        float spacingX = grid.spacing.x * (cols - 1);
        float spacingY = grid.spacing.y * (rows - 1);
        float cellW = (boardRect.rect.width - paddingX - spacingX) / cols;
        float cellH = (boardRect.rect.height - paddingY - spacingY) / rows;
        float size = Mathf.Floor(Mathf.Min(cellW, cellH));
        grid.cellSize = new Vector2(size * 70/100f, size);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;
    }
}
