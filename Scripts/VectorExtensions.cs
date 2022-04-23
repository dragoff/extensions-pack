using UnityEngine;

namespace ExtensionsPack
{
	public static class VectorExtensions
	{
		public static Vector2 xy(this Vector3 v) {
			return new Vector2(v.x, v.y);
		}

		public static Vector3 WithX(this Vector3 v, float x) {
			return new Vector3(x, v.y, v.z);
		}

		public static Vector3 WithY(this Vector3 v, float y) {
			return new Vector3(v.x, y, v.z);
		}

		public static Vector3 WithZ(this Vector3 v, float z) {
			return new Vector3(v.x, v.y, z);
		}

		public static Vector2 WithX(this Vector2 v, float x) {
			return new Vector2(x, v.y);
		}
	
		public static Vector2 WithY(this Vector2 v, float y) {
			return new Vector2(v.x, y);
		}
	
		public static Vector3 WithZ(this Vector2 v, float z) {
			return new Vector3(v.x, v.y, z);
		}
		
		public static Vector3 WithAddX(this Vector3 v, float x)
		{
			return new Vector3(v.x + x, v.y, v.z);
		}

		public static Vector3 WithAddY(this Vector3 v, float y)
		{
			return new Vector3(v.x, v.y + y, v.z);
		}

		public static Vector3 WithAddZ(this Vector3 v, float z)
		{
			return new Vector3(v.x, v.y, v.z + z);
		}
		public static Vector2 WithAddX(this Vector2 v, float x)
		{
			return new Vector2(v.x + x, v.y);
		}
		public static Vector2 WithAddY(this Vector2 v, float y)
		{
			return new Vector2(v.x, v.y + y);
		}
		
		// axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
		// point - the point to find nearest on line for
		public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
		{
			if (!isNormalized) axisDirection.Normalize();
			var d = Vector3.Dot(point, axisDirection);
			return axisDirection * d;
		}

		// lineDirection - unit vector in direction of line
		// pointOnLine - a point on the line (allowing us to define an actual line in space)
		// point - the point to find nearest on line for
		public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
		{
			if (!isNormalized) lineDirection.Normalize();
			var d = Vector3.Dot(point - pointOnLine, lineDirection);
			return pointOnLine + (lineDirection * d);
		}

		public static Vector3 Abs(this Vector3 vector)
		{
			return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

		public static Vector2 Abs(this Vector2 vector)
		{
			return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}

		public static Vector2 RandomInRange(this Vector2 vector, float range)
		{
			return new Vector2(vector.x + Random.Range(-range, range), vector.y + Random.Range(-range, range));
		}

		public static Vector2 RandomInRange(this Vector2 vector, float xRange, float yRange)
		{
			return new Vector2(vector.x + Random.Range(-xRange, xRange), vector.y + Random.Range(-yRange, yRange));
		}

		public static Vector3 RandomInRange(this Vector3 vector, float xRange, float yRange, float zRange)
		{
			return new Vector3(vector.x + Random.Range(-xRange, xRange), vector.y + Random.Range(-yRange, yRange),
				vector.z + Random.Range(-zRange, zRange));
		}

		public static Vector3 RandomInRange(this Vector3 vector, float xRange, float yRange)
		{
			return new Vector3(vector.x + Random.Range(-xRange, xRange), vector.y + Random.Range(-yRange, yRange), vector.z);
		}
	}
}