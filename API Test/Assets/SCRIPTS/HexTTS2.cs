using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

public class HexTTS2 : MonoBehaviour
{

    public AudioClip outClip;
    public AudioSource outSource;
    // Start is called before the first frame update
    void Start()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("name", "en-US-JennyNeural");

        //string requestBody = "<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Male' name = 'en-US-ChristopherNeural' > Microsoft Speech Service Text - to - Speech API</ voice ></ speak > ";
        string text = "Hello Hexagram";
        XDocument requestBody = new XDocument(
                    new XElement("speak",
                        new XAttribute("version", "1.0"),
                        new XAttribute(XNamespace.Xml + "lang", "en-US"),
                        new XElement("voice",
                            new XAttribute(XNamespace.Xml + "lang", "en-US"),
                            new XAttribute(XNamespace.Xml + "gender", "Female"),
                            new XAttribute("name", "en-US-JennyNeural"),
                            text)));
        StartCoroutine(postRequest("https://eastus.tts.speech.microsoft.com/cognitiveservices/v1", requestBody.ToString()));
    }

    IEnumerator postRequest(string uri, string requestStringBody)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Post(uri, requestStringBody))
        {
            uwr.SetRequestHeader("Ocp-Apim-Subscription-Key", "e47077c55e9541d383c71ac1a6a9a154");

            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
                outClip = DownloadHandlerAudioClip.GetContent(uwr);
                Debug.Log("1");
                outSource.clip = outClip;
                Debug.Log("2");
                outSource.Play();
                Debug.Log("3");
            }
        }
        //uwr.SetRequestHeader("Ocp-Apim-Subscription-Key", "e47077c55e9541d383c71ac1a6a9a154");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
