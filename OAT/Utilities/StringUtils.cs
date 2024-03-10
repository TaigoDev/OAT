using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OAT.Utilities
{
	public static class StringUtils
	{
		public static string SerializeYML(this object obj) => new SerializerBuilder()
			.WithNamingConvention(CamelCaseNamingConvention.Instance).Build().Serialize(obj);

		public static T DeserializeYML<T>(this string yml) => new DeserializerBuilder()
			.WithNamingConvention(CamelCaseNamingConvention.Instance).Build().Deserialize<T>(yml);

		public static byte[] ReadFully(Stream input)
		{
			var buffer = new byte[16 * 1024];
			using var ms = new MemoryStream();
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				ms.Write(buffer, 0, read);
			return ms.ToArray();
		}

		public static string SHA226(string value)
		{
			var Sb = new StringBuilder();
			using var hash = SHA256.Create();
			byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
			foreach (byte b in result)
				Sb.Append(b.ToString("x2"));
			return Sb.ToString();
		}

		public static string RandomString(int length, bool without_digits = false)
		{
			string chars = !without_digits ? "qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890" : "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[new Random().Next(s.Length)]).ToArray());
		}

		public static string GetWords(this string text, int count)
			 => string.Join(" ", text.GetWordsLocal(count));

		public static string Base64Encode(string plainText) =>
			Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

		public static string Base64Decode(string base64EncodedData) =>
			Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));

		public static string ConvertStringToHex(string input)
		{
			var stringBytes = Encoding.UTF8.GetBytes(input);
			var sbBytes = new StringBuilder(stringBytes.Length * 2);
			foreach (byte b in stringBytes)
				sbBytes.AppendFormat("{0:X2}", b);
			return sbBytes.ToString();
		}

		public static string ConvertHexToString(string hexInput)
		{
			var numberChars = hexInput.Length;
			var bytes = new byte[numberChars / 2];
			for (int i = 0; i < numberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
			return Encoding.UTF8.GetString(bytes);
		}

		public static string[] GetWordsLocal(this string input, int count = -1, string[] wordDelimiter = null, StringSplitOptions options = StringSplitOptions.None)
		{
			if (string.IsNullOrEmpty(input))
				return new string[] { };

			if (count < 0)
				return input.Split(wordDelimiter, options);

			var words = input.Split(wordDelimiter, count + 1, options);
			if (words.Length <= count)
				return words;

			Array.Resize(ref words, words.Length - 1);
			return words;
		}

		public static string ConvertFullNameToShortName(string s)
		{
			if (string.IsNullOrEmpty(s = s.Trim()))
				return string.Empty;
			string[] sss = s.Split(" ");
			StringBuilder sb = new StringBuilder(sss[0] + " ");
			for (int i = 1; i < sss.Length; i++)
				if (!string.IsNullOrEmpty(s = sss[i].Trim()))
					sb.Append(s.Substring(0, 1).ToUpper() + ". ");
			return sb.ToString();
		}
	}
}
