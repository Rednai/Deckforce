using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    //TODO: a l'avenir, afficher les charactères que le joueur possède plutot que tous les characters existants
    CharactersManager charactersManager;

    public Character selectedCharacter;
    public bool isReady = false;
    [HideInInspector]
    public int characterIndex;

    public Text playerName;
    //public InputField playerName;

    [Header("Character Infos")]
    public Text characterName;
    public StatsSlider statsSlider;
    public Image characterIcon;
    public Image characterIconBackground;

    public GameObject selectionObjects;

    public GameObject readyDisplay;

    GameServer gameServer;

    // Start is called before the first frame update
    void Start()
    {
        characterIndex = 0;
        charactersManager = GameObject.FindObjectOfType<CharactersManager>();
        SetSelectedCharacter(false);
        gameServer = GameObject.FindObjectOfType<GameServer>();
    }

    public void IncreaseCharacterIndex()
    {
        characterIndex++;
        if (characterIndex >= charactersManager.existingCharacters.Count) {
            characterIndex = 0;
        }
        SetSelectedCharacter(true);
    }

    public void DecreaseCharacterIndex()
    {
        characterIndex--;
        if (characterIndex < 0) {
            characterIndex = charactersManager.existingCharacters.Count-1;
        }
        SetSelectedCharacter(true);
    }

    public void SetSelectedCharacter(bool isPackage)
    {
        selectedCharacter = charactersManager.existingCharacters[characterIndex];
        characterName.text = selectedCharacter.entityName;
        statsSlider.SetInfos(selectedCharacter, true);
        characterIcon.sprite = selectedCharacter.entityIcon;
        characterIconBackground.color = selectedCharacter.GetComponent<MeshRenderer>().sharedMaterial.color;

        if (isPackage) {
            List<Player> players = new List<Player>(GameObject.FindObjectsOfType<Player>());
            ChooseCharacter chooseCharacter = new ChooseCharacter();

            chooseCharacter.playerId = players.Find(x => x.isClient == true).id;
            chooseCharacter.characterId = selectedCharacter.id;
            if (gameServer == null) {
                gameServer = GameObject.FindObjectOfType<GameServer>();
            }
            gameServer.SendData(chooseCharacter);
        }
    }
}
