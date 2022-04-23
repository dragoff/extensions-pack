using System;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ExtensionsPack
{
	public static class StringExtensions
	{
		public static string Truncate(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		// Named format strings from object attributes. Eg:
		// string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
		// From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
		public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider = null)
		{
			StringBuilder sb = new StringBuilder();
			Type type = anObject.GetType();
			Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
			MatchCollection mc = reg.Matches(aFormat);
			int startIndex = 0;
			foreach (Match m in mc)
			{
				Group g = m.Groups[2]; //it's second in the match between { and }
				int length = g.Index - startIndex - 1;
				sb.Append(aFormat.Substring(startIndex, length));

				string toGet = string.Empty;
				string toFormat = string.Empty;
				int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
				if (formatIndex == -1) //no formatting, no worries
				{
					toGet = g.Value;
				}
				else //pickup the formatting
				{
					toGet = g.Value.Substring(0, formatIndex);
					toFormat = g.Value.Substring(formatIndex + 1);
				}

				//first try properties
				PropertyInfo retrievedProperty = type.GetProperty(toGet);
				Type retrievedType = null;
				object retrievedObject = null;
				if (retrievedProperty != null)
				{
					retrievedType = retrievedProperty.PropertyType;
					retrievedObject = retrievedProperty.GetValue(anObject, null);
				}
				else //try fields
				{
					FieldInfo retrievedField = type.GetField(toGet);
					if (retrievedField != null)
					{
						retrievedType = retrievedField.FieldType;
						retrievedObject = retrievedField.GetValue(anObject);
					}
				}

				if (retrievedType != null) //Cool, we found something
				{
					string result = string.Empty;
					if (toFormat == string.Empty) //no format info
					{
						result = retrievedType.InvokeMember("ToString",
							BindingFlags.Public | BindingFlags.NonPublic |
							BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
							, null, retrievedObject, null) as string;
					}
					else //format info
					{
						result = retrievedType.InvokeMember("ToString",
							BindingFlags.Public | BindingFlags.NonPublic |
							BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
							, null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
					}

					sb.Append(result);
				}
				else //didn't find a property with that name, so be gracious and put it back
				{
					sb.Append("{");
					sb.Append(g.Value);
					sb.Append("}");
				}

				startIndex = g.Index + g.Length + 1;
			}

			if (startIndex < aFormat.Length) //include the rest (end) of the string
			{
				sb.Append(aFormat.Substring(startIndex));
			}

			return sb.ToString();
		}

		public static string ToTitleCase(this string str)
			=> CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);

		public static DateTime ToDateTime(this string input) =>
			DateTime.TryParse(input, out var result)
				? result
				: new DateTime();

		public static DateTime ToUtcDateTime(this string input) =>
			DateTime.TryParse(input, CultureInfo.InvariantCulture,
				DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var result)
				? result
				: new DateTime();

		public static Color HexToColor(this string hex)
		{
			if (String.IsNullOrEmpty(hex))
			{
				Debug.LogError("Hex string is null");
				return Color.white;
			}

			hex = hex.TrimStart('#');

			if (hex.Length < 6 || hex.Length > 8)
			{
				Debug.LogError("Hex string has wrong format. Must be #FF00AA");
				return Color.white;
			}

			try
			{
				var r = Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
				var g = Byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
				var b = Byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

				if (hex.Length == 8)
				{
					var a = Byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
					return new Color32(r, g, b, a);
				}

				return new Color32(r, g, b, 255);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error parsing color. Used default {"Red".Red().Bold()} color. {e}");
			}

			return Color.red;
		}

		public static string ColorToHex(this Color32 color)
		{
			return $"{color.r:X2}{color.g:X2}{color.b:X2}";
		}

		public static string ColorToHex(this Color color)
		{
			return ColorToHex((Color32)color);
		}

		public static string ToUuid(this string input) => input.ToMd5Hash().Md5ToUuid();

		private static string Md5ToUuid(this string md5)
			=> $"{md5.Substring(0, 8)}-{md5.Substring(8, 4)}-{md5.Substring(12, 4)}-{md5.Substring(16, 4)}-{md5.Substring(20)}";

		public static string ToMd5Hash(this string input)
		{
			var hash = new StringBuilder();
			using (var md5Provider = new MD5CryptoServiceProvider())
			{
				var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

				foreach (var t in bytes)
					hash.Append(t.ToString("x2"));
			}

			return hash.ToString();
		}

		public static string ReplaceLastOccurrence(this string source, string find, string replace, bool ignoreCase = false)
		{
			var index = ignoreCase
				? source.LastIndexOf(find, StringComparison.InvariantCultureIgnoreCase)
				: source.LastIndexOf(find, StringComparison.Ordinal);

			return index == -1
				? source
				: source
					.Remove(index, find.Length)
					.Insert(index, replace);
		}

		public static bool Same(this string value, string other)
			=> value?.Equals(other, StringComparison.InvariantCultureIgnoreCase) ?? other == null;

		public static string Ellipsis(this string value, int maxChars)
			=> value.Length <= maxChars
				? value
				: $"{value.Substring(0, maxChars)}...";

		public static void CopyToClipboard(this string value)
		{
			var te = new TextEditor { text = value };
			te.SelectAll();
			te.Copy();
		}

		/// <summary>
		/// converts a string to dash separated lowercase
		/// "lineBreaking" will become "line-breaking"
		/// </summary>
		private static string ToDashedCase(this string input)
			=> string.IsNullOrEmpty(input)
				? input
				: (input[0] + Regex.Replace(input.Substring(1), "([A-Z])", "-$0", RegexOptions.Compiled))
				.ToLower();

		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf(toCheck, comp) >= 0;
		}
	}
}