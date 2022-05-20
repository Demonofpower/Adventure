using System.Collections.Generic;

namespace Adv.Server.Util
{
    public static class IdHelper
    {
        private static int newestId;
        private static HashSet<int> existingIds;

        static IdHelper()
        {
            existingIds = new HashSet<int>();
            newestId = 100;
        }

        public static int RequestUniqueId()
        {
            newestId += 1;
            existingIds.Add(newestId);
            return newestId;
        }
    }
}
