using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;

public static class SaveSystem {
    public static string groupRegisterUrl = "https://www.dcce.ibilce.unesp.br/sxp/group-register/";
    public static string loginUrl = "https://www.dcce.ibilce.unesp.br/sxp/game-login/";
    public static string matchRegisterUrl = "https://www.dcce.ibilce.unesp.br/sxp/match-register/";
    public static string decisionRegisterUrl = "https://www.dcce.ibilce.unesp.br/sxp/decision-register/";
    public static string homeUrl = "https://www.dcce.ibilce.unesp.br/sxp/";
    public static string formsUrl = "https://forms.gle/gJcvL78JERLhGsig8";

    private static string path = Application.persistentDataPath;
    private static string decisionsPath = Directory.GetCurrentDirectory();

    public static void Save(string decisionId, string decisionDescription, string scenery, string output, bool isMistake, int index) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if(!Directory.Exists(path + "/player_data/"))
            Directory.CreateDirectory(path + "/player_data/");

        string file = path + "/player_data/decision_" + index + ".sxp";
        FileStream stream = new FileStream(file, FileMode.Create);

        Decision data = new Decision(decisionId, decisionDescription, scenery, output, isMistake);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGeneralInfo(int timeouts, int teamMeetingMistakes, int clientMeetingMistakes, int developmentMistakes, int teamMeetingHits, int clientMeetingHits, int developmentHits) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if(!Directory.Exists(path + "/general_data/"))
            Directory.CreateDirectory(path + "/general_data/");

        string file = path + "/general_data/group_stats.sxp";
        FileStream stream = new FileStream(file, FileMode.Create);

        GeneralInfo info = new GeneralInfo(timeouts, teamMeetingMistakes, clientMeetingMistakes, developmentMistakes, teamMeetingHits, clientMeetingHits, developmentHits);
        binaryFormatter.Serialize(stream, info);
        stream.Close();
    }

    public static void SaveInDatabase(string decisionId, string decisionDescription, string scenery, string output, bool isMistake, string role, string turn, string round) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        //if(!Directory.Exists(decisionsPath + "/data/"))
        //    Directory.CreateDirectory(decisionsPath + "/data/");

        //if(!Directory.Exists(decisionsPath + "/data/hits/"))
        //    Directory.CreateDirectory(decisionsPath + "/data/hits/");

        //if(!Directory.Exists(decisionsPath + "/data/mistakes1/"))
        //    Directory.CreateDirectory(decisionsPath + "/data/mistakes1/");

        //if(!Directory.Exists(decisionsPath + "/data/mistakes2/"))
        //    Directory.CreateDirectory(decisionsPath + "/data/mistakes2/");

        //if(!Directory.Exists(decisionsPath + "/data/hits/" + scenery + "/"))
        //    Directory.CreateDirectory(decisionsPath + "/data/hits/" + scenery + "/");
        
        if(!Directory.Exists(decisionsPath + "/data/hits/" + scenery + "/" + role + "/"))
            Directory.CreateDirectory(decisionsPath + "/data/hits/" + scenery + "/" + role + "/");

        if(!Directory.Exists(decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/"))
            Directory.CreateDirectory(decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/");

        if(!Directory.Exists(decisionsPath + "/data/mistakes2/" + scenery + "/" + role + "/"))
            Directory.CreateDirectory(decisionsPath + "/data/mistakes2/" + scenery + "/" + role + "/");

        string file;

        if(isMistake) {
            if(File.Exists(decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp"))
                file = decisionsPath + "/data/mistakes2/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
            else
                file = decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
        }
        else {
            file = decisionsPath + "/data/hits/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
        }
        
        FileStream stream = new FileStream(file, FileMode.Create);

        Decision data = new Decision(decisionId, decisionDescription, scenery, output, isMistake);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static void DeleteFromDB(string scenery, string role, string turn, string round) {
        string file = decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
        if(File.Exists(file)) {
            File.Delete(file);
        }
    }

    public static Decision Load(int index) {
        string file = path + "/player_data/decision_" + index + ".sxp";
        Debug.Log(file);

        if(File.Exists(file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(file, FileMode.Open);

            Decision data = binaryFormatter.Deserialize(stream) as Decision;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static List<Decision> LoadAll() {
        int index = 0;
        string file = path + "/player_data/decision_" + index + ".sxp";
        List<Decision> list = new List<Decision>();

        while(File.Exists(file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(file, FileMode.Open);

            Decision data = binaryFormatter.Deserialize(stream) as Decision;
            stream.Close();

            list.Add(data);

            index++;
            file = path + "/player_data/decision_" + index + ".sxp";
        }
        return list;
    }

    public static GeneralInfo LoadGeneralInfo() {
        string file = path + "/general_data/group_stats.sxp";

        if(File.Exists(file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(file, FileMode.Open);

            GeneralInfo data = binaryFormatter.Deserialize(stream) as GeneralInfo;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static List<Decision> LoadFromDatabase(string scenery, string role, string turn, string round) {
        List<Decision> list = new List<Decision>();

        string file1 = decisionsPath + "/data/hits/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
        string file2 = decisionsPath + "/data/mistakes1/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";
        string file3 = decisionsPath + "/data/mistakes2/" + scenery + "/" + role + "/decision_" + turn + "_" + round + ".sxp";

        if(File.Exists(file1)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream stream = new FileStream(file1, FileMode.Open);
            Decision data = binaryFormatter.Deserialize(stream) as Decision;
            list.Add(data);

            stream = new FileStream(file2, FileMode.Open);
            data = binaryFormatter.Deserialize(stream) as Decision;
            list.Add(data);

            stream = new FileStream(file3, FileMode.Open);
            data = binaryFormatter.Deserialize(stream) as Decision;
            list.Add(data);

            return list;
        }
        else {
            return null;
        }
    }

    public static void DeleteAll() {
        int index = 0;
        string file = path + "/player_data/decision_" + index + ".sxp";

        while(File.Exists(file)) {
            File.Delete(file);
            index++;
            file = path + "/player_data/decision_" + index + ".sxp";
        }

        file = path + "/general_data/group_stats.sxp";
        if(File.Exists(file))
            File.Delete(file);

        file = path + "/info.sxp";
        if(File.Exists(file))
            File.Delete(file);
    }

    public static void InfoFile(string status, string username, string role, string hits, string mistakes, string feedback, string group, string matchId) {
        string file = path + "/info.sxp";

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(file, FileMode.Create);

        InfoFile data = new InfoFile(status, username, role, hits, mistakes, feedback, group, matchId);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static InfoFile LoadInfoFile() {
        string file = path + "/info.sxp";
        if(File.Exists(file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream stream = new FileStream(file, FileMode.Open);
            InfoFile data = binaryFormatter.Deserialize(stream) as InfoFile;
            stream.Close();
            return data;
        }
        else {
            return null;
        }
    }

    public static string Post(string url, Dictionary<string, string> data, HttpClient client) {
        try {
            var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(data) };
            var response = client.SendAsync(request).Result;
            string statusCode = response.StatusCode.ToString();
            
            if(statusCode.Equals("OK")) {
                var content = response.Content.ReadAsStringAsync().Result;

                try {
                    JObject json = JObject.Parse(content);
                    return json["response"].ToString();
                }
                catch(Exception e) {
                    Debug.LogError(e);
                    return null;
                }
            }
            else {
                throw new Exception();
            }
        }
        catch(Exception e) {
            return "Connection Failed";
        }
        
    }

    public static string Get(string url, HttpClient client) {
        try {
            var response = client.GetAsync(url).Result;
            string statusCode = response.StatusCode.ToString();

            if(statusCode.Equals("OK"))
                return statusCode;
            else
                throw new Exception();
        }
        catch(Exception e) {
            return "Connection Failed";
        }
    }
}
