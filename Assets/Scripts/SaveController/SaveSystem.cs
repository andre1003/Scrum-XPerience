using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    private static string path = Application.persistentDataPath;

    public static void Save(string decisionId, string decisionDescription, string scenery, bool isMistake, int index) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string file = path + "/decision_" + index + ".sxp";
        FileStream stream = new FileStream(file, FileMode.Create);

        Decision data = new Decision(decisionId, decisionDescription, scenery, isMistake);
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

    public static void SaveInDatabase(string decisionId, string decisionDescription, string scenery, bool isMistake, int index) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if(!Directory.Exists(Directory.GetCurrentDirectory() + "/data/"))
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/data/");

        string file = Directory.GetCurrentDirectory() + "/data/decision_" + index + ".sxp";
        FileStream stream = new FileStream(file, FileMode.Create);

        Decision data = new Decision(decisionId, decisionDescription, scenery, isMistake);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static Decision Load(int index) {
        string file = path + "/decision_" + index + ".sxp";

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
}
