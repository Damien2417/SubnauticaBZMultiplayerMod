using ClientSubnautica.MultiplayerManager;
using HarmonyLib;
using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClientSubnautica
{
    class AddMenuButton
    {
        [HarmonyPatch(typeof(MainMenuRightSide), "OpenGroup")]
        public class Patches
        {
            [HarmonyPostfix]
            static void Postfix(string target, MainMenuRightSide __instance)
            {
                if (GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu") == null)
                {
                    if (target == "SavedGames")
                    {
                        // server ip
                        GameObject gameObject = GameObject.Instantiate(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox"));
                        gameObject.name = "MultiplayerMenu";
                        gameObject.transform.SetParent(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide").transform, false);

                        gameObject.transform.position = new Vector3(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.position.x, 0.25f, (float)(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.position.z - 0.01));
                        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                        gameObject.transform.transform.rotation = (GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.rotation);
                        
                        // nickname
                        GameObject gameObject2 = GameObject.Instantiate(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox"));
                        gameObject2.name = "NicknameMenu";
                        gameObject2.transform.SetParent(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide").transform, false);

                        gameObject2.transform.position = new Vector3(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.position.x, 0.395f, (float)(GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.position.z - 0.01));
                        gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
                        gameObject2.transform.transform.rotation = (GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox").transform.rotation);

                        GameObject.Destroy(gameObject.FindChild("HeaderText").GetComponent<TranslationLiveUpdate>());
                        GameObject.Destroy(gameObject2.FindChild("HeaderText").GetComponent<TranslationLiveUpdate>());

                        GameObject.Destroy(gameObject.transform.Find("SubscriptionSuccess/Text").GetComponent<TranslationLiveUpdate>());
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionSuccess/Text").GetComponent<TextMeshProUGUI>().text = "Server found!";

                        GameObject.Destroy(gameObject.transform.Find("SubscriptionInProgress/Text").GetComponent<TranslationLiveUpdate>());
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress/Text").GetComponent<TextMeshProUGUI>().text = "Searching server...";

                        GameObject.Destroy(gameObject.transform.Find("SubscriptionError/Text").GetComponent<TranslationLiveUpdate>());
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError/Text").GetComponent<TextMeshProUGUI>().text = "Server not found";

                        gameObject.FindChild("Subscribe").GetComponent<Button>().onClick.AddListener(() =>
                        {
                            //GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(true);
                            InitializeConnection test = new InitializeConnection();
                            try
                            {
                                MainPatcher.username = gameObject2.FindChild("InputField").GetComponent<TMP_InputField>().text;
                                test.start(gameObject.FindChild("InputField").GetComponent<TMP_InputField>().text, MainPatcher.username);
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(false);

                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionSuccess").SetActive(true);
                            }
                            catch (SocketException)
                            {
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(false);

                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError").SetActive(true);
                            }
                            catch (FormatException)
                            {
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(false);

                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError/Text").GetComponent<TextMeshProUGUI>().text = "Invalid address ip";
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError").SetActive(true);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(false);

                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError/Text").GetComponent<TextMeshProUGUI>().text = "No port specified or wrong ip address format";
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError").SetActive(true);
                            }
                            catch (Exception)
                            {
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionInProgress").SetActive(false);

                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError/Text").GetComponent<TextMeshProUGUI>().text = "Uknown error occured.";
                                GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/SubscriptionError").SetActive(true);
                            }
                        });

                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/HeaderText").GetComponent<TextMeshProUGUI>().text = "Join server: (User ID: " + MainPatcher.id + ")";
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/InputField/Placeholder").GetComponent<TextMeshProUGUI>().text = "Enter the ip adress of the server...";
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/InputField").GetComponent<TMP_InputField>().text = MainPatcher.configFile["server"].ToString();

                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu/HeaderText").GetComponent<TextMeshProUGUI>().text = "Username: ";
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu/InputField/Placeholder").GetComponent<TextMeshProUGUI>().text = "Enter your new nickname";
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu/InputField").GetComponent<TMP_InputField>().text = MainPatcher.username;

                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu/ViewPastUpdates/").SetActive(false);
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu/ViewPastUpdates/").SetActive(false);
                        GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu/Subscribe/").SetActive(false);

                        gameObject.SetActive(true);
                        gameObject2.SetActive(true);
                    }
                }
                else
                {
                    GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/MultiplayerMenu").SetActive(target == "SavedGames");
                    GameObject.Find("Menu canvas/Panel/MainMenu/RightSide/NicknameMenu").SetActive(target == "SavedGames");
                }
            }                    
        }
    }
}

