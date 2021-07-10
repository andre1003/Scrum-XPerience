using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    private static string path = Application.persistentDataPath;

    public static void Save(string decisionId, string decisionDescription, string scenary, bool isMistake, int index) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string file = path + "/decision_" + index + ".sxp";
        FileStream stream = new FileStream(file, FileMode.Create);

        Data data = new Data(decisionId, decisionDescription, scenary, isMistake);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data Load(int index) {
        string file = path + "/decision_" + index + ".sxp";

        if(File.Exists(file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(file, FileMode.Open);

            Data data = binaryFormatter.Deserialize(stream) as Data;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
