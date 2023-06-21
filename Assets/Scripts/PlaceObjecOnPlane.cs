using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlaceObjecOnPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid;
    private bool isPlaced = false;
    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera ARCamera;

    private void Awake(){
        raycastManager = GetComponent<ARRaycastManager>();
    }
    
    void Update(){
        
        UpdatePlacementPose();
        
        if (!isPlaced && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            PlaceObject();
        }
    }
    
    private void UpdatePlacementPose(){ 
         
        var screenCenter = ARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits,TrackableType.AllTypes);
        placementPoseIsValid = hits.Count > 0;
            
        if (placementPoseIsValid){
            placementPose = hits[0].pose;
            var cameraForward = ARCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation =Quaternion.LookRotation(cameraBearing);
            positionIndicator.transform.SetPositionAndRotation(placementPose.position,placementPose.rotation);
        }
        else{
            positionIndicator.SetActive(false);
        } 
    }
    
    private void PlaceObject(){
        Instantiate(prefabToPlace, placementPose.position, placementPose.rotation);
        isPlaced = true;
    }
}