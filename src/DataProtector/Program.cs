using System.Text;
using CommandLine;
using DataProtector.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Parser.Default.ParseArguments<EncryptionOption>(args)
	.WithParsed(opt =>
	{
		var services = new ServiceCollection();
		services.AddLogging(cfg =>
		{
			cfg.AddConsole();
		});
		services.AddSingleton<IEncryptionOption>(opt);
		services.AddTransient<DataHandler>();

		using var provider = services.BuildServiceProvider();
		var dataHandler = provider.GetRequiredService<DataHandler>();
		var encryptedData = dataHandler.Encrypt(opt.Data);

		var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
		logger.LogInformation(encryptedData);

		if (!string.IsNullOrEmpty(opt.FilePath))
		{
			var str = new StringBuilder();
			str.AppendLine($"# Date: {DateTime.UtcNow}");
			str.AppendLine($"# User: {Environment.UserName}");
			str.AppendLine($"# Salt: {opt.Salt}");
			str.AppendLine();
			str.AppendLine(encryptedData);

			var fileInfo = new FileInfo(opt.FilePath);
			if (fileInfo?.Directory != null && !fileInfo.Directory.Exists == false)
			{
				fileInfo.Directory.Create();
			}

			File.WriteAllText(opt.FilePath, str.ToString());
		}
	})
	.WithNotParsed(HandleParseError);

static void HandleParseError(IEnumerable<Error> errs)
{
	//handle errors
	Console.ReadLine();
}

internal class EncryptionOption : IEncryptionOption
{
	//[Option('d', "data", Required = true, HelpText = "The data to encrypt.")]
	[Value(0, Required = true, ResourceType = typeof(string), MetaName = "The data to encrypt.")]
	public string Data { get; set; } = default!;

	[Option("salt", Required = false, HelpText = "Encryption salt.")]
	public string? Salt { get; set; }

	[Option('o', "output", Required = false, HelpText = "File to write result.")]
	public string? FilePath { get; set; }
}