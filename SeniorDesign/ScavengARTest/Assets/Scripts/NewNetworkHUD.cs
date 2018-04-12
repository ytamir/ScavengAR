#if ENABLE_UNET

using UnityEngine.UI;
using UnityEngine.Networking.Match;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NewNetworkHUD : MonoBehaviour
	{
		public NetworkManager manager;
        public GameObject MatchMakeButton;
        public GameObject AvailableGames;
        public GameObject GameListMenu;
        public GameObject WaitingMenu;
        public GameObject GameDrive;
        public GameObject waitText;
        public GameObject usernameText;
        public GameObject errorUserText;

		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		// Runtime variable
		bool showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

        public void EnableMatchMaking()
        {
            if(usernameText.GetComponent<Text>().text.Equals(""))
            {
                errorUserText.SetActive(true);
            }else
            {
                manager.StartMatchMaker();
            }
            
        }

        public void GenerateMatch()
        {
            manager.matchName = GameObject.Find("RoomNameText").GetComponent<Text>().text;
            //manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, GameObject.Find("RoomPassText").GetComponent<Text>().text, "", "", 0, 0, manager.OnMatchCreate);
            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
            //GameDrive.GetComponent<GameDriver>().CmdSetPlayerName(usernameText.GetComponent<Text>().text);

        }

        public void ShowMatches()
        {
            manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
            int ypos = 500;
            int spacing = 600;
            if(manager.matches != null)
            {
                foreach (Transform child in AvailableGames.transform)
                {
                    if(child.gameObject.name != "Refresh")
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
                foreach (var match in manager.matches)
                {
                    GameObject newButton = Instantiate(MatchMakeButton, AvailableGames.transform) as GameObject;
                    newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ypos);
                    ypos -= 200;
                    newButton.GetComponentInChildren<Text>().text = match.name;
                    newButton.name = match.name;
                    newButton.transform.SetParent(AvailableGames.transform);
                    newButton.GetComponent<Button>().onClick.AddListener(() => JoinGame(match.name, (uint)match.currentSize, match.networkId));
                    newButton.GetComponent<Button>().onClick.AddListener(() => GameListMenu.SetActive(false));
                    newButton.GetComponent<Button>().onClick.AddListener(() => WaitingMenu.SetActive(true));
                }
                ypos = 500;
            }
        }

        public void JoinGame(string name, uint size, Types.NetworkID id)
        {
            manager.matchName = name;
            manager.matchSize = size;
            manager.matchMaker.JoinMatch(id, "", "", "", 0, 0, manager.OnMatchJoined);
            waitText.GetComponent<Text>().text = "You are player " + usernameText.GetComponent<Text>().text + "\n Please wait for host to start the game.";
        }

        /*public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchinfo)
        {
            Debug.Log("on match joined");
            if(success)
            {
                GameDrive.GetComponent<GameDriver>().CmdSetPlayerName(usernameText.GetComponent<Text>().text);
            }

        }*/

		void OnGUI()
		{
			if (!showGUI)
				return;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 24;

			/*if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
					ypos += spacing;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					ypos += spacing;
				}
			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
				
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}
            */
			/*if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100, 20), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
							{
								manager.matchMaker.ListMatches(0,20, "", true, 0, 0, manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

					GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += spacing;

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}*/
		}
	}
};
#endif //ENABLE_UNET
