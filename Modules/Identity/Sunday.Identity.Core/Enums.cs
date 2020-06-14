namespace Sunday.Identity.Core
{
    public enum LoginStatus
    {
        Success,
        LockedOut,
        RequiresVerification,
        Failure,
        OrganizationInActive
    }
}
