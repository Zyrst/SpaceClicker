using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadSystem {

    [System.Serializable]
    public class GameState
    {
        public Global _global = null;
        //public CharacterScreen _charScreen = null;
    }

    public static string saveName = "Game";
    public static string saveExtension = "save";

    public static string saveFileName
    {
        get
        {
            return saveName + "." + saveExtension;
        }
    }

    public static void Save()
    {
        for (int i = 0; i < Global.Instance.gameObject.GetComponentsInChildren<Transform>(true).Length; i++)
        {
            Global.Instance.gameObject.GetComponentsInChildren<Transform>(true)[i].gameObject.SetActive(true);
        }

        foreach (var item in Global.Instance.GetComponentsInChildren<Transform>())
        {
            if (item.GetComponent<StoreInformation>() == null)
            {
                item.gameObject.AddComponent<StoreInformation>();
            }
        }

        LevelSerializer.SaveGame("SpaceClicker");

        Global.Instance.PostIO();
    }

    public static void Load()
    {
        /*SpellAttack[] ss = null;
        if (File.Exists(Application.persistentDataPath + "/" + saveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/" + saveFileName, FileMode.Open);
            ss = (SpellAttack[])bf.Deserialize(file);
            file.Close();
        }

        Global.Instance.player._spellsArray = ss;*/
        //CharacterScreen.Instance = gs._charScreen;


        LevelSerializer.LoadNow(LevelSerializer.SavedGames[LevelSerializer.PlayerName][0].Data);

        Global.Instance.PostIO();
    }
}
