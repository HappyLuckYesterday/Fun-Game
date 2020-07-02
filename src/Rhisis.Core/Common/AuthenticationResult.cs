namespace Rhisis.Core.Common
{
    public enum AuthenticationResult
    {
        Success,
        BadUsername,
        BadPassword,
        AccountSuspended,
        AccountTemporarySuspended,
        AccountDeleted
    }

    public class ItemConstants
    {
        public static readonly int WeaponArmonRefineMax = 10;
        public static readonly int JewleryRefineMax = 20;
        public static readonly int ElementRefineMax = 10;
    }
}
