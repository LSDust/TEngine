using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    public class UICameraStackManager : MonoBehaviour  
    {  
        private Camera uiCamera;  
        private Camera mainCamera;  
      
        private void Awake()  
        {  
            uiCamera = GetComponent<Camera>();  
            // 监听场景切换事件  
            SceneManager.sceneLoaded += OnSceneLoaded;  
        }  
      
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)  
        {  
            if (mode == LoadSceneMode.Single)  
            {  
                // 获取新场景的主相机  
                mainCamera = Camera.main;  
                if (mainCamera != null && uiCamera != null)  
                {  
                    // 将UI相机添加到主相机的stack  
                    var stack = mainCamera.GetUniversalAdditionalCameraData();  
                    if (stack != null)  
                    {  
                        stack.cameraStack.Add(uiCamera);  
                    }  
                }  
            }  
        }  
      
        private void OnDestroy()  
        {  
            SceneManager.sceneLoaded -= OnSceneLoaded;  
        }  
    }
}
