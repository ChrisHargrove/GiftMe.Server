namespace ServiceLayer.Services.Firebase;

/// <summary>
/// Represents the available OOB (Out Of Band) Operations
/// </summary>
public enum FirebaseOobType {
    OOB_REQ_TYPE_UNSPECIFIED,
    PASSWORD_RESET,
    [Obsolete]
    OLD_EMAIL_AGREE,
    [Obsolete]
    NEW_EMAIL_ACCEPT,
    VERIFY_EMAIL,
    [Obsolete]
    RECOVER_EMAIL,
    EMAIL_SIGNIN,
    VERIFY_AND_CHANGE_EMAIL,
    [Obsolete]
    REVERT_SECOND_FACTOR_ADDITION,
}