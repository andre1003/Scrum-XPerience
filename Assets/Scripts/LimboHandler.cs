using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboHandler : MonoBehaviour {
    public GameObject respawnSpot;
    private void OnTriggerEnter(Collider other) {
        //if(other.GetComponent<PhotonView>().isMine) {
        //    other.gameObject.transform.position = new Vector3(respawnSpot.transform.position.x, respawnSpot.transform.position.y, respawnSpot.transform.position.z);
        //}
        other.transform.position = new Vector3(respawnSpot.transform.position.x, respawnSpot.transform.position.y, respawnSpot.transform.position.z);
    }
}
