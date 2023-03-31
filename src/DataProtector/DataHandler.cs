using System.Security.Cryptography;
using System.Text;
using ConsoleApp.Exceptions;
using DataProtector.Model;
using Microsoft.Extensions.Logging;

public class DataHandler
{
	private readonly ILogger _logger;
	private readonly byte[] _entropy;

	public DataHandler(ILogger<DataHandler> logger, IEncryptionOption options)
	{
		_logger = logger;
		_entropy = Encoding.UTF8.GetBytes(options.Salt ?? string.Empty);
	}

	public string Encrypt(string dataToEncrypt, DataProtectionScope scope = DataProtectionScope.CurrentUser)
	{
		_logger.LogDebug($"Data to encrypt: {dataToEncrypt}");

		var dataToEncryptBytes = UnicodeEncoding.UTF8.GetBytes(dataToEncrypt);
		var encryptedDataBytes = EncryptData(dataToEncryptBytes, _entropy, scope);
		var encryptedData = Convert.ToBase64String(encryptedDataBytes);

		var decryptedData = Decrypt(encryptedData, scope);
		if (!decryptedData.Equals(dataToEncrypt))
		{
			throw new DecryptionException();
		}

		return encryptedData;
	}

	public string Decrypt(string encryptedData, DataProtectionScope scope = DataProtectionScope.CurrentUser)
	{
		var encryptedDataBytes = Convert.FromBase64String(encryptedData);

		var decryptedDataBytes = DecryptData(encryptedDataBytes, _entropy, scope);
		return UnicodeEncoding.UTF8.GetString(decryptedDataBytes);
	}


	private static byte[] EncryptData(byte[] Buffer, byte[] Entropy, DataProtectionScope Scope)
	{
		if (Buffer == null)
			throw new ArgumentNullException(nameof(Buffer));

		if (Buffer.Length <= 0)
			throw new ArgumentException("The buffer length was 0.", nameof(Buffer));

		if (Entropy == null)
			throw new ArgumentNullException(nameof(Entropy));

		// Encrypt the data and store the result in a new byte array. The original data remains unchanged.
		return ProtectedData.Protect(Buffer, Entropy, Scope);
	}

	private static byte[] DecryptData(byte[] bytes, byte[] entropy, DataProtectionScope scope)
	{
		if (bytes == null)
			throw new ArgumentNullException(nameof(bytes));

		if (entropy == null)
			throw new ArgumentNullException(nameof(entropy));

		// Encrypt the data and store the result in a new byte array. The original data remains unchanged.
		return ProtectedData.Unprotect(bytes, entropy, scope);
	}
}