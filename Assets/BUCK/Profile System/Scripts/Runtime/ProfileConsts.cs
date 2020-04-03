namespace BUCK.ProfileSystem
{
    /// <summary>
    /// Set of constraints used when saving and loading data related to Profiles and ProfileSystem.
    /// </summary>
    public static class ProfileConsts
    {
        /// <summary>
        /// Key used for saving and loading Profile Name.
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// Key used for saving and loading if the Profile has been Deleted.
        /// </summary>
        public const string Deleted = "Deleted";
        /// <summary>
        /// Key used for saving and loading the index of the current profile used for Single Player.
        /// </summary>
        public const string SingleSelected = "Single Selected";
        /// <summary>
        /// Key used for saving and loading the indexes of the current profiles used for Multiplayer.
        /// </summary>
        public const string MultiSelected = "Multi Selected ";
    }
}