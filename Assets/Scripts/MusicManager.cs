using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource source;
    private float volume;

    private void Start()
    {
        volume = source.volume;
    }

    void Update () {
        source.volume = volume * MatchPref.musicVol;
	}
}
