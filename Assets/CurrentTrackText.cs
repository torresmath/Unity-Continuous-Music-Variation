using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTrackText : MonoBehaviour
{
    Text currentTrackText;
    MusicVariationController musicVariationController;
    private void Awake()
    {
        currentTrackText = GetComponent<Text>();
        musicVariationController = FindObjectOfType<MusicVariationController>();
    }

    private void Update()
    {
        string track = musicVariationController.GetCurrentTrack();
        currentTrackText.text = string.IsNullOrEmpty(track) ? "Paused..." :  $"Playing: {track}";
    }

}
