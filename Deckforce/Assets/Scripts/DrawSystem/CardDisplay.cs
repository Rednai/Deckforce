using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Player ownerPlayer;
    public Card card;
    
    public int cost;
    public int playerRange;
    public int effectRange;

    public Image cardLayout;
    public Image visual;
    public Text description;
    public Text cardName;
    public Color color;

    public AudioClip activateClip;

    public void InitiateCard(Player currentPlayer)
    {
        ownerPlayer = currentPlayer;
        cost = card.cost;
        playerRange = card.playerRange;
        effectRange = card.effectRange;
        visual.sprite = card.visual; 
        description.text = card.description;
        cardName.text = card.name;
        activateClip = card.activateClip;
        cardLayout.color = card.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
