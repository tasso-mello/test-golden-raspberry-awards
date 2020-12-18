﻿namespace common.raspberry.awards.Utilities
{
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Dynamic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml;

	public static class Messages
	{
		#region Constants

		private const int Keysize = 128;
		private const int DerivationIterations = 1000;

		#endregion

		#region Public Methods

		public static object GenerateGenericErrorMessage(string message)
		{
			return new { Error = new { Code = 400, Message = message } };
		}

		public static object GenerateGenericErrorMessage(string message, int statusCode)
		{
			return new { Error = new { Code = statusCode, Message = message } };
		}

		public static object GenerateGenericErrorMessage(string message, string innerException)
		{
			return new { Error = new { Code = 400, Message = message, Exception = innerException } };
		}

		public static object GenerateGenericNullErrorMessage(string obj, string message)
		{
			return new { Error = new { Code = 400, Object = obj, Message = message } };
		}

		public static object GenerateGenericSuccessCreatedMessage(string message)
		{
			return new { Code = 201, Message = message };
		}

		public static object GenerateGenericSuccessMessage(string message)
		{
			return new { Code = 200, Message = message };
		}

		public static object GenerateGenericSuccessObjectMessage(string propertyName, object objectResult, int statusCode)
		{
			var property = new ExpandoObject() as IDictionary<string, object>;
			property.Add("Code", statusCode);
			property.Add(propertyName, objectResult);

			return property;
		}

		public static object GenerateGenericSuccessObjectMessage(string propertyName, object objectResult, int statusCode, string message)
		{
			var property = new ExpandoObject() as IDictionary<string, object>;
			property.Add("Code", statusCode);
			property.Add("Message", message);
			property.Add(propertyName, objectResult);

			return property;
		}

		public static object GenerateGenericErrorObjectMessage(string propertyName, string objectResult)
		{
			var property = new ExpandoObject() as IDictionary<string, Object>;
			property.Add("Code", 400);
			property.Add(propertyName, objectResult);

			return property;
		}

		public static string GetEnumDescription(Enum enumType)
		{
			FieldInfo fi = enumType.GetType().GetField(enumType.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return enumType.ToString();
			}
		}

		public static T GetEnumByDescription<T>(string descriptionEnum)
		{
			var type = typeof(T);
			if (!type.IsEnum) throw new InvalidOperationException();
			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attribute != null)
				{
					if (attribute.Description.Trim().ToUpper().Equals(descriptionEnum.Trim().ToUpper()))
						return (T)field.GetValue(null);
				}
				else
				{
					if (field.Name.Trim().ToUpper().Equals(descriptionEnum.Trim().ToUpper()))
						return (T)field.GetValue(null);
				}
			}
			throw new ArgumentException("Não encontrado.", "description");
		}

		public static string GenerateNew256Token(string text)
		{
			return Encrypt(text, string.Empty);
		}

		public static string GerarMensagemRetorno(string origem, string codigo, string mensagem)
		{
			return JsonConvert.SerializeObject(new { Origem = origem, Codigo = codigo, Mensagem = mensagem });
		}

		public static string GerarXmlRequest(string valores)
		{
			string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
			var xml = new XmlDocument();
			xml.CreateElement("Entrada");

			var strXml = xml.ToString();

			if (strXml.StartsWith(_byteOrderMarkUtf8))
			{
				strXml = strXml.Remove(0, _byteOrderMarkUtf8.Length);
			}

			return strXml;
		}

		public static string Encrypt(this string plainText, string passPhrase)
		{
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var saltStringBytes = Generate256BitsOfRandomEntropy();
			var ivStringBytes = Generate256BitsOfRandomEntropy();
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
			{
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 128;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream())
						{
							using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
							{
								cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
								cryptoStream.FlushFinalBlock();
								// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
								var cipherTextBytes = saltStringBytes;
								cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
								cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
								memoryStream.Close();
								cryptoStream.Close();
								return Convert.ToBase64String(cipherTextBytes);
							}
						}
					}
				}
			}
		}

		public static string Decrypt(this string cipherText, string passPhrase)
		{
			// Get the complete stream of bytes that represent:
			// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
			// Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
			// Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
			// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
			{
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 128;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream(cipherTextBytes))
						{
							using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
							{
								var plainTextBytes = new byte[cipherTextBytes.Length];
								var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
								memoryStream.Close();
								cryptoStream.Close();
								return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
							}
						}
					}
				}
			}
		}

		#endregion

		#region Private Methods

		private static byte[] Generate256BitsOfRandomEntropy()
		{
			var randomBytes = new byte[16]; // 32 Bytes will give us 256 bits.
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}

		#endregion
	}
}
