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
}
