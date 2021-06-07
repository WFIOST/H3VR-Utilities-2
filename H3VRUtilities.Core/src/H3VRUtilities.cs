using H3VRUtilities.APIs;

namespace H3VRUtilities
{
    public static class H3VRUtilities
    {
        /// <summary>
        /// Provides an API for interacting with the player
        /// </summary>
        public static PlayerAPI Player { get; } = new PlayerAPI();
    }
}