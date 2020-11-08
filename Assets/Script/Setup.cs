using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
	public double g = 9.81;
	public double n = 10;
	public bool bool_Paused = false;
	public Sprite paused;
	public Sprite playing;
	public Image play_pause;
	int rate = 24;
	public double dt;

	private void Start()
	{
		Application.targetFrameRate = rate;
		dt = 1 / (double)rate / n;
	}

	public void pause()
	{
		bool_Paused = !bool_Paused;
		if (bool_Paused)
		{
			play_pause.sprite = paused;
		}
		else
		{
			play_pause.sprite = playing;
		}
	}
}