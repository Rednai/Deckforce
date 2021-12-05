using UnityEngine;
using UnityEngine.Video;

public class playvideo : MonoBehaviour
{
    public bool loop = true;
    public bool playFromStart = true;
    public VideoClip myVideoClip;

    private VideoPlayer myVideoPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer = GetComponent<VideoPlayer>();
        if (playFromStart)
            ControlMovie();
        myVideoPlayer.isLooping = loop;
    }

    private void OnMouseUp()
    {
        ControlMovie();
    }

    public void ControlMovie()
    {
        if (myVideoPlayer.isPlaying)
            myVideoPlayer.Pause();
        else
            myVideoPlayer.Play();
    }
}