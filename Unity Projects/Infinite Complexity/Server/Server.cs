using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;
    private Dictionary<string, ServerClient> nameToClient;
 //   public List<string> allNames;


    public int port = 6321;
    private TcpListener server;
    private bool serverStarted;

    private ServerClient savedDefender;
    private bool hpUpdateNeeded;

    private string[] Alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    private char[] Symbols = new char[13] { '!', '@', '#', '$', '%', '^', '&', '*', '+', '=', '~', ':', '?' };

    [SerializeField]
    private int passwordLength = 6;
    [SerializeField]
    private int logCount = 15;
    [SerializeField]
    private float passwordChangeTime = 300.0f;
    [SerializeField]
    private float saveTime = 1800.0f;

    [SerializeField]
    private int mapLength;
    [SerializeField]
    private int mapWidth;

    private mapPieceServer[][] map;

    [SerializeField]
    private float getResourcesTime = 60.0f;

    [SerializeField]
    private bool createNewMap, wipePlayers;
    [SerializeField]
    private int saveSize = 5;
    [SerializeField]
    private int startingCredits = 10000;
    [SerializeField]
    private int heavyStrength, mediumStrength, lightStrength;

    

    private void Start()
    {
     //   Debug.Log("Starting Server");
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();
        nameToClient = new Dictionary<string, ServerClient>(StringComparer.InvariantCultureIgnoreCase);

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log("Server has started on port " + port.ToString());
        }catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }

        Load();
        
        //     allNames.Add(clients[clients.Count - 1].clientName);

        InvokeRepeating("ChangePasswords", 5.0f, passwordChangeTime);
        InvokeRepeating("GetResources", getResourcesTime, getResourcesTime);
        InvokeRepeating("AutoSave", saveTime, saveTime);
  //      Save();
    }

    private void Update()
    {
        if (!serverStarted)
            return;
        try
        {
            foreach (ServerClient c in clients)
            {
                if(c.tcp != null)
                {
                    // Is the client still connected?
                    if (!IsConnected(c.tcp))
                    {
                        c.tcp.Close();
                        clients.Remove(c);
                        disconnectList.Add(c);
                        continue;
                    }
                    // Check for message from the client
                    else
                    {
                        NetworkStream s = c.tcp.GetStream();

                        if (s.DataAvailable)
                        {
                            StreamReader reader = new StreamReader(s, true);
                            string data;
                            //            while ((data = reader.ReadLine()) != null)
                            //          {
                            data = reader.ReadLine();
                    //        Debug.Log(data);
                            OnIncomingData(c, data);
                            //         }

                        }
                    }
                }
                
            }
            /*
            for (int i = 0; i < disconnectList.Count - 1; i++)
            {
                Broadcast(disconnectList[i].clientName + " has disconnected.", clients);

                clients.Remove(disconnectList[i]);
                nameToClient.Remove(disconnectList[i].clientName);
                disconnectList.RemoveAt(i);
            } */
        }
        catch(Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        
    } 

    private void Save()
    {
        string[] JsonString = new string[mapWidth/saveSize];
     
        JsonString = JsonHelper.ToJson(map, saveSize);
        string JsonUserString = "";
        int userCount = 0;
        int index = 0;
        foreach(KeyValuePair<string,ServerClient> client in nameToClient)
        {
            userCount++;
            JsonUserString += JsonUtility.ToJson(client.Value) + "|";
            if (userCount % 5 == 0) // Find a more permanent solution 
            {
             //   Debug.Log(JsonUserString);
                PlayerPrefs.SetString("users"+index.ToString(), JsonUserString);
                JsonUserString = "";
                index++;
            }

        }
 //       Debug.Log(JsonUserString);
        PlayerPrefs.SetString("users" + index.ToString(), JsonUserString);
        PlayerPrefs.SetInt("userAmount", index);


        for (int i = 0; i < (mapWidth / saveSize); i++)
        {
 //           Debug.Log(JsonString[i]);
            PlayerPrefs.SetString("map"+i.ToString(), JsonString[i]);
        }
    }
    private void Load()
    {
        if(wipePlayers || !PlayerPrefs.HasKey("users0"))
        {
            CreateDefaultUsers();
        }
        else
        {
            string[] userList = null;
            string JsonString = "";
            ServerClient client = null;
            for (int i = 0; i <= PlayerPrefs.GetInt("userAmount"); i++)
            {
                if (PlayerPrefs.HasKey("users" + i.ToString()))
                {
                    JsonString = PlayerPrefs.GetString("users" + i.ToString());
                    userList = JsonString.Split('|');
                    foreach (string user in userList)
                    {
                        if (user.Length>2)
                        {
                            client = JsonUtility.FromJson<ServerClient>(user);
                            nameToClient.Add(client.clientName, client);
                        }
                        
                    }
                }
            }
        }

        if (createNewMap || !PlayerPrefs.HasKey("map0"))
        {
            CreateMap();
        }
        else
        {
            //     Debug.Log("Tried to load");
            string[] JsonStrings = new string[mapWidth / saveSize];
            for (int i = 0; i < (mapWidth / saveSize); i++)
            {
                JsonStrings[i] = PlayerPrefs.GetString("map" + i.ToString());
            //             Debug.Log(JsonStrings[i]);
            }
            map = JsonHelper.FromJson<mapPieceServer>(JsonStrings, saveSize);
        }
        if(createNewMap || !PlayerPrefs.HasKey("map0") || wipePlayers || !PlayerPrefs.HasKey("users")) 
            Save();
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient,server);
    }
    
    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
        
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar), "Guest","Password", startingCredits));
        StartListening();

        //Send a message to everyone, say someone has connected
        Broadcast("%NAME",new List<ServerClient>() { clients[clients.Count - 1] });
        
    }
    private void Connect(ServerClient c, string user)
    {

        ServerClient client = nameToClient[user];
        client.tcp = c.tcp;
        clients.Remove(c);
        clients.Add(client);
        Broadcast(user + " has connected!", clients);
        ChangePassword(client);
    }
    private void FailedConenct(ServerClient c)
    {
        try
        {
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            writer.WriteLine("99999");
            writer.Flush();
            clients.Remove(clients[clients.Count - 1]);
            Debug.Log("Wrote the Command to Disconnect");
        }
        catch (Exception e)
        {
            Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
        }
    }
    private void Register(ServerClient c, string user, string pass)
    {
        c.clientName = user;
        c.clientPassword = pass;
        Broadcast(user + " has connected!", clients);
        nameToClient.Add(user,c);
        clients.Add(c);
        ChangePassword(c);

    }

    private void OnIncomingData(ServerClient c, string data)
    {
   //     Debug.Log(data);
        if (data.Length>=5 && data.Substring(0,5).Equals("&NAME"))
        {
            
            string[] info = data.Split('|');
            string user = info[1];
            string pass = info[2];

            if (nameToClient.ContainsKey(user))
            {
                //           Broadcast("The name " + user + " has already been taken", new List<ServerClient>() { c });
      //          Debug.Log("Attempted password: "+pass+", Real password: "+nameToClient[user].GetClientPassword());
                if (pass.Equals(nameToClient[user].GetClientPassword()))
                {
                    Debug.Log("Logged in as "+user);
                    Broadcast("099", nameToClient[user].clientName, new List<ServerClient>() { c });
                    Connect(c, user);
                }
                else
                {
                    Debug.Log("Wrong password: " + pass);
                    Broadcast("299", "", new List<ServerClient>() { c });
                    FailedConenct(c);
                }
             //   Broadcast("The name " + user + " has already been taken", new List<ServerClient>() { c });
                
            }
            else
            {
                Debug.Log("Wrong Username: " + user);
                Broadcast("199", "", new List<ServerClient>() { c });
                FailedConenct(c);
            }
            return;
        }
        if (data.Length >= 6 && data.Substring(0, 6).Equals("&&NAME"))
        {

            string[] info = data.Split('|');
            string user = info[1];
            string pass = info[2];

            if (nameToClient.ContainsKey(user))
            {
                Debug.Log("Username Taken: " + user);
                Broadcast("399", "", new List<ServerClient>() { c });
                FailedConenct(c);
            }
            else
            {
                Debug.Log("Registered as: " + user);
                Broadcast("099", user, new List<ServerClient>() { c });
                Register(c, user, pass);
                Save();
            }
            return;
        }
        int selection = Int32.Parse(data.Substring(0, 3));
        switch (selection)
        {
            case 3:
                CheckPlayerOnline(data.Substring(3),c);
                break;
            case 4:
                string selection2 = data.Substring(3, 1);
   //             Debug.Log(selection2);
                switch (selection2)
                {
                    case "P":
                        ExtractPassword(data.Substring(4), c);
                        break;
                    case "L":
                        ExtractLogs(data.Substring(4), c);
                        break;
                    case "T":
                        TestPassword(data.Substring(4), c);
                        break;
                    default:
                        Debug.Log("I messed up trying to extract something!");
                        break;
                }
                break;
            case 5:
                int creditChange = Int32.Parse(data.Substring(3));
                c.ChangeCredits(creditChange);
                Save();
                UpdateCredits(c.GetCredits(),c);
                break;
            case 6: UpdateMap(c);
                break;
            case 16: UpdateMapPiece(data.Substring(3), c);
                break;

            default: Broadcast("That command does NOTHING", new List<ServerClient>() { c });
                break;
        }
      /*  if (nameToClient.ContainsKey(data)) //// THIS IS WHERE THE DATA IS (delete it all)
            Attack(c, data);
        else
        Broadcast(c.clientName+" : attacked the air!", clients); */
    }



    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach(ServerClient c in cl)
        {
            try
            {
                if (c.tcp != null)
                {
                    StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                    writer.WriteLine("000" + data);
                    writer.Flush();
                }
                
            }
            catch(Exception e)
            {
                Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
            }
        }
    }

    private void Broadcast(string code,string data, List<ServerClient> cl)
    {
        foreach (ServerClient c in cl)
        {
            try
            {
                if (c.tcp != null)
                {
                    StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                    writer.WriteLine(code + data);
                    writer.Flush();
                }

            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
            }
        }
    }

    private void AutoSave()
    {
        Save();
    }

    private void CreateDefaultUsers()
    {
        clients.Add(new ServerClient("Test_Server", 10000));
        nameToClient.Add(clients[clients.Count - 1].clientName, clients[clients.Count - 1]);

        //Temp user before registering works
   //     clients.Add(new ServerClient(null, "ADMIN", "SF<OM", 14923));
        nameToClient.Add("ADMIN", new ServerClient(null, "ADMIN", "SF<OM", 14923));
    }

    private void UpdateCredits(int data, ServerClient c)
    {
      //  Debug.Log("Username: "+c.clientName+" - "+"TCP Status: "+ (c.tcp != null).ToString());
        if (c.tcp == null)
            return;
  //      Debug.Log("Is a Player: "+c.isPlayer.ToString());
        if (!c.IsPlayerCheck()) // If not a player, dont update their currency counter
         return;
        //      Debug.Log("Credits: " + c.GetCredits());
        //      Debug.Log("Should of added: " + c.GetResources());
        Broadcast("001",data.ToString(), new List<ServerClient>() { c });
 //       Debug.Log("Wrote " + data.ToString() + " to " + c.clientName);
    }

    private void UpdateMap( ServerClient c)
    {
        try
        {
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            string data = GetMapInfo();
            writer.WriteLine("006" + data);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
        }
    }

    private void UpdateMapPiece(string data, ServerClient c)
    {
        try
        {
            String[] realData = data.Split(' ');
            String[] ID = realData[3].Split('-');
            map[Int32.Parse(ID[0])][Int32.Parse(ID[1])].create(data.Split(' '));
            c.addMapPiece(map[Int32.Parse(ID[0])][Int32.Parse(ID[1])]);
        } catch (Exception e) { Debug.Log("In Server-UpdateMapPiece: data sent incorrectly: "+e); }

        Save();
        Broadcast("016", data, clients);
    }

    private void ChangePasswords()
    {

        foreach (ServerClient client in clients)
        {
            ChangePassword(client);
        }
    }
    private void ChangePassword(ServerClient client)
    {
        string  newRealPassword = "";
        string newLog = "Password Encryption starting...|";
        string newEncodedPassword = "";

        for (int i = 0; i < passwordLength; i++)
            newRealPassword += Alphabet[UnityEngine.Random.Range(0, Alphabet.Length)];
        newEncodedPassword = newRealPassword;
 //       Debug.Log(newRealPassword);
        for (int i = 0; i < logCount; i++)
        {
            char firstThing = newEncodedPassword[UnityEngine.Random.Range(0, newEncodedPassword.Length)];
            char secondThing = Symbols[UnityEngine.Random.Range(0, Symbols.Length)];
            while (newEncodedPassword.Contains("" + secondThing))
            {
                secondThing = Symbols[UnityEngine.Random.Range(0, Symbols.Length)];
            }
            newEncodedPassword = newEncodedPassword.Replace(firstThing, secondThing);
   //         Debug.Log(firstThing + " was replaced with " + secondThing );
            newLog += firstThing + " was replaced with " + secondThing + "|";
            
        }
        newLog += "Password Encryption finished!";
   //     Debug.Log(newEncodedPassword);
        client.UpdatePassword(newRealPassword, newEncodedPassword, newLog);
    }

    private void GetResources()
    {
        foreach (KeyValuePair<string,ServerClient> client in nameToClient)
        {
            client.Value.ChangeCredits(client.Value.GetResources());
            UpdateCredits(client.Value.GetCredits(), client.Value);
        }
    }

    private void CheckPlayerOnline(string name, ServerClient c)
    {
        if (nameToClient.ContainsKey(name))
        {
            Send("003", new List<ServerClient> { c });
        }
        else
        {
            Broadcast("That User is either not Online, or does not Exist!", new List<ServerClient> { c });
        }

    }

    private void ExtractPassword(string name, ServerClient c)
    {
        ServerClient target = null;
        nameToClient.TryGetValue(name, out target);
        Broadcast(target.GetPassword(), new List<ServerClient> { c });
    }
    private void ExtractLogs(string name, ServerClient c)
    {
        ServerClient target = null;
        nameToClient.TryGetValue(name, out target);
        Send("010"+target.GetLog(), new List<ServerClient> { c });
    }
    private void TestPassword(string data, ServerClient c)
    {
        try
        {
            string guess = data.Substring(0, passwordLength);
            ServerClient target = null;
            nameToClient.TryGetValue(data.Substring(passwordLength), out target);
            if (target.CheckPassword(guess))
            {
                Send("004", new List<ServerClient> { c });
            }
            else
            {
                Broadcast("Incorrect Password!", new List<ServerClient> { c });
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            Broadcast("Incorrect Password!", new List<ServerClient> { c });
        }
    }

    private void Send(string data, List<ServerClient> cl)
    {
        foreach (ServerClient c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
            }
        }
    }

    private void CreateMap()
    {
        map = new mapPieceServer[mapWidth][];
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new mapPieceServer[mapLength];
        }
        //  CreateFreeBallRiver(); // Near Random river of near random length at near random location. FEW RESITRCTIONS
        CreateCuttingRiver(); //River that stretches entire map left to right, cuts the map in half.
        // FILLS EMPTY SPOTS
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (map[i][j] == null)
                {
                    map[i][j] = new mapPieceServer("Wasteland", 0, "None", "" + i.ToString() + "-" + j.ToString()); // Fill all empty zones with Wasteland
              /*      map[i][j].rH=(int)Math.Round((double)UnityEngine.Random.Range(0, 2));
                    map[i][j].tH = (int)Math.Round((double)UnityEngine.Random.Range(0, 2));
                    map[i][j].lH = (int)Math.Round((double)UnityEngine.Random.Range(0, 2));
                    map[i][j].bH = (int)Math.Round((double)UnityEngine.Random.Range(0, 2)); */
                }
            }
        }
   //     Save();




   /*//     Debug.Log("Server should make "+ (mapLength*mapWidth)+" many map pieces");
        int count = 0;
        for(int i=0; i < mapLength; i++)
        {
            for(int j=0; j< mapWidth; j++)
            {
                int landTypeNum = (int)Math.Round((double)UnityEngine.Random.Range(0,4)); 
                string landType = "Wasteland";
                switch (landTypeNum)
                {
                    case 0: landType = "Wasteland"; break;
                    case 1: landType = "Grassland"; break;
                    case 2: landType = "Water"; break;
                    case 3: landType = "Mountain"; break;

                }
                map[i, j] = new mapPieceServer(landType,0,"None",""+i.ToString()+"-"+j.ToString()); // Creates a new map Piece from scratch
                count++;
            }
        }
//        Debug.Log("Server is making " + (count) + " many map pieces"); */
    }
    private void CreateFreeBallRiver()
    {
        //FreeBall Style
        //RIVER STARTS HERE
        int randomSpotLength = (int)Math.Round(UnityEngine.Random.Range(0, mapLength * 0.8f));
        int randomSpotWidth = (int)Math.Round(UnityEngine.Random.Range(0, mapWidth * 0.8f));

        int riverWidth = 0;
        if (UnityEngine.Random.Range(0, 1) < 1) // 100% chance to be width of 2
            riverWidth = 2;
        else  // otherwise can be 1 through 3 width
            riverWidth = (int)Math.Round((double)UnityEngine.Random.Range(0, 3)) + 1;

        int i = randomSpotWidth;
        int j = randomSpotLength;
        bool riverGoing = true;
        float chanceToStop = 0;
        float increaseStop = 0;
        float riverSwitch = 0;
        bool recentlyTurned = false;
        Debug.Log("Should start at " + i + " - " + j);
        Debug.Log("River Width = " + riverWidth);
        while (riverGoing)
        {
            Debug.Log("Currently at " + i + " - " + j);
            Debug.Log("while going - " + chanceToStop);
            Debug.Log("if " + j + "<" + mapLength);
            if (j < mapLength)
            {
                {
                    for (int x = i; x < i + riverWidth; x++)// goes down up to three times depending on riverWidth
                    {
                        Debug.Log("if " + x + "<" + mapWidth);
                        if (x < mapWidth)
                        {
                            map[x][j] = new mapPieceServer("Water", 0, "None", "" + x.ToString() + "-" + j.ToString()); // Creates a new map Piece from scratch
                            Debug.Log("Made water at " + x + " - " + j);
                        }
                    }
                }



                riverSwitch = UnityEngine.Random.Range(0, 4); // 25 percent chance to either go down or go up by one block
                if (!recentlyTurned)
                {
                    if (riverSwitch <= 1.0)
                        i++;
                    else
                        if (riverSwitch >= 3.0)
                        i--;
                    recentlyTurned = true;
                }
                else { recentlyTurned = false; }

                /*    if (i < 0)
                        i++;
                    if (i >= riverWidth)
                        i--; */ // This code corrects out of bounds, possible bug tho
                if (chanceToStop > UnityEngine.Random.Range(0, 100))
                    riverGoing = false;
                increaseStop += 1.0f; // percent increase to stop goes up by 1 percent each time, in theory, chanceToStop goes from 0, to 1, to 3, to 6, to 10.
                chanceToStop += increaseStop; //Simple exponential growth.
                j++;

            }
            else
            {
                riverGoing = false;
            }

        }
        // RIVER STOPS HERE
    }
    private void CreateCuttingRiver()
    {
        //Cutting Style

        int riverWidth = 0;
        if (UnityEngine.Random.Range(0, 1) < 1) // 100% chance to be width of 2
            riverWidth = 2;
        else  // otherwise can be 1 through 3 width
            riverWidth = (int)Math.Round((double)UnityEngine.Random.Range(0, 3)) + 1;

        int i = (int)Math.Round(UnityEngine.Random.Range(mapWidth * 0.3f, mapWidth * 0.7f));
       
        float riverSwitch = 0;
        bool recentlyTurned = false;
 //       Debug.Log("Should start at " + i + " - " + 0);
  //      Debug.Log("River Width = " + riverWidth);
        for(int j=0; j<mapLength;j++) // start at the left part of the map, and go through
        {
    //        Debug.Log("Currently at " + i + " - " + j);
      //      Debug.Log("if " + j + "<" + mapLength);
                {
                    for (int x = i; x < i + riverWidth; x++)// goes down up to three times depending on riverWidth
                    {
                //        Debug.Log("if " + x + "<" + mapWidth);
                        if (x < mapWidth)
                        {
                            map[x][j] = new mapPieceServer("Water", 0, "None", "" + x.ToString() + "-" + j.ToString()); // Creates a new map Piece from scratch
           //                 Debug.Log("Made water at " + x + " - " + j);
                        }
                    }
                }



                riverSwitch = UnityEngine.Random.Range(0, 10); // percent chance to either go down or go up by one block
          
                if (!recentlyTurned)
                {
                //      Debug.Log("Turn number - "+riverSwitch);
                    if (riverSwitch <= 2)
                        i++;
                    else
                        if (riverSwitch >= 8.5)
                        i--;
                    recentlyTurned = true;
                }
                else { recentlyTurned = false; }

                /*    if (i < 0)
                        i++;
                    if (i >= riverWidth)
                        i--; */ // This code corrects out of bounds, possible bug tho

            }
    }

    private String GetMapInfo()
    {
        String mapInfo = "";
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                mapInfo+= map[i][j].DataInfo()+":";
            }
        }
 //       Debug.Log("Sending "+temp.Length+" map Pieces");
        return mapInfo;
    }
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;
    public string clientPassword;
    public int credits;
    public bool isPlayer;
    public mapPieceServer[] ownedLand;
    public int landCount = 0;

    private string password;
    private string encodedPassword;
    private string log;

    public ServerClient(string _name, int _startingCredits)
    {
        clientName = _name;
        credits = _startingCredits;
        isPlayer = false;

        ownedLand = new mapPieceServer[landCount];
    }
    public ServerClient(TcpClient _clientSocket, string _name, string _password, int _startingCredits)
    {
        clientName = _name;
        clientPassword = _password;
        tcp = _clientSocket;
        credits = _startingCredits;
        isPlayer = true;

        ownedLand = new mapPieceServer[landCount];
    }

    public int GetCredits()
    {
        return credits;
    }
    public bool ChangeCredits(int c)
    {
   //     Debug.Log(clientName + " Added " + c + " credits.");
        if (credits + c >= 0)
        {
            credits += c;
            return true;
        }
        else
        {
            credits = 0;
            return false;
        }

    }

    public void addMapPiece(mapPieceServer data)
    {
        landCount++;
        mapPieceServer[] temp = new mapPieceServer[landCount];
        for (int x = 0; x < ownedLand.Length; x++) // go through and add existing land
        {
            temp[x] = ownedLand[x];
        }
        temp[landCount - 1] = data; // add the new piece
        ownedLand = temp; // overwrite old list
    }
    public bool removeMapPiece(mapPieceServer target)
    {
        bool removedPiece = false;
        int y = 0; // this will be a counting variable for the second list
        landCount--;
        mapPieceServer[] temp = new mapPieceServer[landCount];
        for (int x = 0; x < ownedLand.Length; x++) // go through and add existing land besides the one we are removing
        {
            if (target != ownedLand[x])
            {
                temp[y] = ownedLand[x]; // if it is not the piece we are removing, add it to the new list
                y++;
            }
            else
            {
                removedPiece = true; // otherwise simply dont
            }
        }

        if (removedPiece) // if we actually were able to remove the object, return true
        {
            ownedLand = temp; // only need to overwrite
            return true;
        }
        return false;
    }

    public int GetResources()
    {
        int total = 0;
        foreach (mapPieceServer element in ownedLand)
        {
            total += element.resourceCount;
        }
  //      Debug.Log(clientName+" Total resources: " + total);
    //    Debug.Log(clientName + " Total credits: " + credits);
        return total;
    }

    public bool IsPlayerCheck()
    {
        return isPlayer;
    }

    public string GetPassword()
    {
        return encodedPassword;
    }
    public string GetClientPassword()
    {
        return clientPassword;
    }
    public void SetClientPassword(string cp)
    {
        clientPassword = cp;
    }
    public string GetLog()
    {
        return log;
    }

    public bool CheckPassword(string guess)
    {
        if (guess.Equals(password))
            return true;
        return false;
    }

    public void UpdatePassword(string newPassword, string newEncodedPassword, string newLog)
    {
        password = newPassword;
        log = newLog;
        encodedPassword = newEncodedPassword;
    }
}
