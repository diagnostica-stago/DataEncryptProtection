namespace DataProtector.Model;

public interface IEncryptionOption
{
	string? Salt { get; }
}
