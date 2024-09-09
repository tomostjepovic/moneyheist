
namespace MoneyHeist.Data.ErrorCodes
{
    public static class HeistErrors
    {
        public static readonly string HeistNotFound = "Heist not found";
        public static readonly string HeistStatusNotReady = "Heist status not ready";
        public static readonly string HeistHasStarted = "Heist already started";
        public static readonly string MemberNotFound = "Member not found";
        public static readonly string HeistNotFinished = "Heist not finished";
        public static readonly string HeistDoesntHaveAnySkill = "Heist doesnt have any skill";
        public static readonly string HeistDoesntHaveAnyMember = "Heist doesnt have any member";
        public static readonly string MemberOrMemberSkillNotFound = "Member or member skill not found";
        public static readonly string HeistNotInPlaning = "Heist status is not Planning";
        public static readonly string MembersAlreadyConfirmed = "Heist members already confirmed";
        public static readonly string HeistInPlaning = "Heist is in Planning status";
        public static readonly string HeistDoesntHaveEligibleMembers = "Heist doesnt have eligible members";
        public static readonly string MemberIsNotEligibleForThisHeist = "Member is not eligible for this heist";
        public static readonly string NoLevelUpTimeSettings = "Settings for level up not provided";
    }
}
