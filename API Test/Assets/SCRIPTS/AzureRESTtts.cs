using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.Networking;

namespace HexTTS
{
    public class AzureRESTtts : MonoBehaviour
    {

        string audioPlayPath = "C:/Users/jtrex/Documents/UnityDocuments/API Test/hexfile.wav";
        public AudioClip outClip;
        public AudioSource outSource;
        public class Authentication
        {
            private string subscriptionKey;
            private string tokenFetchUri;

            public Authentication(string tokenFetchUri, string subscriptionKey)
            {
                if (string.IsNullOrWhiteSpace(tokenFetchUri))
                {
                    throw new ArgumentNullException(nameof(tokenFetchUri));
                }
                if (string.IsNullOrWhiteSpace(subscriptionKey))
                {
                    throw new ArgumentNullException(nameof(subscriptionKey));
                }
                this.tokenFetchUri = tokenFetchUri;
                this.subscriptionKey = subscriptionKey;
            }

            public async Task<string> FetchTokenAsync()
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
                    UriBuilder uriBuilder = new UriBuilder(this.tokenFetchUri);

                    HttpResponseMessage result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null).ConfigureAwait(false);
                    return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }


        static async void PostIt()
        {
            // Prompts the user to input text for TTS conversion
            Debug.Log("What would you like to convert to speech? ");
            //INPUT HERE
            string text = "Welcome back";

            // Gets an access token
            string accessToken;
            //Debug.Log("Attempting token exchange. Please wait...\n");
            Debug.Log("Token Exchange disabled");

            // Add your subscription key here
            // If your resource isn't in WEST US, change the endpoint
            //Might need to bypass for webGL
            /*
            Authentication auth = new Authentication("https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken", "e47077c55e9541d383c71ac1a6a9a154");
            try
            {
                accessToken = await auth.FetchTokenAsync().ConfigureAwait(false);
                Debug.Log("Successfully obtained an access token. \n");
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to obtain an access token.");
                Debug.Log(ex.ToString());
                Debug.Log(ex.Message);
                return;
            }
            */

            string host = "https://eastus.tts.speech.microsoft.com/cognitiveservices/v1";

            // Create SSML document.
            // Makes up the body of the request.
            XDocument body = new XDocument(
                    new XElement("speak",
                        new XAttribute("version", "1.0"),
                        new XAttribute(XNamespace.Xml + "lang", "en-US"),
                        new XElement("voice",
                            new XAttribute(XNamespace.Xml + "lang", "en-US"),
                            new XAttribute(XNamespace.Xml + "gender", "Female"),
                            new XAttribute("name", "en-US-JennyNeural"), // Short name for "Microsoft Server Speech Text to Speech Voice (en-US, Jessa24KRUS)"
                            text)));

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    // Set the HTTP method
                    request.Method = HttpMethod.Post;
                    // Construct the URI
                    request.RequestUri = new Uri(host);
                    // Set the content type header
                    request.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/ssml+xml");
                    // Set additional header, such as Authorization and User-Agent
                    //TRYING SOMETHING HERE
                    //request.Headers.Add("Authorization", "Bearer " + accessToken);
                    request.Headers.Add("Ocp-Apim-Subscription-Key", "e47077c55e9541d383c71ac1a6a9a154");
                    request.Headers.Add("Connection", "Keep-Alive");
                    // Update your ressaource name
                    request.Headers.Add("User-Agent", "AUDIOTEST");
                    // Audio output format. See API reference for full list.
                    request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
                    // Create a request
                    Debug.Log("Calling the TTS service. Please wait... \n");
                    
                    using (HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        Debug.Log("Inside using");
                        response.EnsureSuccessStatusCode();
                        Debug.Log("Starting second using");
                        // Asynchronously read the response
                        using (Stream dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            Debug.Log("Your speech file is being written to file...");
                            if (File.Exists(@"hexfile.wav"))
                            {
                                Debug.Log("File will be overwritten");
                                //Info("File will be overwritten");
                                File.Delete(@"hexample.wav");
                            }

                            //Will create file in root named hexample.wav
                            using (FileStream fileStream = new FileStream(@"hexfile.wav", FileMode.Create, FileAccess.Write, FileShare.Write))
                            {
                                //try catch?
                                await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                                fileStream.Close();
                            }
                            Debug.Log("\nFile ready");
                            //PlayRetrievedFile();
                            //Debug.Log(Application.dataPath);
                        }
                    }
                    

                }
            }
        }

        

        void PlayRetrievedFile()
        {
            AudioClip outClip = null;
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file:///"+audioPlayPath, AudioType.WAV))
            {
                uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Error: " + uwr.error);
                    //Error(uwr.error);
                }
                else
                {
                    outClip = DownloadHandlerAudioClip.GetContent(uwr);
                    outSource.clip = outClip;
                    outSource.Play();
                }

            }
        }

        /*
        public IEnumerator StartAudio(string audioPath)
        {
            AudioClip outClip = null;
            UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file:///" + audioPlayPath, AudioType.WAV);

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + uwr.error);
                //Error(uwr.error);
            }
            else
            {
                outClip = DownloadHandlerAudioClip.GetContent(uwr);
                outSource.clip = outClip;
                outSource.Play();
            }

            return outClip;
        }
        */

        async Task<AudioClip> LoadClip(string path)
        {
            AudioClip clip = null;
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
            {
                uwr.SendWebRequest();

                // wrap tasks in try/catch, otherwise it'll fail silently
                try
                {
                    while (!uwr.isDone) await Task.Delay(5);

                    if (uwr.result != UnityWebRequest.Result.Success) {
                        Debug.Log($"{uwr.error}");
                    } 
                    else
                    {
                        clip = DownloadHandlerAudioClip.GetContent(uwr);
                    }
                }
                catch (Exception err)
                {
                    Debug.Log($"{err.Message}, {err.StackTrace}");
                }
            }

            return clip;
        }


        void Awake()
        {
            
            PostIt();
            Debug.Log("Playing");
            //PlayRetrievedFile();
            //Debug.Log(Application.dataPath + "/ResponseSpeech.wav");



        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}