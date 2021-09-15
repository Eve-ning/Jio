 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 
 public class PopupMessage : MonoBehaviour {
 
     public GameObject ui;
 
     // Use this for initialization
     void Start ()
     {
         ui.SetActive(false);
     }
     
     // Update is called once per frame
     void Update () {
     }
 
     /*public void Open(string inventoryStuffName, string message){
         ui.SetActive (!ui.activeSelf);
 
         if (ui.activeSelf) {
             if(!string.IsNullOrEmpty(inventoryStuffName)){
                 var texture = TakeInvenotryCollecition (inventoryStuffName);
                 RawImage rawImage = ui.gameObject.GetComponentInChildren<RawImage>();
                 rawImage.texture = texture;
             }
             if (!string.IsNullOrEmpty (message)) {
                 Text textObject = ui.gameObject.GetComponentInChildren<Text> ();
                 textObject.text = message;
             }
             Time.timeScale = 0f;
         } 
     }*/

     public void Open()
     {
         ui.SetActive(!ui.activeSelf);
         if (!ui.activeSelf) {
             Time.timeScale = 1f;
         } 
         Debug.Log("Open");
     }

     public void Close()
     {
         ui.SetActive (!ui.activeSelf);
         if (!ui.activeSelf) {
             Time.timeScale = 1f;
         }
         Debug.Log("Close");
     }
 //You need to have Folder Resources/InvenotryItems
     /*public Texture TakeInvenotryCollecition(string LoadCollectionsToInventory)
     {
         Texture loadedGO = Resources.Load("InvenotryItems/"+LoadCollectionsToInventory, typeof(Texture)) as Texture;
         return loadedGO;
     }*/
 }