using UnityEngine;

public class MatchButton : MonoBehaviour
{
    public void OnFindMatch()
    {
        //Matchmaking.instance.CancelMatchmaking(Queues.OneVsOne);
        Matchmaking.instance.FindMatch(Queues.OneVsOne);
    }
}
