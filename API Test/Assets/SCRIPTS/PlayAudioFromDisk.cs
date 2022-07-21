
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace HexTTS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioFromDisk : MonoBehaviour
    {
        //string filePath = "C:/Users/jtrex/Documents/UnityDocuments/API Test/welcome.wav";
        string filePath = "http://www.aoakley.com/articles/stereo-test.mp3";

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
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
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
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
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