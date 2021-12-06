using UnityEngine;

public class MatchButtonTest : MonoBehaviour
{
    public void OnFindMatch()
    {
        Matchmaking.instance.FindMatch(Queues.OneVsOne);
    }
}
