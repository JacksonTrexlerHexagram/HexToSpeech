using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;

[RequireComponent(typeof(AudioSource))]
public class AzureGETtts : MonoBehaviour
{
    string host = "https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken";
    string text = "Basic text to be transcribed";
    void Start()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("Ocp-Apim-Subscription-Key", "e47077c55e9541d383c71ac1a6a9a154");
        XDocument body = new XDocument(
                    new XElement("speak",
                        new XAttribute("version", "1.0"),
                        new XAttribute(XNamespace.Xml + "lang", "en-US"),
                        new XElement("voice",
                            new XAttribute(XNamespace.Xml + "lang", "en-US"),
                            new XAttribute(XNamespace.Xml + "gender", "Female"),
                            new XAttribute("name", "en-US-JennyNeural"), // Short name for "Microsoft Server Speech Text to Speech Voice (en-US, Jessa24KRUS)"
                            text)));
        using (UnityWebRequest www = UnityWebRequest.Post(host, body.ToString()))
        {
            (www.downloadHandler as DownloadHandlerAudioClip).streamAudio = true;

            www.SetRequestHeader("Ocp-Apim-Subscription-Key", "e47077c55e9541d383c71ac1a6a9a154");
            www.SetRequestHeader("X-Microsoft-OutputFormat", "audio-16khz-128kbitrate-mono-mp3");
            www.SetRequestHeader("Connection", "Keep-Alive");
            www.SetRequestHeader("User-Agent", "Unity");

            Debug.Log("Setup done, starting request");
            // www.downloadHandler = new DownloadHandlerBuffer();
            //www.downloadHandler = new DownloadHandlerAudioClip();
            //AudioClip downClip = (www.downloadHandler as DownloadHandlerAudioClip.audioClip);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload success");
                AudioSource mySource = GetComponent<AudioSource>();
                Debug.Log(www);
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                //byte[] results = DownloadHandlerBuffer.GetContent(www);
                Debug.Log(DownloadHandlerBuffer.GetContent(www));
                //byte[] results = www.downloadHandler.data;
                Debug.Log("The funky clip is in");
                mySource.clip = myClip;
                mySource.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
