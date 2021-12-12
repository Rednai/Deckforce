using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Player ownerPlayer;
    public Card card;
    
    public Image cardLayout;
    public Image visual;
    public Text description;
    public Text cardName;
    public Text cardCost;
    public Color color;

    public AudioClip activateClip;
    public AudioClip selectClip;
    public AudioClip selectBackClip;
    public AudioClip cannotDoClip;

    public void InitiateCard(Player currentPlayer, Card card)
    {
        ownerPlayer = currentPlayer;
        this.card = card;

        visual.sprite = card.visual;
        description.text = card.description;
        cardName.text = card.cardName;
        cardCost.text = $"{card.cost}";
        activateClip = card.activateClip;
        cannotDoClip = card.cannotClip;
        cardLayout.color = card.color;
    }
}
