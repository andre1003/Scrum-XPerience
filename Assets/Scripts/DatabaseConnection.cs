using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using UnityEngine.UI;

public class DatabaseConnection : MonoBehaviour {
    public MenuController menuController;

    public void TokenConnection() {
        string filePath = Directory.GetCurrentDirectory() + @"\Data\data.txt";

        try {
            string authToken = File.ReadAllText(filePath);
            string response = Get($"http://127.0.0.1:8000/login/{authToken}");

            response = response.Replace(" ", "");
            response = response.Replace("{", "");
            response = response.Replace("}", "");
            response = response.Replace("\"", "");

            string[] r = response.Split(',');
            string username = "";

            foreach(var item in r) {
                var data = item.Split(':');
                if(data[0].Equals("username")) {
                    username = data[1];
                    break;
                }
            }

            //menuController.SetUsernameFromDB(username);
            Debug.Log(username);
        }
        // Nothing to do
        catch { }        
    }

    private string Get(string uri) {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using(Stream stream = response.GetResponseStream())
        using(StreamReader reader = new StreamReader(stream)) {
            return reader.ReadToEnd();
        }
    }
}
