using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] AudioSource musicSource;

    [Header("------Audio Files------")]
    [SerializeField] AudioClip[] backgroundTracks; // Array to hold multiple audio clips

    private void Start()
    {
        // Optionally, play a default track at start, e.g., the first track
        PlayTrack(0);
    }

    // Method to play a track by index
    public void PlayTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < backgroundTracks.Length)
        {
            musicSource.clip = backgroundTracks[trackIndex];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Track index out of range: " + trackIndex);
        }
    }
}
