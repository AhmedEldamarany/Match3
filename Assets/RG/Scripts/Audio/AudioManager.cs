using System;

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClipSO clickSO;
    [SerializeField] private AudioClipSO matchSO;
    [SerializeField] private AudioClipSO shiftSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe<Tile>(ActionType.TileClicked, OnTileClicked);
        EventManager.Subscribe<bool>(ActionType.TileMatched, OnMatchFound);
        EventManager.Subscribe<bool>(ActionType.TileShifted, OnTilesShifted);
    }

    private void OnTilesShifted(bool isShifting)
    {
        if (isShifting)
        {
            audioSource.PlayOneShot(shiftSO.audioClip);
        }
    }

    private void OnMatchFound(bool hasMatch)
    {
        if (hasMatch) {
            audioSource.PlayOneShot(matchSO.audioClip);
        }
    }

    private void OnTileClicked(Tile tile) { 
    
        audioSource.PlayOneShot(clickSO.audioClip);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<Tile>(ActionType.TileClicked, OnTileClicked);
        EventManager.Unsubscribe<bool>(ActionType.TileMatched, OnMatchFound);
    }
}
