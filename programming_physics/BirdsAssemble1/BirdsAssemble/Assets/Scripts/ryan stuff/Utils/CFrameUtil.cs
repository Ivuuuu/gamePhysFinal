//The CFrame class is a class that orginates from Roblox. I just like using this more than whatever Unity is doing.
//Modified to go with C# naming convention and use Unity classes.
//Original Author: EgoMoose

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A class that I use to easily represent position and rotation at once, and to easily modify it. I don't recommend using this compared to Unity's transform if you already know how to use it. 
/// This is much more complex, using a 4x4 matrix to represent rotation. On the plus side it's much easier to combine transformations, which is why I like to use it.
/// </summary>
[Serializable]
public class CFrame {
	private float m11 = 1, m12 = 0, m13 = 0, m14 = 0;
	private float m21 = 0, m22 = 1, m23 = 0, m24 = 0;
	private float m31 = 0, m32 = 0, m33 = 1, m34 = 0;
	private const float m41 = 0, m42 = 0, m43 = 0, m44 = 1;

	public readonly float x = 0, y = 0, z = 0;
	public Vector3 p = new Vector3(0, 0, 0);
	public readonly Vector3 lookVector = new Vector3(0, 0, -1);
	public readonly Vector3 rightVector = new Vector3(1, 0, 0);
	public readonly Vector3 upVector = new Vector3(0, 1, 0);

	private static Vector3 RIGHT = new Vector3(1, 0, 0);
	private static Vector3 UP = new Vector3(0, 1, 0);
	private static Vector3 BACK = new Vector3(0, 0, 1);

	// constructors

	/// <summary>
	/// Creates a CFrame from a Vector3.
	/// </summary>
	/// <param name="pos">The position of the CFrame.</param>
	public CFrame(Vector3 pos) {
		m14 = pos.x;
		m24 = pos.y;
		m34 = pos.z;
		x = m14; y = m24; z = m34;
		p = new Vector3(m14, m24, m34);
		lookVector = new Vector3(-m13, -m23, -m33);
		rightVector = new Vector3(m11, m21, m31);
		upVector = new Vector3(m12, m22, m32);
	}

	/// <summary>
	/// Creates a new CFrame located at pos and facing towards lookAt, assuming that (0, 1, 0) is considered “up”.
	/// At high pitch angles (around 82 degrees), you may experience numerical instability. If this is an issue, or if you require a different up vector, it’s recommended you use CFrame.fromMatrix instead to more accurately construct the CFrame. Additionally, if lookAt is directly above pos (pitch angle of 90 degrees) the up vector switches to the X-axis.
	/// </summary>
	/// <param name="pos">The position of the CFrame.</param>
	/// <param name="look">Where the CFrame will look at.</param>
	public CFrame(Vector3 eye, Vector3 look) {
		Vector3 zAxis = (eye - look).normalized;
		Vector3 xAxis = Vector3.Cross(UP, zAxis);
		Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
		if (xAxis.magnitude == 0) {
			if (zAxis.y < 0) {
				xAxis = new Vector3(0, 0, -1);
				yAxis = new Vector3(1, 0, 0);
				zAxis = new Vector3(0, -1, 0);
			} else {
				xAxis = new Vector3(0, 0, 1);
				yAxis = new Vector3(1, 0, 0);
				zAxis = new Vector3(0, 1, 0);
			}
		}
		m11 = xAxis.x; m12 = yAxis.x; m13 = zAxis.x; m14 = eye.x;
		m21 = xAxis.y; m22 = yAxis.y; m23 = zAxis.y; m24 = eye.y;
		m31 = xAxis.z; m32 = yAxis.z; m33 = zAxis.z; m34 = eye.z;
		x = m14; y = m24; z = m34;
		p = new Vector3(m14, m24, m34);
		lookVector = new Vector3(-m13, -m23, -m33);
		rightVector = new Vector3(m11, m21, m31);
		upVector = new Vector3(m12, m22, m32);
	}

	public CFrame(float nx = 0, float ny = 0, float nz = 0) {
		m14 = nx;
		m24 = ny;
		m34 = nz;
		x = m14; y = m24; z = m34;
		p = new Vector3(m14, m24, m34);
		lookVector = new Vector3(-m13, -m23, -m33);
		rightVector = new Vector3(m11, m21, m31);
		upVector = new Vector3(m12, m22, m32);
	}

	/// <summary>
	/// Creates a CFrame from position (x, y, z) and quaternion (i, j, k, w)
	/// </summary>
	/// <param name="nx"></param>
	/// <param name="ny"></param>
	/// <param name="nz"></param>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="k"></param>
	/// <param name="w"></param>
	public CFrame(float nx, float ny, float nz, float i, float j, float k, float w) {
		m14 = nx;
		m24 = ny;
		m34 = nz;
		m11 = 1 - 2 * (float)Math.Pow(j, 2) - 2 * (float)Math.Pow(k, 2);
		m12 = 2 * (i * j - k * w);
		m13 = 2 * (i * k + j * w);
		m21 = 2 * (i * j + k * w);
		m22 = 1 - 2 * (float)Math.Pow(i, 2) - 2 * (float)Math.Pow(k, 2);
		m23 = 2 * (j * k - i * w);
		m31 = 2 * (i * k - j * w);
		m32 = 2 * (j * k + i * w);
		m33 = 1 - 2 * (float)Math.Pow(i, 2) - 2 * (float)Math.Pow(j, 2);
		x = m14; y = m24; z = m34;
		p = new Vector3(m14, m24, m34);
		lookVector = new Vector3(-m13, -m23, -m33);
		rightVector = new Vector3(m11, m21, m31);
		upVector = new Vector3(m12, m22, m32);
	}

	/// <summary>
	/// Creates a CFrame from position (x, y, z) and a Quaternion.
	/// </summary>
	/// <param name="nx"></param>
	/// <param name="ny"></param>
	/// <param name="nz"></param>
	/// <param name="quaternion"></param>
	public CFrame(float nx, float ny, float nz, Quaternion quaternion) : this(nx, ny, nz, quaternion.x, quaternion.y, quaternion.z, quaternion.w) {}

	/// <summary>
	/// Creates a CFrame from Unity's Transform.
	/// </summary>
	public CFrame(Transform obj) : this(obj.position.x, obj.position.y, obj.position.z, obj.rotation) { }


	/// <summary>
	/// Creates a CFrame from its components. Don't use this unless you know what you doing.
	/// </summary>
	/// <param name="n14"></param>
	/// <param name="n24"></param>
	/// <param name="n34"></param>
	/// <param name="n11"></param>
	/// <param name="n12"></param>
	/// <param name="n13"></param>
	/// <param name="n21"></param>
	/// <param name="n22"></param>
	/// <param name="n23"></param>
	/// <param name="n31"></param>
	/// <param name="n32"></param>
	/// <param name="n33"></param>
	public CFrame(float n14, float n24, float n34, float n11, float n12, float n13, float n21, float n22, float n23, float n31, float n32, float n33) {
		m14 = n14; m24 = n24; m34 = n34;
		m11 = n11; m12 = n12; m13 = n13;
		m21 = n21; m22 = n22; m23 = n23;
		m31 = n31; m32 = n32; m33 = n33;
		x = m14; y = m24; z = m34;
		p = new Vector3(m14, m24, m34);
		lookVector = new Vector3(-m13, -m23, -m33);
		rightVector = new Vector3(m11, m21, m31);
		upVector = new Vector3(m12, m22, m32);
	}

	// operator overloads

	/// <summary>
	/// Returns the CFrame translated in world space by the Vector3 using addition.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static CFrame operator +(CFrame a, Vector3 b) {
		float[] ac = a.Components();
		float x = ac[0], y = ac[1], z = ac[2], m11 = ac[3], m12 = ac[4], m13 = ac[5], m21 = ac[6], m22 = ac[7], m23 = ac[8], m31 = ac[9], m32 = ac[10], m33 = ac[11];
		return new CFrame(x + b.x, y + b.y, z + b.z, m11, m12, m13, m21, m22, m23, m31, m32, m33);
	}

	/// <summary>
	/// Returns the CFrame translated in world space by the Vector3 using subtraction.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static CFrame operator -(CFrame a, Vector3 b) {
		float[] ac = a.Components();
		float x = ac[0], y = ac[1], z = ac[2], m11 = ac[3], m12 = ac[4], m13 = ac[5], m21 = ac[6], m22 = ac[7], m23 = ac[8], m31 = ac[9], m32 = ac[10], m33 = ac[11];
		return new CFrame(x - b.x, y - b.y, z - b.z, m11, m12, m13, m21, m22, m23, m31, m32, m33);
	}

	/// <summary>
	/// Returns the Vector3 transformed from Object to World coordinates.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static Vector3 operator *(CFrame a, Vector3 b) {
		float[] ac = a.Components();
		float x = ac[0], y = ac[1], z = ac[2], m11 = ac[3], m12 = ac[4], m13 = ac[5], m21 = ac[6], m22 = ac[7], m23 = ac[8], m31 = ac[9], m32 = ac[10], m33 = ac[11];
		Vector3 right = new Vector3(m11, m21, m31);
		Vector3 up = new Vector3(m12, m22, m32);
		Vector3 back = new Vector3(m13, m23, m33);
		return a.p + b.x * right + b.y * up + b.z * back;
	}

	/// <summary>
	/// Returns the composition of two <c>CFrames</c>.
	/// Proceeding CFrames are offset in relative object space by preceding CFrames when multiplied together.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns>A newly constructed CFrame</returns>
	public static CFrame operator *(CFrame a, CFrame b) {
		float[] ac = a.Components();
		float[] bc = b.Components();
		float a14 = ac[0], a24 = ac[1], a34 = ac[2], a11 = ac[3], a12 = ac[4], a13 = ac[5], a21 = ac[6], a22 = ac[7], a23 = ac[8], a31 = ac[9], a32 = ac[10], a33 = ac[11];
		float b14 = bc[0], b24 = bc[1], b34 = bc[2], b11 = bc[3], b12 = bc[4], b13 = bc[5], b21 = bc[6], b22 = bc[7], b23 = bc[8], b31 = bc[9], b32 = bc[10], b33 = bc[11];
		float n11 = a11 * b11 + a12 * b21 + a13 * b31 + a14 * m41;
		float n12 = a11 * b12 + a12 * b22 + a13 * b32 + a14 * m42;
		float n13 = a11 * b13 + a12 * b23 + a13 * b33 + a14 * m43;
		float n14 = a11 * b14 + a12 * b24 + a13 * b34 + a14 * m44;
		float n21 = a21 * b11 + a22 * b21 + a23 * b31 + a24 * m41;
		float n22 = a21 * b12 + a22 * b22 + a23 * b32 + a24 * m42;
		float n23 = a21 * b13 + a22 * b23 + a23 * b33 + a24 * m43;
		float n24 = a21 * b14 + a22 * b24 + a23 * b34 + a24 * m44;
		float n31 = a31 * b11 + a32 * b21 + a33 * b31 + a34 * m41;
		float n32 = a31 * b12 + a32 * b22 + a33 * b32 + a34 * m42;
		float n33 = a31 * b13 + a32 * b23 + a33 * b33 + a34 * m43;
		float n34 = a31 * b14 + a32 * b24 + a33 * b34 + a34 * m44;
		float n41 = m41 * b11 + m42 * b21 + m43 * b31 + m44 * m41;
		float n42 = m41 * b12 + m42 * b22 + m43 * b32 + m44 * m42;
		float n43 = m41 * b13 + m42 * b23 + m43 * b33 + m44 * m43;
		float n44 = m41 * b14 + m42 * b24 + m43 * b34 + m44 * m44;
		return new CFrame(n14, n24, n34, n11, n12, n13, n21, n22, n23, n31, n32, n33);
	}

	public override string ToString() {
		return String.Join(", ", Components());
	}

	// private static functions

	private static Vector3 vectorAxisAngle(Vector3 n, Vector3 v, float t) {
		n = n.normalized;
		return v * (float)Math.Cos(t) + Vector3.Dot(v, n) * n * (1 - (float)Math.Cos(t)) + Vector3.Cross(n, v) * (float)Math.Sin(t);
	}

	private static float getDeterminant(CFrame a) {
		float[] ac = a.Components();
		float a14 = ac[0], a24 = ac[1], a34 = ac[2], a11 = ac[3], a12 = ac[4], a13 = ac[5], a21 = ac[6], a22 = ac[7], a23 = ac[8], a31 = ac[9], a32 = ac[10], a33 = ac[11];
		float det = (a11 * a22 * a33 * m44 + a11 * a23 * a34 * m42 + a11 * a24 * a32 * m43
				+ a12 * a21 * a34 * m43 + a12 * a23 * a31 * m44 + a12 * a24 * a33 * m41
				+ a13 * a21 * a32 * m44 + a13 * a22 * a34 * m41 + a13 * a24 * a31 * m42
				+ a14 * a21 * a33 * m42 + a14 * a22 * a31 * m43 + a14 * a23 * a32 * m41
				- a11 * a22 * a34 * m43 - a11 * a23 * a32 * m44 - a11 * a24 * a33 * m42
				- a12 * a21 * a33 * m44 - a12 * a23 * a34 * m41 - a12 * a24 * a31 * m43
				- a13 * a21 * a34 * m42 - a13 * a22 * a31 * m44 - a13 * a24 * a32 * m41
				- a14 * a21 * a32 * m43 - a14 * a22 * a33 * m41 - a14 * a23 * a31 * m42);
		return det;
	}

	private static CFrame invert4x4(CFrame a) {
		float[] ac = a.Components();
		float a14 = ac[0], a24 = ac[1], a34 = ac[2], a11 = ac[3], a12 = ac[4], a13 = ac[5], a21 = ac[6], a22 = ac[7], a23 = ac[8], a31 = ac[9], a32 = ac[10], a33 = ac[11];
		float det = getDeterminant(a);
		if (det == 0) { return a; }
		float b11 = (a22 * a33 * m44 + a23 * a34 * m42 + a24 * a32 * m43 - a22 * a34 * m43 - a23 * a32 * m44 - a24 * a33 * m42) / det;
		float b12 = (a12 * a34 * m43 + a13 * a32 * m44 + a14 * a33 * m42 - a12 * a33 * m44 - a13 * a34 * m42 - a14 * a32 * m43) / det;
		float b13 = (a12 * a23 * m44 + a13 * a24 * m42 + a14 * a22 * m43 - a12 * a24 * m43 - a13 * a22 * m44 - a14 * a23 * m42) / det;
		float b14 = (a12 * a24 * a33 + a13 * a22 * a34 + a14 * a23 * a32 - a12 * a23 * a34 - a13 * a24 * a32 - a14 * a22 * a33) / det;
		float b21 = (a21 * a34 * m43 + a23 * a31 * m44 + a24 * a33 * m41 - a21 * a33 * m44 - a23 * a34 * m41 - a24 * a31 * m43) / det;
		float b22 = (a11 * a33 * m44 + a13 * a34 * m41 + a14 * a31 * m43 - a11 * a34 * m43 - a13 * a31 * m44 - a14 * a33 * m41) / det;
		float b23 = (a11 * a24 * m43 + a13 * a21 * m44 + a14 * a23 * m41 - a11 * a23 * m44 - a13 * a24 * m41 - a14 * a21 * m43) / det;
		float b24 = (a11 * a23 * a34 + a13 * a24 * a31 + a14 * a21 * a33 - a11 * a24 * a33 - a13 * a21 * a34 - a14 * a23 * a31) / det;
		float b31 = (a21 * a32 * m44 + a22 * a34 * m41 + a24 * a31 * m42 - a21 * a34 * m42 - a22 * a31 * m44 - a24 * a32 * m41) / det;
		float b32 = (a11 * a34 * m42 + a12 * a31 * m44 + a14 * a32 * m41 - a11 * a32 * m44 - a12 * a34 * m41 - a14 * a31 * m42) / det;
		float b33 = (a11 * a22 * m44 + a12 * a24 * m41 + a14 * a21 * m42 - a11 * a24 * m42 - a12 * a21 * m44 - a14 * a22 * m41) / det;
		float b34 = (a11 * a24 * a32 + a12 * a21 * a34 + a14 * a22 * a31 - a11 * a22 * a34 - a12 * a24 * a31 - a14 * a21 * a32) / det;
		float b41 = (a21 * a33 * m42 + a22 * a31 * m43 + a23 * a32 * m41 - a21 * a32 * m43 - a22 * a33 * m41 - a23 * a31 * m42) / det;
		float b42 = (a11 * a32 * m43 + a12 * a33 * m41 + a13 * a31 * m42 - a11 * a33 * m42 - a12 * a31 * m43 - a13 * a32 * m41) / det;
		float b43 = (a11 * a23 * m42 + a12 * a21 * m43 + a13 * a22 * m41 - a11 * a22 * m43 - a12 * a23 * m41 - a13 * a21 * m42) / det;
		float b44 = (a11 * a22 * a33 + a12 * a23 * a31 + a13 * a21 * a32 - a11 * a23 * a32 - a12 * a21 * a33 - a13 * a22 * a31) / det;
		return new CFrame(b14, b24, b34, b11, b12, b13, b21, b22, b23, b31, b32, b33);
	}

	private static Quaternion quaternionFromCFrame(CFrame a) {
		float[] ac = a.Components();
		float mx = ac[0], my = ac[1], mz = ac[2], m11 = ac[3], m12 = ac[4], m13 = ac[5], m21 = ac[6], m22 = ac[7], m23 = ac[8], m31 = ac[9], m32 = ac[10], m33 = ac[11];
		float trace = m11 + m22 + m33;
		float w = 1, i = 0, j = 0, k = 0;
		if (trace > 0) {
			float s = (float)Math.Sqrt(1 + trace);
			float r = 0.5f / s;
			w = s * 0.5f; i = (m32 - m23) * r; j = (m13 - m31) * r; k = (m21 - m12) * r;
		} else {
			float big = Math.Max(Math.Max(m11, m22), m33);
			if (big == m11) {
				float s = (float)Math.Sqrt(1 + m11 - m22 - m33);
				float r = 0.5f / s;
				w = (m32 - m23) * r; i = 0.5f * s; j = (m21 + m12) * r; k = (m13 + m31) * r;
			} else if (big == m22) {
				float s = (float)Math.Sqrt(1 - m11 + m22 - m33);
				float r = 0.5f / s;
				w = (m13 - m31) * r; i = (m21 + m12) * r; j = 0.5f * s; k = (m32 + m23) * r;
			} else if (big == m33) {
				float s = (float)Math.Sqrt(1 - m11 - m22 + m33);
				float r = 0.5f / s;
				w = (m21 - m12) * r; i = (m13 + m31) * r; j = (m32 + m23) * r; k = 0.5f * s;
			}
		}
		return new Quaternion(i,j,k,w);
	}

	private static CFrame lerpinternal(CFrame a, CFrame b, float t) {
		CFrame cf = a.Inverse() * b;
		Quaternion q = quaternionFromCFrame(cf);
		float i = q[0], j = q[1], k = q[2], w = q[3];
		float theta = (float)Math.Acos(w) * 2;
		Vector3 v = new Vector3(i, j, k);
		Vector3 p = Vector3.Lerp(a.p, b.p, t);
		if (theta != 0) {
			CFrame r = a * FromAxisAngle(v, theta * t);
			return (r - r.p) + p;
		} else {
			return (a - a.p) + p;
		}
	}

	// public static functions

	/// <summary>
	/// Creates a rotated CFrame from a Unit Vector3 and a rotation in radians
	/// </summary>
	/// <param name="axis"></param>
	/// <param name="theta"></param>
	/// <returns></returns>
	public static CFrame FromAxisAngle(Vector3 axis, float theta) {
		Vector3 r = vectorAxisAngle(axis, RIGHT, theta);
		Vector3 u = vectorAxisAngle(axis, UP, theta);
		Vector3 b = vectorAxisAngle(axis, BACK, theta);
		return new CFrame(0, 0, 0, r.x, u.x, b.x, r.y, u.y, b.y, r.z, u.z, b.z);
	}

	/// <summary>
	/// Equivalent to fromEulerAnglesXYZ
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static CFrame Angles(float x, float y, float z) {
		CFrame cfx = FromAxisAngle(RIGHT, x);
		CFrame cfy = FromAxisAngle(UP, y);
		CFrame cfz = FromAxisAngle(BACK, z);
		return cfx * cfy * cfz;
	}

	/// <summary>
	/// Creates a rotated CFrame using angles (rx, ry, rz) in radians. Rotations are applied in Z, Y, X order.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static CFrame FromEulerAnglesXYZ(float x, float y, float z) {
		return Angles(x, y, z);
	}

	// methods

	/// <summary>
	/// Returns the inverse of this CFrame
	/// </summary>
	/// <returns></returns>
	public CFrame Inverse() {
		return invert4x4(this);
	}

	/// <summary>
	/// Returns a Quaternion constructed from this CFrame.
	/// </summary>
	/// <returns></returns>
	public Quaternion ToQuaternion() {
		return quaternionFromCFrame(this);
	}

	/// <summary>
	/// Returns a CFrame interpolated between this CFrame and the goal by the fraction alpha
	/// </summary>
	/// <param name="cf2"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public CFrame Lerp(CFrame cf2, float t) {
		return lerpinternal(this, cf2, t);
	}

	/// <summary>
	/// Returns a CFrame transformed from Object to World space. Equivalent to [CFrame * cf]
	/// </summary>
	/// <param name="cf2"></param>
	/// <returns></returns>
	public CFrame ToWorldSpace(CFrame cf2) {
		return this * cf2;
	}

	/// <summary>
	/// Returns a CFrame transformed from World to Object space. Equivalent to [CFrame:inverse() * cf]
	/// </summary>
	/// <param name="cf2"></param>
	/// <returns></returns>
	public CFrame ToObjectSpace(CFrame cf2) {
		return this.Inverse() * cf2;
	}

	/// <summary>
	/// Returns a Vector3 transformed from Object to World space. Equivalent to [CFrame * v3]
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector3 PointToWorldSpace(Vector3 v) {
		return this * v;
	}

	/// <summary>
	/// Returns a Vector3 transformed from World to Object space. Equivalent to [CFrame:inverse() * v3]
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector3 PointToObjectSpace(Vector3 v) {
		return this.Inverse() * v;
	}

	/// <summary>
	/// Returns a Vector3 rotated from Object to World space. Equivalent to [(CFrame - CFrame.p) *v3]
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector3 VectorToWorldSpace(Vector3 v) {
		return (this - this.p) * v;
	}

	/// <summary>
	/// Returns a Vector3 rotated from World to Object space. Equivalent to [(CFrame:inverse() - CFrame:inverse().p) * v3]
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector3 VectorToObjectSpace(Vector3 v) {
		return (this - this.p).Inverse() * v;
	}

	/// <summary>
	/// Returns the values: x, y, z, R00, R01, R02, R10, R11, R12, R20, R21, R22, where R00-R22 represent the 3x3 rotation matrix of the CFrame, and xyz represent the position of the CFrame.
	/// </summary>
	/// <returns></returns>
	public float[] Components() {
		return new float[] { m14, m24, m34, m11, m12, m13, m21, m22, m23, m31, m32, m33 };
	}

	/// <summary>
	/// Returns approximate angles that could be used to generate CFrame, if angles were applied in Z, Y, X order
	/// </summary>
	/// <returns></returns>
	public float[] ToEulerAnglesXYZ() {
		float x = (float)Math.Atan2(-m23, m33);
		float y = (float)Math.Asin(m13);
		float z = (float)Math.Atan2(-m12, m11);
		return new float[] { x, y, z };
	}
}