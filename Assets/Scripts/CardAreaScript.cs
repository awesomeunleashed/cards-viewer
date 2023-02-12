using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardAreaScript : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Image cardPrefab;
    [SerializeField] Card[] spawnCards;

    // Not in love with this sprite storage approach, but it seemed simpler than doing something fancy with Resources.
    [SerializeField] Sprite[] heartsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] diamondsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] clubsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] spadesSprites = new Sprite[Util.RanksCount];

    List<Image> cards;
    float wrapLeftPosition;
    float wrapRightPosition;

    public void OnMoved(Vector2 position)
    {
        // WIP
        // TODO: Make the viewport exactly 4 cards wide so the math can be fixed
        Debug.Log($"Moved to {position}");
        if (position.x > wrapRightPosition)
        {
            Debug.Log("Wrapping Right");
            Image card = cards[0];
            cards.RemoveAt(0);
            cards.Add(card);
            card.transform.SetAsLastSibling();
            scrollRect.normalizedPosition = new Vector2(position.x - (1f / cards.Count), position.y);
        }
        else if (position.x < wrapLeftPosition)
        {
            Debug.Log("Wrapping Left");
            // Image card = cards[cards.Count - 1];
            // cards.RemoveAt(cards.Count - 1);
            // cards.Insert(0, card);
            // card.transform.SetAsFirstSibling();
            // card.transform.localPosition = new Vector3(-1, 0, 0);
        }
    }

    void Start()
    {
        Dictionary<Suit, Sprite[]> spritesBySuit = new()
        {
            { Suit.Hearts, heartsSprites },
            { Suit.Diamonds, diamondsSprites },
            { Suit.Clubs, clubsSprites },
            { Suit.Spades, spadesSprites },
        };

        cards = new List<Image>(spawnCards.Length);
        foreach (Card card in spawnCards.OrderBy(card => card.suit).ThenByDescending(card => card.rank))
        {
            Image image = Instantiate(cardPrefab, transform);
            image.name = $"{card.rank}{card.suit}";
            image.sprite = spritesBySuit[card.suit][(int)card.rank];
            cards.Add(image);
        }
        wrapLeftPosition = 3f / cards.Count;
        wrapRightPosition = 1 - wrapLeftPosition;
    }
}