
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public int Level = 1;
        public int Money = 0;
        public int Score = 0;
        public int PlayerHealth = 100;
        public int TwinsHealth = 100;
        public float Sensetivity = 0.27F;
        public float SFX = 0;
        public float Music = -20;
      //  public bool IsExistBackgroundMusic = false;

        public Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>()
        {
            { "AK-47", new WeaponData(){IsBought = true, IsEquip = true, IsUnlock = true}},
            { "M4A1", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "SCAR", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "M1911", new WeaponData(){IsBought = true, IsEquip = true, IsUnlock = true}},
            { "Beretta 92", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "HK USP", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "Sniper Rifle", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "Shotgun", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "UZI", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "P90", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
            { "UMP 45", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        };
        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
