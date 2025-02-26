using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] public AudioSource Sfx;

    public AudioClip musicbg;
    public AudioClip slashfx;
    public AudioClip jumpfx;
    public AudioClip walkingfx;
    public AudioClip dashfx;

    private void Start()
    {
        Music.clip = musicbg;
        Music.Play();
    }

    public void playclip(AudioClip audio)
    {
        Sfx.PlayOneShot(audio); 
    }

    
}
