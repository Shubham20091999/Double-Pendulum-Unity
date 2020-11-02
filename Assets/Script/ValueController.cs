using UnityEngine;
using UnityEngine.UI;
using System;

public class ValueController : MonoBehaviour
{
	public InputField input;
	public Controller controller;

	public void ChangeScaleY(string y)
	{
		string pre = transform.localScale.y.ToString();
		float f = float.Parse(y);
		if (f <= 0.0001)
		{
			input.text = pre;
		}
		else
		{
			transform.localScale = new Vector3(1, f, 1);
			controller.UpdateScale();
		}
	}

	public void ChangeScale(string z)
	{
		string pre = transform.localScale.y.ToString();
		float f = float.Parse(z);
		if (f <= 0.0001)
		{
			input.text = pre;
		}
		else
		{
			gameObject.GetComponent<FixedScale>().actualVal = f;
			controller.UpdateMass();
		}
	}
}