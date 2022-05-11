using UnityEngine;

namespace ExtensionsPack
{
	public static class TransformExtensions
	{
		public static void CopyPositionFrom(this Transform transform, Transform other)
		{
			transform.position = other.position;
		}

		public static void CopyLocalPositionFrom(this Transform transform, Transform other)
		{
			transform.localPosition = other.localPosition;
		}

		public static void CopyRotationFrom(this Transform transform, Transform other)
		{
			transform.rotation = other.rotation;
		}

		public static void CopyLocalRotationFrom(this Transform transform, Transform other)
		{
			transform.localRotation = other.localRotation;
		}

		public static void CopyLocalScaleFrom(this Transform transform, Transform other)
		{
			transform.localScale = other.localScale;
		}

		public static void CopyAll(this Transform transform, Transform other)
		{
			transform.CopyPositionFrom(other);
			transform.CopyRotationFrom(other);
			transform.CopyLocalScaleFrom(other);
			transform.CopyLocalPositionFrom(other);
			transform.CopyLocalRotationFrom(other);
		}

		public static Transform ResetLocalPosition(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			return transform;
		}

		public static Transform ResetLocalRotation(this Transform transform)
		{
			transform.localRotation = Quaternion.identity;
			return transform;
		}

		public static Transform ResetLocalScale(this Transform transform)
		{
			transform.localScale = Vector3.zero;
			return transform;
		}

		public static Transform ResetPosition(this Transform transform)
		{
			transform.position = Vector3.zero;
			return transform;
		}

		public static Transform ResetRotation(this Transform transform)
		{
			transform.rotation = Quaternion.identity;
			return transform;
		}
	}
}