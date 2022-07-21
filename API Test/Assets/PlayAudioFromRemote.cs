/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioFromRemote : MonoBehaviour
{
    string url = "https://www.wavsource.com/snds_2020-10-01_3728627494378403/people/comedians/allen_arrogh.wav";
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudio(url));
    }
    private IEnumerator GetAudio(string url)
    {
        WWW www = new WWW( url);
        yield return www;
        if (string.IsNullOrEmpty(www.error) == false)
        {
            Debug.Log("Did not work");
            yield break;
        }
        source = GetComponent<AudioSource>();
        source.clip = www.GetAudioClip(false, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/

/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MyBehaviour : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetAudioClip());

    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://www.wavsource.com/snds_2020-10-01_3728627494378403/people/comedians/allen_arrogh.wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayAudioFromRemote : MonoBehaviour
{

    AudioSource audioSource;
    AudioClip myClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GetAudioClip());
        Debug.Log("Starting to download the audio...");
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://www.wavsource.com/snds_2020-10-01_3728627494378403/people/comedians/allen_arrogh.wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.Play();
                Debug.Log("Audio is playing.");
            }
        }
    }


    public void pauseAudio()
    {
        audioSource.Pause();
    }

    public void playAudio()
    {
        audioSource.Play();
    }

    public void stopAudio()
    {
        audioSource.Stop();

    }
}
*/
/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioFromRemote : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetAudioClip());
    }

    IEnumerator GetAudioClip()
    {
        //Application.dataPath + "/hexample.wav"
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://www.wavsource.com/snds_2020-10-01_3728627494378403/people/comedians/allen_arrogh.wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                Debug.Log("Screams of the danged");
            }
            else
            {
                AudioSource mySource = GetComponent<AudioSource>();
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("The clip is in");
                mySource.clip = myClip;
                mySource.Play();
            }
        }
    }
}
*/
/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace HexTTS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioFromRemote : MonoBehaviour
    {
        string filePath = "C:/Users/jtrex/Documents/UnityDocuments/API Test/hexample.wav";

        void Start()
        {
            //Testing
            StartCoroutine(GetAudioClip());
        }

        public void PlayAudio(string pathImport)
        {
            filePath = pathImport;

            StartCoroutine(PlayAudioClip());

            IEnumerator PlayAudioClip()
            {
                //Application.dataPath + "/hexample.wav"
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
                {
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(www.error);
                        Debug.Log("Screams of the danged");
                    }
                    else
                    {
                        AudioSource mySource = GetComponent<AudioSource>();
                        AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                        Debug.Log("The clip is in");
                        mySource.clip = myClip;
                        mySource.Play();
                    }
                }
            }
        }

        IEnumerator GetAudioClip()
        {
            //Application.dataPath + "/hexample.wav"
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                    Debug.Log("Screams of the danged");
                }
                else
                {
                    AudioSource mySource = GetComponent<AudioSource>();
                    AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                    Debug.Log("The clip is in");
                    mySource.clip = myClip;
                    mySource.Play();
                }
            }
        }
    }
}
*/
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioFromDisk : MonoBehaviour
{
    string path;
    string extension;
    public GameObject musicAnalysis;
    string songName;
    float length;
    AudioSource song;

    // Use this for initialization
    void Start()
    {
        FileSelect();
        //song = musicAnalysis.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (song.isPlaying != true)
        {
            song.Play();
        }
        */
/*
    }

    public void FileSelect()
    {
        //Open windows Exploer 
        path = Application.dataPath + "/hexample.wav";

        Debug.Log(path);

        //Take the end of the the path and sasve it to another string
        extension = path.Substring(path.IndexOf('.') + 1);

        Debug.Log(extension);
        //Check if the user has select the correct file
        if (extension == "mp3" || extension == "wav" || extension == "ogg")
        {
            //if correct file process file
            Debug.Log("You have selected the correct file type congrats");

            LoadSong();
            Debug.Log("Song Name: " + songName);
            Debug.Log("Song Length: " + length);
        }
        //if the user selects the wrong file type
        else
        {
            Debug.Log("Invalid Path");
        }
    }

    public void LoadSong()
    {
        StartCoroutine(LoadSongCoroutine());
    }

    IEnumerator LoadSongCoroutine()
    {
        string url = string.Format("file://{0}", path);
        WWW www = new WWW(url);
        yield return www;

        song.clip = www.GetAudioClip(false, false);
        songName = song.clip.name;
        length = song.clip.length;
        Debug.Log("Going for it");
        song = musicAnalysis.GetComponent<AudioSource>();
        song.Play();
    }
}
*/