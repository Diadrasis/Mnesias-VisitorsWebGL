using UnityEngine;
using System.Linq;


//this class is only used to load audio clips from resources folder
public class DatabaseAudio : MonoBehaviour
{
    public AudioClip[] audioFiles;
    public AudioSource source;

    public void AudioInit()
    {
        LoadAudio("_main_dafnisL/");
    }
    public void LoadAudio(string fileName)
    {
        audioFiles = Resources.LoadAll(fileName, typeof(AudioClip)).Cast<AudioClip>().ToArray();
       
        source = Camera.main.gameObject.GetComponent<AudioSource>();
        //Debug.Log("Folder Name Audio: "+fileName);
    }

    //this method is assigned on each button to play the respected sound. Is assigned from editor.
    public void PlaySounds(int num)
    {
        source.clip = audioFiles[num];
        source.PlayOneShot(audioFiles[num]);
        Debug.Log("Num: " + num);
        
    }
}
