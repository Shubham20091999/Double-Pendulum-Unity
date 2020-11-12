using UnityEngine;

public class DeltaController : Controller
{
	public Controller controller;
	public double dt1;
	public double dt2;

	override public void Start()
	{
		base.Start();
		t1 += dt1 * Mathf.Deg2Rad;
		t2 += dt2 * Mathf.Deg2Rad;
	}

	public void updateT1(float ddt1)
	{
		dt1 = ddt1 * Mathf.Deg2Rad;
		t1 = controller.t1 + dt1;
	}

	public void updateT2(float ddt2)
	{
		dt2 = ddt2 * Mathf.Deg2Rad;
		t2 = controller.t2 + dt2;
	}

}
