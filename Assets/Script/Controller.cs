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

public class Controller : Setup
{
	public Transform rod1;
	public Transform rod2;

	public FixedScale bob1;
	public FixedScale bob2;

	protected double t1;
	protected double t2;
	private double w1;
	private double w2;

	public override void Start()
	{
		base.Start();
		t1 = rod1.rotation.eulerAngles.z * Mathf.Deg2Rad;
		t2 = (rod2.rotation.eulerAngles.z) * Mathf.Deg2Rad;
		w1 = 0.0;
		w2 = 0.0;
		UpdateScale();
		UpdateMass();
	}

	array4 getABs(array4 para)
	{
		double t1d = para.a1;
		double t2d = para.a2;
		double w1d = para.a3;
		double w2d = para.a4;

		double A1 = l2 / l1 * (m2 / (m1 + m2)) * Math.Cos(t1d - t2d);
		double A2 = l1 / l2 * Math.Cos(t1d - t2d);

		double B1 = -l2 / l1 * Math.Pow(w2d, 2.0) * (m2 / (m1 + m2)) * Math.Sin(t1d - t2d) - g / l1 * Math.Sin(t1d);
		double B2 = l1 / l2 * Math.Pow(w1d, 2.0) * Math.Sin(t1d - t2d) - g / l2 * Math.Sin(t2d);

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
		double ke1 = 0.5 * m1 * Math.Pow(l1 * w1, 2.0) + m2 * l1 * l2 * w1 * w2 * Math.Cos(t1 - t2);
		double ke2 = 0.5 * m2 * (Math.Pow(l1 * w1, 2.0) + Math.Pow(l2 * w2, 2.0));
		return ke1 + ke2;
	}

	double PotentialEnergy()
	{
		return -(m1 + m2) * g * l1 * Math.Cos(t1) - m2 * g * l2 * Math.Cos(t2);
	}

	array4 getNxtRangKutta(array4 y)
	{
		array4 k1 = getDummy(y);
		array4 k2 = getDummy(y + k1 * (double)(dt / 2.0));
		array4 k3 = getDummy(y + k2 * (double)(dt / 2.0));
		array4 k4 = getDummy(y + k3 * (double)dt);

		return (k1 + k2 * 2.0 + k3 * 2.0 + k4) * (dt / 6.0);
	}

	array4 getNxtNaive(array4 y)
	{
		array4 vs = getDummy(y);
		double g1 = vs.a3;
		double g2 = vs.a4;
		double w1 = vs.a1;
		double w2 = vs.a2;
		double dw1 = g1 * dt;
		double dw2 = g2 * dt;
		double dt1 = dt * (w1 + dw1 / 2.0);
		double dt2 = dt * (w2 + dw2 / 2.0);
		return new array4(dt1, dt2, dw1, dw2);
	}

	public void UpdateScale()
	{
		l1 = rod1.localScale.y;
		l2 = rod2.localScale.y;
	}

	public void UpdateMass()
	{
		m1 = bob1.actualVal;
		m2 = bob2.actualVal;
	}

	void Update()
	{
		if (!bool_Paused)
		{
			for (int i = 0; i < n; i++)
			{
				array4 R = getNxtRangKutta(new array4(t1, t2, w1, w2));
				t1 += R.a1;
				t2 += R.a2;
				w1 += R.a3;
				w2 += R.a4;
			}
		}
		rod1.rotation = Quaternion.Euler(0, 0, (float)t1 * Mathf.Rad2Deg);
		rod2.rotation = Quaternion.Euler(0, 0, (float)t2 * Mathf.Rad2Deg);
	}

	public void updateTheta1(string t)
	{
		float d = float.Parse(t) % (360);
		t1 = d * Mathf.Deg2Rad;
		rod1.rotation = Quaternion.Euler(0, 0, d);
	}

	public void updateTheta2(string t)
	{
		float d = float.Parse(t) % (360);
		t2 = d * Mathf.Deg2Rad;
		rod2.rotation = Quaternion.Euler(0, 0, d);
	}

	public void updateOmega1(string t)
	{
		w1 = double.Parse(t);
	}

	public void updateOmega2(string t)
	{
		w2 = double.Parse(t);
	}
}
