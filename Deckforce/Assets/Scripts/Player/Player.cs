using System.Collections;
using System.Collections.Generic;
using DrawSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public Character selectedCharacter;
    [SerializeField]public GameObject cardPrefab;

    public Deck deck;
    public GameObject discardPile;
    public GameObject hand;
    
    public List<Card> deckCards;
    public List<Card> discardedCards;
    public List<Card> handCards;

    private bool firstTurn = true;

    public void StartTurn()
    {
        foreach (Card card in deckCards)
        {
            var newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(deck.transform);
        }
        foreach (Card card in discardedCards)
        {
            var newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(discardPile.transform);
        }
        foreach (Card card in handCards)
        {
            var newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(hand.transform);
        }
        if (firstTurn)
        {
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
        foreach (Transform child in deck.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in discardPile.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in hand.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
