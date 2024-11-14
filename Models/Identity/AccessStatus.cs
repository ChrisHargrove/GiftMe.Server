namespace Models.Identity;

/// <summary>
/// Represents the current allowed access of an account to the system.
/// </summary>
public enum AccessStatus {
    Pending,
    Denied,
    Blocked,
    Banned,
    Accepted,
}