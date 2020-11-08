using UnityEngine;

public class DeltaController : Controller
{
	public double dt1;
	public double dt2;

	override public void Start()
	{
		base.Start();
		t1 += dt1*Mathf.Deg2Rad;
		t2 += dt2*Mathf.Deg2Rad;
	}
}
