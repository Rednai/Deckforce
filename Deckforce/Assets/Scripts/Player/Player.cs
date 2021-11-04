using System.Collections;
using System.Collections.Generic;
using DrawSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public Character selectedCharacter;
    [SerializeField]
    public CardDisplay cardPrefab;

    public Deck deck;
    public GameObject discardPile;
    public GameObject hand;
    
    public List<Card> deckCards;
    public List<Card> discardedCards;
    public List<Card> handCards;

    private bool firstTurn = true;
    
    public void InstanciateDeckCards()
    {
        foreach (Card card in deckCards)
        {
            CardDisplay newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.card = card;
            newCard.InitiateCard(this);
            newCard.transform.SetParent(deck.transform);
        }
    }

    private void ManagingActivation(bool activating)
    {
        foreach (Transform child in deck.transform)
        {
            CardDisplay card = child.GetComponent<CardDisplay>();
            if (card.ownerPlayer == this)
            {
                card.gameObject.SetActive(activating);
            }
        }
        foreach (Transform child in discardPile.transform)
        {
            CardDisplay card = child.GetComponent<CardDisplay>();
            if (card.ownerPlayer == this)
            {
                card.gameObject.SetActive(activating);
            }
        }
        foreach (Transform child in hand.transform)
        {
            CardDisplay card = child.GetComponent<CardDisplay>();
            if (card.ownerPlayer == this)
            {
                card.gameObject.SetActive(activating);
            }
        }
    }
    
    public void StartTurn()
    {
        ManagingActivation(true);
        if (firstTurn)
        {
            InstanciateDeckCards();
            deck.Shuffle();
            deck.Draw();
            deck.Draw();
            deck.Draw();
            deck.Draw();
            firstTurn = false;
        }
        else
        {
            deck.Draw();
        }
    }
    
    public void EndTurn()
    {
        ManagingActivation(false);
    }
}
