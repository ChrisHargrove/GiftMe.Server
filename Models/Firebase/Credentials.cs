
using Newtonsoft.Json;

/// <summary>
/// Represents the firebase app credentials that are stored in the AppSettings.json for the application
/// FIXME: Need to move this into a kubernetes ConfigMap or something similar!
/// </summary>
public class FirebaseCredentials {
    [JsonProperty("type")]
    public required string Type { get; set; }
    [JsonProperty("project_id")]
    public required string ProjectId { get; set; }
    [JsonProperty("private_key_id")]
    public required string PrivateKeyId { get; set; }
    [JsonProperty("private_key")]
    public required string PrivateKey { get; set; }
    [JsonProperty("client_email")]
    public required string ClientEmail { get; set; }
    [JsonProperty("client_id")]
    public required string ClientId { get; set; }
    [JsonProperty("auth_uri")]
    public required string AuthUri { get; set; }
    [JsonProperty("token_uri")]
    public required string TokenUri { get; set; }
    [JsonProperty("auth_provider_x509_cert_url")]
    public required string AuthProviderCertUrl { get; set; }
    [JsonProperty("client_x509_cert_url")]
    public required string ClientCertUrl { get; set; }
    [JsonProperty("universe_domain")]
    public required string UniverseDomain { get; set; }
}