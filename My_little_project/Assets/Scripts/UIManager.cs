using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    private NetworkVariable<int> playerID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public TMP_Text idText;
    // Start is called before the first frame update
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        StartCoroutine(CheckPlayers());


    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        StartCoroutine(CheckPlayers());
    }

    public void GetID()
    {
        idText.text = string.Format("Client ID: {0}", playerID.Value);
    }

    private void Update()
    {
    }

    private IEnumerator CheckPlayers()
    {
        while(IsServer)
        {
            GetID();
            playerID.Value = NetworkManager.Singleton.ConnectedClients.Count;
            yield return new WaitForSeconds(0.25f);
        }

        while(!IsServer)
        {
            GetID();
            yield return new WaitForSeconds(0.25f);
        }
        yield return null;
    }
}
