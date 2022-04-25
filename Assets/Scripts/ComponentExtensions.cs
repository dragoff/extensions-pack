using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ExtensionsPack
{
	public static class ComponentExtensions
	{
		public static bool HasChildren(this Component component) => component.transform.childCount != 0;

		public static IEnumerable<Transform> GetChildren(this Component component)
		{
			foreach (Transform t in component.transform)
				yield return t;
		}

		public static IEnumerable<RectTransform> GetChildrenRect(this Component component)
		{
			foreach (RectTransform t in component.transform)
				yield return t;
		}

		public static IEnumerable<T> GetChildren<T>(this Component component) where T : Component
		{
			foreach (Transform t in component.transform)
			{
				var result = t.GetComponent<T>();
				if (result != null)
					yield return result;
			}
		}

		public static void DestroyGameObject(this Component component, float delay = 0)
		{
			Object.Destroy(component.gameObject, delay);
		}

		public static void Destroy(this Component component, float delay = 0)
		{
			Object.Destroy(component, delay);
		}

		public static void DestroyChildren(this Component component)
		{
			foreach (Transform t in component.transform)
				Object.Destroy(t.gameObject);
		}

		public static void DestroyChildrenImmediate(this Component component)
		{
			foreach (Transform t in component.transform)
				Object.DestroyImmediate(t.gameObject);
		}

		public static void DestroyChildren<T>(this Component component) where T : Component
		{
			foreach (Transform t in component.transform)
				if (t.TryGetComponent(out T _))
					Object.Destroy(t.gameObject);
		}

		public static void DestroyChildrenExcept<T>(this Component component) where T : Component
		{
			foreach (Transform t in component.transform)
				if (t.TryGetComponent(out T _))
					Object.Destroy(t.gameObject);
		}

		public static void DestroyChildrenExcept(this Component component, Transform except)
		{
			foreach (Transform t in component.transform)
				if (t != except)
					Object.Destroy(t.gameObject);
		}

		public static void DestroyChildrenExcept(this Component component, params Transform[] except)
		{
			foreach (Transform t in component.transform)
				if (!except.Contains(t))
					Object.Destroy(t.gameObject);
		}

		public static void ForeachChildren<T>(this Component component, Action<T> action) where T : Transform
		{
			foreach (T child in component.transform)
				action(child);
		}

		public static void ForeachChildren<T>(this Component component, Action<T, int> action) where T : Transform
		{
			var i = 0;
			foreach (T child in component.transform)
				action(child, i++);
		}

		public static void ForeachChildrenExcept(this Component component, Transform except, Action<Transform> action)
		{
			foreach (Transform child in component.transform)
				if (child != except)
					action(child);
		}

		public static void SetChildrenActive(this Component component, bool active = true)
		{
			foreach (Transform t in component.transform)
				t.gameObject.SetActive(active);
		}

		public static T GetOrAddComponent<T>(this Component cmp) where T : Component
		{
			return GetOrAddComponent<T>(cmp.gameObject);
		}

		public static T GetOrAddComponent<T>(this GameObject go) where T : Component
		{
			return go.TryGetComponent(out T component) ? component : go.gameObject.AddComponent<T>();
		}
	}
}