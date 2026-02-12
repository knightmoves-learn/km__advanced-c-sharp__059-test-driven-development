using HomeEnergyApi.Services;

public class DecryptionAuditServiceTest
{
    private StubLogger<DecryptionAuditService> stubLogger;
    private DecryptionAuditService service;

    public DecryptionAuditServiceTest()
    {
        stubLogger = new StubLogger<DecryptionAuditService>();
        service = new DecryptionAuditService(stubLogger);
    }

    [Fact]
    public void ShouldLog_WhenValueDecrypted()
    {
        var cipherText = "testCipher";
        var plaintext = "testText";

        service.OnValueDecrypted(cipherText, plaintext);

        Assert.Equal($"{cipherText} has been decrypted to {plaintext}", stubLogger.LoggedInfoMessages[0]);
    }
}