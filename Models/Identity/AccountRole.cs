namespace Models.Identity;

/// <summary>
/// Represents the current role of an account, which will restrict access to certain endpoints.
/// </summary>
public enum AccountRole {
    User,
    Member,
    Admin,
    Owner,
}