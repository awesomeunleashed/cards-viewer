using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardAreaScript : MonoBehaviour
{
    [SerializeField] Image cardPrefab;
    [SerializeField] Card[] spawnCards;

    // Not in love with this sprite storage approach, but it seemed simpler than doing something fancy with Resources.
    [SerializeField] Sprite[] heartsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] diamondsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] clubsSprites = new Sprite[Util.RanksCount];
    [SerializeField] Sprite[] spadesSprites = new Sprite[Util.RanksCount];

    HorizontalLayoutGroup layout;
    RectTransform rect;
    int cardWidth;
    float wrapRightX;
    float wrapLeftX;

    public void OnMoved()
    {
        if (rect.offsetMin.x < wrapRightX - layout.padding.left)
        {
            rect.GetChild(0).SetAsLastSibling();
            layout.padding.left += cardWidth;
        }
        else if (rect.offsetMin.x > wrapLeftX - layout.padding.left)
        {
            rect.GetChild(rect.childCount - 1).SetAsFirstSibling();
            layout.padding.left -= cardWidth;
        }
    }

    void Start()
    {
        layout = GetComponent<HorizontalLayoutGroup>();
        rect = GetComponent<RectTransform>();
        Dictionary<Suit, Sprite[]> spritesBySuit = new()
        {
            { Suit.Hearts, heartsSprites },
            { Suit.Diamonds, diamondsSprites },
            { Suit.Clubs, clubsSprites },
            { Suit.Spades, spadesSprites },
        };

        foreach (Card card in spawnCards.OrderBy(card => card.suit).ThenByDescending(card => card.rank))
        {
            Image image = Instantiate(cardPrefab, transform);
            image.sprite = spritesBySuit[card.suit][(int)card.rank];
        }

        // Storing cardWidth as an int because padding.left is an int
        cardWidth = (int)cardPrefab.rectTransform.sizeDelta.x;
        float viewportWidth = GetComponentInParent<ScrollRect>().viewport.rect.width;
        float maskedWidth = (cardWidth * spawnCards.Length) - viewportWidth;
        wrapRightX = cardWidth - maskedWidth;
        wrapLeftX = -cardWidth;
    }
}