using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Firebase;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    bool isSaveSuccessful;

    // Awake is called before Start
    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance; //預設
    }
    // Start is called before the first frame update
    void Start()
    {
        auth.StateChanged += AuthStateChange;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChange;
    }

    void AuthStateChange(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            if(user != null)
            {
                print($"Login - {user.Email}");
            }
        }
    }

    public void SaveData()
    {
        if(user != null)
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
            isSaveSuccessful = true;
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId);
            
            string _key = "playerName";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(Dictionaries.character_name[0]).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存角色
            _key = "myCharacter";
            print("儲存: " + _key);
            foreach(int num in Dictionaries.character_name.Keys)
            {
                if(Dictionaries.myCharacter.ContainsKey(num))
                {
                    userReference.Child(_key).Child(num.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(Dictionaries.myCharacter[num])).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
                else
                {
                    userReference.Child(_key).Child(num.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Save(0, 0, 0))).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
            }
            //儲存道具
            _key = "myItem";
            print("儲存: " + _key);
            foreach(int num in Dictionaries.item_Info.Keys)
            {
                if(Dictionaries.myItem.ContainsKey(num))
                {
                    userReference.Child(_key).Child(num.ToString()).SetValueAsync(Dictionaries.myItem[num]).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
                else
                {
                    userReference.Child(_key).Child(num.ToString()).SetValueAsync(0).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
            }
            //儲存佈陣
            _key = "myFormation";
            print("儲存: " + _key);
            for(int i = 0 ; i < 20 ; i++)
            {
                if(Dictionaries.myFormation.ContainsKey(i))
                {
                    userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(Dictionaries.myFormation[i])).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
                else
                {
                    userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Fight(-1, 0, 0))).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
            }
            //儲存備用佈陣
            _key = "myFormation_backup";
            print("儲存: " + _key);
            for(int i = 0 ; i <= 3 ; i++)
            {
                if(Dictionaries.myFormation_backup.ContainsKey(i))
                {
                    userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(Dictionaries.myFormation_backup[i])).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
                else
                {
                    userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Formation(new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}))).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
            }
            //儲存錢
            _key = "money";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.money).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存升錢幣
            _key = "money_up";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.money_up).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存骰子
            _key = "dice";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.dice).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存碎片
            _key = "fragment";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.fragment).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存樂透點數
            _key = "lottoPoint";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.lottoPoint).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存職業
            _key = "myProfession";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.myProfession).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存職業2
            _key = "myProfession2";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.myProfession2).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存技能點
            _key = "mySkill";
            print("儲存: " + _key);
            userReference.Child(_key).SetRawJsonValueAsync(JsonUtility.ToJson(InitialScene.mySkill)).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存主城
            _key = "myMainCityLv";
            print("儲存: " + _key);
            for(int i = 0 ; i < InitialScene.myMainCityLv.Length ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetValueAsync(InitialScene.myMainCityLv[i]).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存訓練
            _key = "myCharacterTrain";
            print("儲存: " + _key);
            for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(InitialScene.myCharacterTrain[i])).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存礦坑
            _key = "myMine";
            print("儲存: " + _key);
            for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(InitialScene.myMine[i])).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存通過關卡
            _key = "myMapProgress";
            print("儲存: " + _key);
            for(int i = 0 ; i < 2 ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetValueAsync(InitialScene.myMapProgress[i]).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存每日獎勵時間
            _key = "myDailySupplyTime";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.myDailySupplyTime.ToString("dd/MM/yyyy")).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存扭蛋
            _key = "myGachaMachine";
            print("儲存: " + _key);
            userReference.Child(_key).RemoveValueAsync(); //清除扭蛋紀錄
            for(int i = 0 ; i < InitialScene.myGachaMachine.Length ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(InitialScene.myGachaMachine[i])).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存扭蛋編號
            _key = "myGachaNum";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.myGachaNum).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存樂透骰子時間
            _key = "myLottoDiceTime";
            print("儲存: " + _key);
            for(int i = 0 ; i < 7 ; i++)
            {
                userReference.Child(_key).Child(i.ToString()).SetValueAsync(InitialScene.myLottoDiceTime[i].ToString("dd/MM/yyyy")).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        isSaveSuccessful = false;
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            //儲存禮包碼
            _key = "myGiftCode";
            print("儲存: " + _key);
            userReference.Child(_key).SetValueAsync(InitialScene.myGiftCode).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    isSaveSuccessful = false;
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            //儲存造型
            _key = "mySkin";
            print("儲存: " + _key);
            foreach(int num in Dictionaries.sprite_skin.Keys)
            {
                if(Dictionaries.mySkin.ContainsKey(num))
                {
                    userReference.Child(_key).Child(num.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(Dictionaries.mySkin[num])).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
                else
                {
                    List<int> list_int = new List<int>();
                    list_int.Add(0);
                    userReference.Child(_key).Child(num.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Skin(list_int))).ContinueWith(task => {
                        if(task.IsFaulted)
                        {
                            isSaveSuccessful = false;
                            print(task.Exception.InnerException.Message);
                            StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                        }
                    });
                }
            }
            
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
            StartCoroutine(InitialScene.WarnText("儲存結束"));
        }
        else
        {
            print("No user!");
        }
    }
    public IEnumerator LoadData()
    {
        if(user != null)
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
            DatabaseReference UserReference = FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId);

            string _key = "playerName";
            var task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                Dictionaries.character_name[0] = snapshot.Value.ToString();
                PlayerPrefs.SetString(_key, Dictionaries.character_name[0]);
            }
            //載入角色
            if(true)
            {
            _key = "myCharacter";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    if(JsonUtility.FromJson<MyStructures.Character_Save>(data.GetRawJsonValue()).rank == 0)
                    {
                        if(Dictionaries.myCharacter.ContainsKey(num)) Dictionaries.myCharacter.Remove(num);
                        if(PlayerPrefs.HasKey(_key + num)) PlayerPrefs.DeleteKey(_key + num);
                    }
                    else
                    {
                        if(Dictionaries.myCharacter.ContainsKey(num))
                        {
                            Dictionaries.myCharacter[num] = JsonUtility.FromJson<MyStructures.Character_Save>(data.GetRawJsonValue());
                        }
                        else
                        {
                            Dictionaries.myCharacter.Add(num, JsonUtility.FromJson<MyStructures.Character_Save>(data.GetRawJsonValue()));
                        }
                        PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(Dictionaries.myCharacter[num]));
                    }
                }
            }
            }
            //載入道具
            if(true)
            {
            _key = "myItem";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    if(data.Value.ToString() == "0")
                    {
                        if(Dictionaries.myItem.ContainsKey(num)) Dictionaries.myItem.Remove(num);
                        if(PlayerPrefs.HasKey(_key + num)) PlayerPrefs.DeleteKey(_key + num);
                    }
                    else
                    {
                        if(Dictionaries.myItem.ContainsKey(num))
                        {
                            Dictionaries.myItem[num] = int.Parse(data.Value.ToString());
                        }
                        else
                        {
                            Dictionaries.myItem.Add(num, int.Parse(data.Value.ToString()));
                        }
                        PlayerPrefs.SetInt(_key + num, Dictionaries.myItem[num]);
                    }
                }
            }
            }
            //載入佈陣
            if(true)
            {
            _key = "myFormation";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    if(JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()).num == -1)
                    {
                        if(Dictionaries.myFormation.ContainsKey(num)) Dictionaries.myFormation.Remove(num);
                        if(PlayerPrefs.HasKey(_key + num)) PlayerPrefs.DeleteKey(_key + num);
                    }
                    else
                    {
                        if(Dictionaries.myFormation.ContainsKey(num))
                        {
                            Dictionaries.myFormation[num] = JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue());
                        }
                        else
                        {
                            Dictionaries.myFormation.Add(num, JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()));
                        }
                        PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(Dictionaries.myFormation[num]));
                    }
                }
            }
            }
            //載入備用佈陣
            if(true)
            {
            _key = "myFormation_backup";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    if(Dictionaries.myFormation_backup.ContainsKey(num))
                    {
                        Dictionaries.myFormation_backup[num] = JsonUtility.FromJson<MyStructures.Character_Formation>(data.GetRawJsonValue());
                    }
                    else
                    {
                        Dictionaries.myFormation_backup.Add(num, JsonUtility.FromJson<MyStructures.Character_Formation>(data.GetRawJsonValue()));
                    }
                    PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(Dictionaries.myFormation_backup[num]));
                }
            }
            }
            //載入錢
            if(true)
            {
            _key = "money";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.money = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.money);
            }
            }
            //載入升錢幣
            if(true)
            {
            _key = "money_up";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.money_up = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.money_up);
            }
            }
            //載入骰子
            if(true)
            {
            _key = "dice";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.dice = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.dice);
            }
            }
            //載入碎片
            if(true)
            {
            _key = "fragment";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.fragment = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.fragment);
            }
            }
            //載入樂透點數
            if(true)
            {
            _key = "lottoPoint";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.lottoPoint = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.lottoPoint);
            }
            }
            //載入職業
            if(true)
            {
            _key = "myProfession";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.myProfession = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.myProfession);
            }
            }
            //載入職業
            if(true)
            {
            _key = "myProfession2";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.myProfession2 = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.myProfession2);
            }
            }
            //載入技能點
            if(true)
            {
            _key = "mySkill";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.mySkill = JsonUtility.FromJson<MyStructures.Skill>(snapshot.GetRawJsonValue());
                PlayerPrefs.SetString(_key, JsonUtility.ToJson(InitialScene.mySkill));
            }
            }
            //數值計算
            if(InitialScene.myProfession != 0)
            {
                MyStructures.Character_Base cb_me = new MyStructures.Character_Base(50, 50, 50, 50, 50, 1, 1, 1.0f);
                int value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 0);
                cb_me.hp_b = (50 + InitialScene.mySkill.hp) > value_limit ? value_limit : (50 + InitialScene.mySkill.hp);
                value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 1);
                cb_me.atk_b = (50 +InitialScene. mySkill.atk) > value_limit ? value_limit : (50 + InitialScene.mySkill.atk);
                value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 2);
                cb_me.def_b = (50 + InitialScene.mySkill.def) > value_limit ? value_limit : (50 + InitialScene.mySkill.def);
                value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 3);
                cb_me.spd_b = (50 + InitialScene.mySkill.spd) > value_limit ? value_limit : (50 + InitialScene.mySkill.spd);
                value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 4);
                cb_me.sp_b = (50 + InitialScene.mySkill.sp) > value_limit ? value_limit : (50 + InitialScene.mySkill.sp);
                value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 5);
                cb_me.range_b = (10 + InitialScene.mySkill.range) > value_limit ? value_limit/10f : (10 + InitialScene.mySkill.range)/10f;
                Dictionaries.character_base[0] = cb_me;
            }
            //載入主城
            if(true)
            {
            _key = "myMainCityLv";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myMainCityLv[num] = int.Parse(data.Value.ToString());
                    PlayerPrefs.SetInt(_key + num, InitialScene.myMainCityLv[num]);
                }
            }
            }
            //載入訓練
            if(true)
            {
            _key = "myCharacterTrain";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myCharacterTrain[num] = JsonUtility.FromJson<MyStructures.Character_Time>(data.GetRawJsonValue());
                    PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(InitialScene.myCharacterTrain[num]));
                }
            }
            }
            //載入礦坑
            if(true)
            {
            _key = "myMine";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myMine[num] = JsonUtility.FromJson<MyStructures.Character_Time>(data.GetRawJsonValue());
                    PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(InitialScene.myMine[num]));
                }
            }
            }
            //載入通過關卡
            if(true)
            {
            _key = "myMapProgress";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myMapProgress[num] = int.Parse(data.Value.ToString());
                    PlayerPrefs.SetInt(_key + num, InitialScene.myMapProgress[num]);
                }
            }
            }
            //載入每日獎勵時間
            if(true)
            {
            _key = "myDailySupplyTime";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.myDailySupplyTime = DateTime.ParseExact(snapshot.Value.ToString(), "dd/MM/yyyy", null);
                PlayerPrefs.SetString(_key, InitialScene.myDailySupplyTime.ToString("dd/MM/yyyy"));
            }
            }
            //載入扭蛋
            if(true)
            {
            _key = "myGachaMachine";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                for(int i = 0 ; i < 500 ; i++)
                {
                    if(PlayerPrefs.HasKey(_key + i)) PlayerPrefs.DeleteKey(_key + i);
                }
                int gachaNum = (int)snapshot.ChildrenCount;
                InitialScene.myGachaMachine = new MyStructures.GachaItem[gachaNum];
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myGachaMachine[num] = JsonUtility.FromJson<MyStructures.GachaItem>(data.GetRawJsonValue());
                    PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(InitialScene.myGachaMachine[num]));
                }
            }
            }
            //載入扭蛋編號
            if(true)
            {
            _key = "myGachaNum";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.myGachaNum = int.Parse(snapshot.Value.ToString());
                PlayerPrefs.SetInt(_key, InitialScene.myGachaNum);
            }
            }
            //載入樂透骰子時間
            if(true)
            {
            _key = "myLottoDiceTime";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    InitialScene.myLottoDiceTime[num] = DateTime.ParseExact(data.Value.ToString(), "dd/MM/yyyy", null);
                    PlayerPrefs.SetString(_key + num, InitialScene.myLottoDiceTime[num].ToString("dd/MM/yyyy"));
                }
            }
            }
            //載入禮包碼
            if(true)
            {
            _key = "myGiftCode";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                InitialScene.myGiftCode = snapshot.Value.ToString();
                PlayerPrefs.SetString(_key, InitialScene.myGiftCode);
            }
            }
            //載入造型
            if(true)
            {
            _key = "mySkin";
            task = UserReference.Child(_key).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
            }
            if(task.IsCompletedSuccessfully)
            {
                print("載入: " + _key);
                DataSnapshot snapshot = task.Result;
                foreach(var data in snapshot.Children)
                {
                    int num = int.Parse(data.Key);
                    if(Dictionaries.mySkin.ContainsKey(num))
                    {
                        Dictionaries.mySkin[num] = JsonUtility.FromJson<MyStructures.Character_Skin>(data.GetRawJsonValue());
                    }
                    else
                    {
                        Dictionaries.mySkin.Add(num, JsonUtility.FromJson<MyStructures.Character_Skin>(data.GetRawJsonValue()));
                    }
                    PlayerPrefs.SetString(_key + num, JsonUtility.ToJson(Dictionaries.mySkin[num]));
                }
            }
            }
            
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
            StartCoroutine(InitialScene.WarnText("載入結束"));
        }
        else
        {
            print("No user!");
        }
    }
    public void SaveRing(string playerName, Dictionary<int, MyStructures.Character_Fight> formation)
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("ring");

        string _key = "playerName";
        print("儲存" + _key);
        arenaRefence.Child(_key).SetValueAsync(playerName).ContinueWith(task => {
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
            }
        });
        _key = "formation";
        print("儲存" + _key);
        for(int i = 0 ; i < 20 ; i++)
        {
            if(formation.ContainsKey(i))
            {
                arenaRefence.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(formation[i])).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            else
            {
                arenaRefence.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Fight(-1, 0, 0))).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public IEnumerator LoadRing()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("ring");

        string _key = "playerName";
        print("儲存" + _key);
        var task = arenaRefence.Child(_key).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            TradingScene.arena_ring_playerName = string.Empty;
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: " + _key);
            DataSnapshot snapshot = task.Result;
            TradingScene.arena_ring_playerName = snapshot.Value.ToString();
        }
        _key = "formation";
        task = arenaRefence.Child(_key).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            TradingScene.arena_ring_playerName = string.Empty;
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: " + _key);
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                int num = int.Parse(data.Key);
                if(JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()).num == -1)
                {
                    if(TradingScene.arena_ring_formation.ContainsKey(num)) TradingScene.arena_ring_formation.Remove(num);
                }
                else
                {
                    if(TradingScene.arena_ring_formation.ContainsKey(num))
                    {
                        TradingScene.arena_ring_formation[num] = JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue());
                    }
                    else
                    {
                        TradingScene.arena_ring_formation.Add(num, JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()));
                    }
                }
            }
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public void SaveCasual(string playerName, Dictionary<int, MyStructures.Character_Fight> formation)
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("casual");

        string _key = "playerName";
        print("儲存" + _key);
        arenaRefence.Child(_key).SetValueAsync(playerName).ContinueWith(task => {
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
            }
        });
        _key = "formation";
        print("儲存" + _key);
        for(int i = 0 ; i < 20 ; i++)
        {
            if(formation.ContainsKey(i))
            {
                arenaRefence.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(formation[i])).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
            else
            {
                arenaRefence.Child(_key).Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(new MyStructures.Character_Fight(-1, 0, 0))).ContinueWith(task => {
                    if(task.IsFaulted)
                    {
                        print(task.Exception.InnerException.Message);
                        StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                    }
                });
            }
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public IEnumerator LoadCasual()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("casual");

        string _key = "playerName";
        print("儲存" + _key);
        var task = arenaRefence.Child(_key).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            TradingScene.arena_casual_playerName = string.Empty;
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: " + _key);
            DataSnapshot snapshot = task.Result;
            TradingScene.arena_casual_playerName = snapshot.Value.ToString();
        }
        _key = "formation";
        task = arenaRefence.Child(_key).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            TradingScene.arena_casual_playerName = string.Empty;
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText(_key + "載入失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: " + _key);
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                int num = int.Parse(data.Key);
                if(JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()).num == -1)
                {
                    if(TradingScene.arena_casual_formation.ContainsKey(num)) TradingScene.arena_casual_formation.Remove(num);
                }
                else
                {
                    if(TradingScene.arena_casual_formation.ContainsKey(num))
                    {
                        TradingScene.arena_casual_formation[num] = JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue());
                    }
                    else
                    {
                        TradingScene.arena_casual_formation.Add(num, JsonUtility.FromJson<MyStructures.Character_Fight>(data.GetRawJsonValue()));
                    }
                }
            }
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public IEnumerator SaveChallenge(string playerName, int score)
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("challenge");
        string[] array_playerName = new string[3];
        int[] array_score = new int[3];
        //載入資料
        var task = arenaRefence.Child("rank1").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank1");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[0] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[0] = int.Parse(data.Value.ToString());
                }
            }
        }
        task = arenaRefence.Child("rank2").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank2");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[1] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[1] = int.Parse(data.Value.ToString());
                }
            }
        }
        task = arenaRefence.Child("rank3").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank3");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[2] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[2] = int.Parse(data.Value.ToString());
                }
            }
        }

        if(score <= array_score[2])
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
            yield break;
        }
        else if(playerName == array_playerName[1])
        {
            if(score > array_score[1])
            {
                array_score[1] = score;
                if(array_score[1] > array_score[0])
                {
                    string s = array_playerName[0];
                    array_playerName[0] = array_playerName[1];
                    array_playerName[1] = s;
                    int i = array_score[0];
                    array_score[0] = array_score[1];
                    array_score[1] = i;
                }
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
                yield break;
            }
        }
        else if(playerName == array_playerName[0])
        {
            if(score > array_score[0])
            {
                array_score[0] = score;
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
                yield break;
            }
        }
        else
        {
            array_playerName[2] = playerName;
            array_score[2] = score;
            if(array_score[2] > array_score[1])
            {
                string s = array_playerName[1];
                array_playerName[1] = array_playerName[2];
                array_playerName[2] = s;
                int i = array_score[1];
                array_score[1] = array_score[2];
                array_score[2] = i;
            }
            if(array_score[1] > array_score[0])
            {
                string s = array_playerName[0];
                array_playerName[0] = array_playerName[1];
                array_playerName[1] = s;
                int i = array_score[0];
                array_score[0] = array_score[1];
                array_score[1] = i;
            }
        }

        for(int i = 0 ; i < 3 ; i++)
        {
            string _key = "playerName";
            print("儲存" + _key);
            arenaRefence.Child("rank" + (i+1)).Child(_key).SetValueAsync(array_playerName[i]).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
            _key = "score";
            print("儲存" + _key);
            arenaRefence.Child("rank" + (i+1)).Child(_key).SetValueAsync(array_score[i]).ContinueWith(task => {
                if(task.IsFaulted)
                {
                    print(task.Exception.InnerException.Message);
                    StartCoroutine(InitialScene.WarnText(_key + "儲存失敗"));
                }
            });
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public IEnumerator LoadChallenge()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        DatabaseReference arenaRefence = FirebaseDatabase.DefaultInstance.RootReference.Child("arena").Child("challenge");
        string[] array_playerName = new string[3];
        int[] array_score = new int[3];

        var task = arenaRefence.Child("rank1").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank1");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[0] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[0] = int.Parse(data.Value.ToString());
                }
            }
        }
        task = arenaRefence.Child("rank2").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank2");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[1] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[1] = int.Parse(data.Value.ToString());
                }
            }
        }
        task = arenaRefence.Child("rank3").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("儲存失敗"));
            yield break;
        }
        if(task.IsCompletedSuccessfully)
        {
            print("載入: rank3");
            DataSnapshot snapshot = task.Result;
            foreach(var data in snapshot.Children)
            {
                if(data.Key == "playerName")
                {
                    array_playerName[2] = data.Value.ToString();
                }
                else if(data.Key == "score")
                {
                    array_score[2] = int.Parse(data.Value.ToString());
                }
            }
        }

        TradingScene.arena_challenge_rank = "玩家排名 (自動升至Lv.80)\n";
        TradingScene.arena_challenge_rank += "第一名：" + array_playerName[0] + " (" + array_score[0] + "分)\n";
        TradingScene.arena_challenge_rank += "第二名：" + array_playerName[1] + " (" + array_score[1] + "分)\n";
        TradingScene.arena_challenge_rank += "第三名：" + array_playerName[2] + " (" + array_score[2] + "分)";

        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }

    public IEnumerator Register(string email, string password)
    {
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsCanceled)
        {}
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("註冊失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("Register!");
            SaveData();

            if(!isSaveSuccessful) yield return StartCoroutine(InitialScene.WarnText("儲存失敗，請手動儲存"));
        }
    }
    public IEnumerator Login(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);
        if(task.IsCanceled)
        {}
        if(task.IsFaulted)
        {
            print(task.Exception.InnerException.Message);
            yield return StartCoroutine(InitialScene.WarnText("登入失敗"));
        }
        if(task.IsCompletedSuccessfully)
        {
            print("Login!");
            StartCoroutine(LoadData());
        }
    }
    public void Logout()
    {
        SaveData();
        if(isSaveSuccessful)
        {
            print("Logout!");
            auth.SignOut();
        }
        else
        {
            print("Fail to logout!");
            StartCoroutine(InitialScene.WarnText("儲存失敗，無法登出"));
        }
    }
    public DatabaseReference GetUserReference()
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId);
    }
}
