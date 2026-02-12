using Microsoft.Extensions.Configuration;

public class ConfigHelper{
    public static string LookupSecret(string key)
    {
        var projectDir = Path.Combine(Directory.GetCurrentDirectory(), "../../../../HomeEnergyApi");
        var secretsFile = Path.Combine(projectDir, "secrets.json");

        var config = new ConfigurationBuilder()
            .SetBasePath(projectDir)
            .AddJsonFile(secretsFile)
            .Build();

        return config[key];
    }
}
