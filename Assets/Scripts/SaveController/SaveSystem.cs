using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem {
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
}
