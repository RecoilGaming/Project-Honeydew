using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundPlayer;

    // runs before scene loads
    private void Awake()
	{
        if (instance == null) instance = this;
    }

    public void PlaySoundClip(AudioClip clip, Transform spawn, float volume)
    {
        AudioSource audioSource = Instantiate(soundPlayer, spawn.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayRandomSoundClip(AudioClip[] clips, Transform spawn, float volume)
    {
        int random = Random.Range(0, clips.Length);
        AudioSource audioSource = Instantiate(soundPlayer, spawn.position, Quaternion.identity);
        audioSource.clip = clips[random];
        audioSource.volume = volume;

        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // heheheha
    public IEnumerator ApplyKnockback(Rigidbody2D target, float amt)
    {
        target.AddForce(target.transform.position.normalized * amt, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.15f);
        Debug.Log("CANCEL");
        target.velocity = Vector2.zero;
    }
}
