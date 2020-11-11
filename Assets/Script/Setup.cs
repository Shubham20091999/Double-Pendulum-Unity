using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
	public double g = 9.81;
	public double n = 10;

	[HideInInspector]
	public double l1 = 1.0;
	[HideInInspector]
	public double l2 = 1.0;

	[HideInInspector]
	public double m1 = 1.0;
	[HideInInspector]
	public double m2 = 1.0;

	[HideInInspector]
	public double st1;
	[HideInInspector]
	public double st2;

	[HideInInspector]
	public double sw1;
	[HideInInspector]
	public double sw2;

	public Sprite paused;
	public Sprite playing;
	public Image play_pause;


	public InputField length1Input;
	public InputField length2Input;
	public InputField weight1Input;
	public InputField weight2Input;
	public Slider delta1Slider;
	public Text delta1Text;
	public Slider delta2Slider;
	public Text delta2Text;


	int rate = 24;

	[HideInInspector]
	public double dt;
	[HideInInspector]
	public bool bool_Paused = true;

	public List<Configuration> configs;

	public List<DeltaController> dconfigs;

	public Text initialEnergy;
	public Text energyDifference;
	double totalEnergy;

	private void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = rate;
		dt = 1 / (double)rate / n;
		m1 = 1;
		m2 = 1;
		l1 = 1;
		l2 = 1;
		st1 = 180;
		st2 = 180;
		bool_Paused = true;
	}

	void UpdateTotalEnergy()
	{
		totalEnergy = configs[0].controller.KineticEnergy() + configs[0].controller.PotentialEnergy();
		initialEnergy.text = "Initial: " + Math.Round(totalEnergy, 6).ToString();
	}
	public void Start()
	{
		UpdateTotalEnergy();
	}

	public void pause()
	{
		bool_Paused = !bool_Paused;
		if (bool_Paused)
		{
			play_pause.sprite = playing;
		}
		else
		{
			play_pause.sprite = paused;
		}
	}


	private double UpdateValue(double newlen, double prelen)
	{
		if (newlen <= 0.0001)
		{
			return prelen;
		}
		else
		{
			return newlen;
		}
	}
	public void reset()
	{
		if(!bool_Paused)
			pause();
		foreach (Configuration c in configs)
		{
			c.controller.w1 = sw1 * Mathf.Deg2Rad;
			c.controller.w2 = sw2 * Mathf.Deg2Rad;
			c.controller.t1 = st1 * Mathf.Deg2Rad;
			c.controller.t2 = st2 * Mathf.Deg2Rad;
		}

		foreach (DeltaController d in dconfigs)
		{
			d.t1 += d.dt1;
			d.t2 += d.dt2;
		}
	}
	public void updateL1(string s)
	{
		double templ1 = UpdateValue(double.Parse(s), l1);

		if (templ1 != l1)
		{
			l1 = templ1;
			foreach (Configuration c in configs)
			{
				c.rod1.localScale = new Vector3(1, (float)l1, 1);
				c.bob1.localScale = new Vector3(1, 1 / (float)l1, 1);
			}
			UpdateTotalEnergy();
		}
		else
		{
			length1Input.text = l1.ToString();
		}
	}


	public void updateL2(string s)
	{
		double templ2 = UpdateValue(double.Parse(s), l2);

		if (templ2 != l2)
		{
			l2 = templ2;
			foreach (Configuration c in configs)
			{
				c.rod2.localScale = new Vector3(1, (float)l2, 1);
				c.bob2.localScale = new Vector3(1, 1 / (float)l2, 1);
			}
			UpdateTotalEnergy();
		}
		else
		{
			length2Input.text = l2.ToString();
		}
	}

	public void UpdateM1(string s)
	{
		double tempm1 = UpdateValue(double.Parse(s), m1);

		if (tempm1 != m1)
		{
			m1 = tempm1;
			UpdateTotalEnergy();
		}
		else
		{
			weight1Input.text = m1.ToString();
		}
	}

	public void UpdateM2(string s)
	{
		double tempm2 = UpdateValue(double.Parse(s), m2);

		if (tempm2 != m2)
		{
			m2 = tempm2;
			UpdateTotalEnergy();
		}
		else
		{
			weight2Input.text = m2.ToString();

		}

	}

	public void UpdateST1(string t)
	{
		float d = float.Parse(t) % (360);
		st1 = d;

		foreach (Configuration c in configs)
		{
			c.rod1.rotation = Quaternion.Euler(0, 0, d);
			c.controller.t1 = st1 * Mathf.Deg2Rad;
		}
		UpdateTotalEnergy();
	}

	public void UpdateST2(string t)
	{
		float d = float.Parse(t) % (360);
		st2 = d;

		foreach (Configuration c in configs)
		{
			c.rod2.rotation = Quaternion.Euler(0, 0, d);
			c.controller.t2 = st2 * Mathf.Deg2Rad;
		}
		UpdateTotalEnergy();
	}

	public void UpdateSW1(string t)
	{
		float d = float.Parse(t);
		sw1 = d * Mathf.Deg2Rad;
		foreach (Configuration c in configs)
		{
			c.controller.w1 = sw1;
		}
		UpdateTotalEnergy();
	}

	public void UpdateSW2(string t)
	{
		float d = float.Parse(t);
		sw2 = d * Mathf.Deg2Rad;
		foreach (Configuration c in configs)
		{
			c.controller.w2 = sw2;
		}
		UpdateTotalEnergy();
	}

	public void UpdateDelta1(float v)
	{
		delta1Text.text = Math.Round(delta1Slider.value, 3).ToString();
		foreach (DeltaController d in dconfigs)
		{
			d.updateT1(v);
		}
	}

	public void UpdateDelta2(float v)
	{
		delta2Text.text = Math.Round(delta2Slider.value, 3).ToString();
		foreach (DeltaController d in dconfigs)
		{
			d.updateT2(v);
		}
	}

	private void Update()
	{
		energyDifference.text = "Current Difference: " + Math.Round(configs[0].controller.KineticEnergy() + configs[0].controller.PotentialEnergy() - totalEnergy,6).ToString();
	}
}