using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersSelection : MonoBehaviour
{
    [Header("UI")]
    public PlayerSelection playerSelectionTemplate;
    public GameObject playersParent;
    public Player playerTemplate;

    [Header("Players")]
    public Dictionary<PlayerSelection, Player> selectedPlayers = new Dictionary<PlayerSelection, Player>();

    [Header("Offline")]
    public GameObject addPlayerButton;
    Dictionary<int, string> playerNames = new Dictionary<int, string>();
    Dictionary<int, string> namesCopy;
    public int playerIndex = 0;

    [Header("Start Battle")]
    public Button readyButton;
    public Color unreadyColor;
    public Color readyColor;
    public string unreadyText;
    public string readyText;

    public CardsManager cardsManager;

    public MatchmakingButtons matchmakingButtons;

    void Start()
    {
        playerNames.Add(0, "Strider");
        playerNames.Add(1, "Reaver");
        playerNames.Add(2, "Warlock");
        playerNames.Add(3, "Floral Wisp");
        playerNames.Add(4, "LeapFrog");
        namesCopy = new Dictionary<int, string>(playerNames);
    }

    public void AddOfflinePlayer()
    {
        if (selectedPlayers.Count == 5) {
            return ;
        }

        Player newPlayer = Instantiate(playerTemplate);
        newPlayer.id = CreateRandomId();
        newPlayer.team = namesCopy.First().Key;
        newPlayer.isClient = true;
        newPlayer.username = namesCopy.First().Value;
        namesCopy.Remove(namesCopy.First().Key);

        newPlayer.deckCards = new List<Card>();
        for (int i = 0; i != 30; i++) {
            newPlayer.deckCards.Add(Instantiate(cardsManager.cards[Random.Range(0, cardsManager.cards.Count)]));
        }

        PlayerSelection newPlayerSelection = Instantiate(playerSelectionTemplate);
        newPlayerSelection.transform.SetParent(playersParent.transform);
        newPlayerSelection.transform.localScale = new Vector3(1, 1, 1);
        newPlayerSelection.playersSelection = this;
        newPlayerSelection.removePlayerButton.gameObject.SetActive(true);
        GameServer.instance.isOffline = true;

        newPlayerSelection.selectionObjects.SetActive(true);
        newPlayerSelection.playerName.text = newPlayer.username;

        selectedPlayers.Add(newPlayerSelection, newPlayer);

        if (selectedPlayers.Count >= 2) {
            readyButton.gameObject.SetActive(true);
        }
    }

    string CreateRandomId()
    {
        char[] characters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        string newId = "";

        for (int i = 0; i != 10; i++) {
            newId += characters[Random.Range(0, characters.Length-1)];
        }
        return (newId);
    }

    //TODO: a l'avenir on recevra un joueur ou jsp quoi pour instancier les infos
    public void AddPlayer(PlayerJoin player)
    {
        if (selectedPlayers.Count == 5) {
            return ;
        }

        Player newPlayer = Instantiate(playerTemplate);
        newPlayer.id = player.id;
        newPlayer.team = player.team;
        newPlayer.isClient = player.isClient;
        newPlayer.username = player.username;

        //TODO: set l'id et le isClient du joueur par rapport à un packet recu par le serveur

        newPlayer.deckCards = new List<Card>();

        for (int i = 0; i != 30; i++) {
            //Card newCard = Instantiate(cardsManager.cards[Random.Range(0, cardsManager.cards.Count)]);
            newPlayer.deckCards.Add(Instantiate(cardsManager.cards[Random.Range(0, cardsManager.cards.Count)]));
        }

        PlayerSelection newPlayerSelection = Instantiate(playerSelectionTemplate);

        newPlayerSelection.transform.SetParent(playersParent.transform);
        newPlayerSelection.transform.localScale = new Vector3(1, 1, 1);

        if (newPlayer.isClient) {
            newPlayerSelection.selectionObjects.SetActive(true);
        }
        newPlayerSelection.playerName.text = newPlayer.username;

        selectedPlayers.Add(newPlayerSelection, newPlayer);
    }

    public void RemovePlayer(PlayerSelection playerSelection)
    {
        Player playerToRemove = selectedPlayers[playerSelection];
        selectedPlayers.Remove(playerSelection);

        namesCopy.Add(playerToRemove.team, playerNames[playerToRemove.team]);
        Destroy(playerToRemove.gameObject);
        Destroy(playerSelection.gameObject);
        if (selectedPlayers.Count < 2) {
            readyButton.gameObject.SetActive(false);
        }
    }

    public void GoBackToMenu()
    {
        matchmakingButtons.gameObject.SetActive(true);
        while (selectedPlayers.Count > 0) {
            RemovePlayer(selectedPlayers.First().Key);
        }
        gameObject.SetActive(false);
    }

    public void StartPlaying()
    {
        if (!GameServer.instance.isOffline) {
            KeyValuePair<PlayerSelection, Player> player = selectedPlayers.Single(x => x.Value.isClient == true);
        
            if (player.Key.isReady) {
                readyButton.GetComponent<Image>().color = unreadyColor;
                readyButton.transform.GetChild(0).GetComponent<Text>().text = unreadyText;
                player.Key.readyDisplay.SetActive(false);
            } else {
                readyButton.GetComponent<Image>().color = readyColor;
                readyButton.transform.GetChild(0).GetComponent<Text>().text = readyText;
                player.Key.readyDisplay.SetActive(true);
            }
            player.Key.isReady = !player.Key.isReady;

            if (!GameServer.instance.isOffline) {
                PlayerReady playerReady = new PlayerReady();
                playerReady.playerId = player.Value.id;
                GameServer.instance.SendData(playerReady);
            }
            if (CheckIfAllReady()) {
                SetupCharacters();
            }
        } else {
            SetupCharacters();
        }
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

    KeyValuePair<PlayerSelection, Player> GetPlayer(string playerId)
    {
        foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
            if (pair.Value.id == playerId) {
                return (pair);
            }
        }
        return (new KeyValuePair<PlayerSelection, Player>());
    }

    public void SetPlayerCharacter(ChooseCharacter chooseCharacter)
    {
        KeyValuePair<PlayerSelection, Player> pair = selectedPlayers.Single(x => x.Value.id == chooseCharacter.playerId);
        //KeyValuePair<PlayerSelection, Player> pair = GetPlayer(chooseCharacter.playerId);
        pair.Key.selectedCharacter = CharactersManager.instance.existingCharacters.Find(x => x.id == chooseCharacter.characterId);
        pair.Key.characterIndex = CharactersManager.instance.existingCharacters.IndexOf(pair.Key.selectedCharacter);
        pair.Key.SetSelectedCharacter(false);
    }

    public void SetPlayerReady(PlayerReady readyObj)
    {
        KeyValuePair<PlayerSelection, Player> pair = selectedPlayers.Single(x => x.Value.id == readyObj.playerId);

        if (pair.Key.isReady) {
            pair.Key.readyDisplay.SetActive(false);
        } else {
            pair.Key.readyDisplay.SetActive(true);
        }
        pair.Key.isReady = !pair.Key.isReady;
        if (CheckIfAllReady()) {
            SetupCharacters();
        }
    }

    bool CheckIfAllReady()
    {
        foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
            if (!pair.Key.isReady) {
                return (false);
            }
        }
        return (true);
    }

    void SetupCharacters()
    {
        Parser parser = GameObject.FindObjectOfType<Parser>();
        parser.players = new List<Player>();
        foreach (KeyValuePair<PlayerSelection, Player> pair in selectedPlayers) {
            pair.Value.username = pair.Key.playerName.text;
            Character playerCharacter = Instantiate(pair.Key.selectedCharacter);
            playerCharacter.playerId = pair.Value.id;
            playerCharacter.transform.SetParent(pair.Value.transform);
            pair.Value.name = pair.Key.playerName.text;
            pair.Value.selectedCharacter = playerCharacter;
            parser.players.Add(pair.Value);
            playerCharacter.gameObject.SetActive(false);
            DontDestroyOnLoad(pair.Value.gameObject);
        }
        parser.players.Sort(delegate(Player player1, Player player2) {
            return (player1.team.CompareTo(player2.team));
        });
        SceneManager.LoadScene("BattleScene");
    }
}
