using UnityEngine;
using UnityEngine.UI;
using System;

public class ValueController : MonoBehaviour
{
	public InputField input;
	public Controller controller;

	void Start()
	{
		input.onEndEdit.AddListener(SubmitName);
	}

	private void SubmitName(string y)
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
}