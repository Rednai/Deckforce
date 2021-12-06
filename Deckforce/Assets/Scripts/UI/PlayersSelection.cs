using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersSelection : MonoBehaviour
{
    public PlayerSelection playerSelectionTemplate;
    public GameObject playersParent;
    public Player playerTemplate;

    public Dictionary<PlayerSelection, Player> selectedPlayers;
    List<string> playersNames;

    public CardsManager cardsManager;

    GameServer gameServer;

    void Start()
    {
        selectedPlayers = new Dictionary<PlayerSelection, Player>();
        playersNames = new List<string>();

        gameServer = GameServer.FindObjectOfType<GameServer>();
    }

    //TODO: a l'avenir on recevra un joueur ou jsp quoi pour instancier les infos
    public void AddPlayer()
    {
        if (selectedPlayers.Count == 5) {
            return ;
        }

        Player newPlayer = Instantiate(playerTemplate);

        //TODO: set l'id et le isClient du joueur par rapport à un packet recu par le serveur

        newPlayer.deckCards = new List<Card>();

        for (int i = 0; i != 30; i++) {
            //Card newCard = Instantiate(cardsManager.cards[Random.Range(0, cardsManager.cards.Count)]);
            newPlayer.deckCards.Add(cardsManager.cards[Random.Range(0, cardsManager.cards.Count)]);
        }

        PlayerSelection newPlayerSelection = Instantiate(playerSelectionTemplate);

        newPlayerSelection.transform.SetParent(playersParent.transform);
        newPlayerSelection.transform.localScale = new Vector3(1, 1, 1);

        if (newPlayer.isClient) {
            newPlayerSelection.selectionObjects.SetActive(true);
        }
        newPlayerSelection.playerName.text = newPlayer.id;

        selectedPlayers.Add(newPlayerSelection, newPlayer);
    }

    public void RemovePlayer(PlayerSelection playerSelection)
    {
    }

    public void StartPlaying()
    {
        foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
            pair.Value.playerName = pair.Key.playerName.text;
            Character playerCharacter = Instantiate(pair.Key.selectedCharacter);
            playerCharacter.transform.SetParent(pair.Value.transform);
            pair.Value.name = pair.Key.playerName.text;
            pair.Value.selectedCharacter = playerCharacter;
            playerCharacter.gameObject.SetActive(false);
            DontDestroyOnLoad(pair.Value.gameObject);
        }
        if (gameServer == null) {
            gameServer = GameServer.FindObjectOfType<GameServer>();
        }
        ChooseCharacter chooseCharacter = new ChooseCharacter();
        //TODO: faire la recherche plus proprement
        KeyValuePair<PlayerSelection, Player> player = GetPlayer();
        chooseCharacter.characterId = player.Key.selectedCharacter.id;
        chooseCharacter.playerId = player.Value.id;
        gameServer.SendData(chooseCharacter);
        /*
        if (selectedPlayers.Count < 2) {
            return ;
        }

        //Instancier les characteres et les mettre en enfants du joueur
        if (AreUsernamesValids()) {
            foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
                pair.Value.playerName = pair.Key.playerName.text;
                Character playerCharacter = Instantiate(pair.Key.selectedCharacter);
                playerCharacter.transform.SetParent(pair.Value.transform);
                pair.Value.name = pair.Key.playerName.text;
                pair.Value.selectedCharacter = playerCharacter;
                playerCharacter.gameObject.SetActive(false);
                DontDestroyOnLoad(pair.Value.gameObject);
            }
            SceneManager.LoadScene("BattleScene");
        }
        */
    }

    KeyValuePair<PlayerSelection, Player> GetPlayer()
    {
        foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
            if (pair.Value.isClient == true) {
                return (pair);
            }
        }
        return (new KeyValuePair<PlayerSelection, Player>());
    }

    bool AreUsernamesValids()
    {
        List<string> playersNames = new List<string>();
        foreach (KeyValuePair<PlayerSelection, Player> player in selectedPlayers) {
            //TODO: faudra aussi regarder si la string contient pas que des espaces
            if (player.Key.playerName.text == "") {
                return (false);
            }
            playersNames.Add(player.Key.playerName.text);
        }
        foreach (string playerName in playersNames) {
            if (!IsNameUsed(playerName)) {
                return (false);
            }
        }
        return (true);
    }

    bool IsNameUsed(string name)
    {
        int nameCount = 0;

        foreach (string playerName in playersNames) {
            if (playerName == name) {
                nameCount++;
            }
        }
        if (nameCount > 1) {
            return (false);
        }
        return (true);
    }
}
