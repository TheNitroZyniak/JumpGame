using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour{
    private AudioSource _audioSource;

    float time;
    private GameObject childObject;
    private Renderer _renderer;

    void Start(){
        time = 0;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();

        childObject = gameObject.transform.GetChild(0).gameObject;
        _renderer = childObject.GetComponent<SpriteRenderer>();
    }

    void Update(){
        time += Time.deltaTime;

        if (childObject.activeSelf) {
            _renderer.material.color = new Color(1,1,1, Random.Range(0.75f, 1));
        }

        if(time > 5 && _audioSource.isPlaying) {
            childObject.SetActive(false);
            _audioSource.Stop();
        }

        if (time > 10) {
            childObject.SetActive(true);
            _audioSource.Play();
            time = 0;
        }


    }
}
