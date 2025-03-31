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
    public AudioClip Damagedfx;
    public AudioClip EnemyTakeDamagefx;
    public AudioClip deathfx;
    public AudioClip enemyatkfx;
    public AudioClip keyfx;

    private void Start()
    {
        Music.clip = musicbg;
        Music.Play();
    }

    public void playclip(AudioClip audio)
    {
        Sfx.PlayOneShot(audio); 
    }

    public void stopMusic()
    {
        Music.Stop();
    }
}
