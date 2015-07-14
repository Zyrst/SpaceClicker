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
        Transform[] Garr = Global.Instance.gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < Garr.Length; i++)
        {
            Garr[i].gameObject.SetActive(true);
            if (Garr[i].GetComponent<StoreInformation>() == null)
            {
                Garr[i].gameObject.AddComponent<StoreInformation>();
            }
            if (Garr[i].GetComponent<StoreMaterials>() == null)
            {
                Garr[i].gameObject.AddComponent<StoreMaterials>();
            }
        }

        Global.Instance.player.PreSave();

        Transform[] Sarr = Starmap.Instance.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < Sarr.Length; i++)
        {
            Sarr[i].gameObject.SetActive(true);
        }
        Starmap.Instance.gameObject.SetActive(true);

        LevelSerializer.SaveGame("SpaceClicker");

        Global.Instance.PostIO();
        Global.Instance.player.PostSave();

        Starmap.Instance.gameObject.SetActive(true);

        //CharacterScreen.Instance.ClosePopup();
        //Ship.Instance.ExitCharacter();
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
        Global.Instance.player.PreLoad();

        LevelSerializer.LoadNow(LevelSerializer.SavedGames[LevelSerializer.PlayerName][0].Data);

        Global.Instance.PostIO();
        Global.Instance.player.PostLoad();
        Starmap.Instance.gameObject.SetActive(false);
        //CharacterScreen.Instance.ClosePopup();
        //Ship.Instance.ExitCharacter();
    }
}
