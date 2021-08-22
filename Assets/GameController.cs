using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject SplashCanvas;
    [SerializeField] private GameObject SplashCamera;

	private void Awake()
	{
        SplashCanvas.SetActive(true);
        SplashCamera.SetActive(true);
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
	{
        SplashCanvas.SetActive(false);
        SplashCamera.SetActive(false);
    }
}
