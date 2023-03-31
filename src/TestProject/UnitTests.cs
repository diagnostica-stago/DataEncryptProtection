using System.Security.Cryptography;
using DataProtector.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace TestProject;

public class UnitTests
{
	[TestCase("AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAQRhUQS729k6Zv4ypsDzw/AAAAAACAAAAAAAQZgAAAAEAACAAAACENEE5O7cRqNUlIR4z8xvhnfj/ArUogIaP4CEy9WnPbQAAAAAOgAAAAAIAACAAAABKHskqmiHoianFe89JPtwf8zHy66MnS328CnMRm19zLxAAAACwXMUOClpxhoeDN3OMQTFmQAAAAEUrRV5q9SvuJam8A1wubPfhd33nQgsfwXWbTh7atqkZB8kwkCPHFNoEQk8X3uZ/JBSyhxSdIfNsIQqaRe7NThg=")]
	public void FailToDecryptFromAnotherAccountTest(string encryptedData)
	{
		var options = new Mock<IEncryptionOption>();
		var dataHandler = new DataHandler(NullLogger<DataHandler>.Instance, options.Object);

		Assert.Throws<CryptographicException>(
			() => dataHandler.Decrypt(encryptedData),
			"The data should not be decrypted since it was encrypted with another account");
	}

	[Test]
	public void EncryptDecryptTest([Values] DataProtectionScope encryptScope, [Values] DataProtectionScope decryptScope)
	{
		var options = new Mock<IEncryptionOption>();
		var dataHandler = new DataHandler(NullLogger<DataHandler>.Instance, options.Object);
		
		var data = "Hello world";
		var encryptedData = dataHandler.Encrypt(data, encryptScope);
		Assert.That(encryptedData, Is.Not.Null);

		var decryptedData = dataHandler.Decrypt(encryptedData, decryptScope);
		Assert.That(decryptedData, Is.EqualTo(data));
	}
}