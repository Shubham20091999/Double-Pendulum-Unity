using System;
using UnityEngine;

public class array4
{
	public double a1;
	public double a2;
	public double a3;
	public double a4;

	public array4(double _a1, double _a2, double _a3, double _a4)
	{
		a1 = _a1;
		a2 = _a2;
		a3 = _a3;
		a4 = _a4;
	}

	public static array4 operator +(array4 obj1, array4 obj2)
	{
		return new array4(obj1.a1 + obj2.a1, obj1.a2 + obj2.a2, obj1.a3 + obj2.a3, obj1.a4 + obj2.a4);
	}

	public static array4 operator *(array4 obj1, double a)
	{
		return new array4(obj1.a1 * a, obj1.a2 * a, obj1.a3 * a, obj1.a4 * a); ;
	}

}

public class Controller : MonoBehaviour
{
	public Setup setup;
	public Transform rod1;
	public Transform rod2;

	[HideInInspector]
	public double t1;
	[HideInInspector]
	public double t2;
	[HideInInspector]
	public double w1;
	[HideInInspector]
	public double w2;

	/*double tot;*/
	public virtual void Start()
	{
		t1 = setup.st1 * Mathf.Deg2Rad;
		t2 = setup.st2 * Mathf.Deg2Rad;
		w1 = 0.0;
		w2 = 0.0;
	}

	array4 getABs(array4 para)
	{
		double t1d = para.a1;
		double t2d = para.a2;
		double w1d = para.a3;
		double w2d = para.a4;

		double A1 = setup.l2 / setup.l1 * (setup.m2 / (setup.m1 + setup.m2)) * Math.Cos(t1d - t2d);
		double A2 = setup.l1 / setup.l2 * Math.Cos(t1d - t2d);

		double B1 = -setup.l2 / setup.l1 * Math.Pow(w2d, 2.0) * (setup.m2 / (setup.m1 + setup.m2)) * Math.Sin(t1d - t2d) - setup.g / setup.l1 * Math.Sin(t1d);
		double B2 = setup.l1 / setup.l2 * Math.Pow(w1d, 2.0) * Math.Sin(t1d - t2d) - setup.g / setup.l2 * Math.Sin(t2d);

		return new array4(A1, A2, B1, B2);
	}

	array4 getDummy(array4 para)
	{
		array4 ABs = getABs(para);
		double g1 = (ABs.a3 - ABs.a1 * ABs.a4) / (1 - ABs.a1 * ABs.a2);
		double g2 = (ABs.a4 - ABs.a2 * ABs.a3) / (1 - ABs.a1 * ABs.a2);

		return new array4(para.a3, para.a4, g1, g2);
	}

	double KineticEnergy()
	{
		double ke1 = 0.5 * setup.m1 * Math.Pow(setup.l1 * w1, 2.0) + setup.m2 * setup.l1 * setup.l2 * w1 * w2 * Math.Cos(t1 - t2);
		double ke2 = 0.5 * setup.m2 * (Math.Pow(setup.l1 * w1, 2.0) + Math.Pow(setup.l2 * w2, 2.0));
		return ke1 + ke2;
	}

	double PotentialEnergy()
	{
		return -(setup.m1 + setup.m2) * setup.g * setup.l1 * Math.Cos(t1) - setup.m2 * setup.g * setup.l2 * Math.Cos(t2);
	}

	array4 getNxtRangKutta(array4 y)
	{
		array4 k1 = getDummy(y);
		array4 k2 = getDummy(y + k1 * (double)(setup.dt / 2.0));
		array4 k3 = getDummy(y + k2 * (double)(setup.dt / 2.0));
		array4 k4 = getDummy(y + k3 * (double)setup.dt);

		return (k1 + k2 * 2.0 + k3 * 2.0 + k4) * (setup.dt / 6.0);
	}

	array4 getNxtNaive(array4 y)
	{
		array4 vs = getDummy(y);
		double g1 = vs.a3;
		double g2 = vs.a4;
		double w1 = vs.a1;
		double w2 = vs.a2;
		double dw1 = g1 * setup.dt;
		double dw2 = g2 * setup.dt;
		double dt1 = setup.dt * (w1 + dw1 / 2.0);
		double dt2 = setup.dt * (w2 + dw2 / 2.0);
		return new array4(dt1, dt2, dw1, dw2);
	}

	void Update()
	{
		if (!setup.bool_Paused)
		{
			for (int i = 0; i < setup.n; i++)
			{
				array4 R = getNxtRangKutta(new array4(t1, t2, w1, w2));
				t1 += R.a1;
				t2 += R.a2;
				w1 += R.a3;
				w2 += R.a4;
			}
			/*Debug.Log(KineticEnergy() + PotentialEnergy() - tot);*/
		}
		else
		{
			w1 = 0;
			w2 = 0;
		}
		rod1.rotation = Quaternion.Euler(0, 0, (float)t1 * Mathf.Rad2Deg);
		rod2.rotation = Quaternion.Euler(0, 0, (float)t2 * Mathf.Rad2Deg);
	}
}
