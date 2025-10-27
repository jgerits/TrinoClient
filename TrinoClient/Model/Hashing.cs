namespace TrinoClient.Model
{
    public static class Hashing
    {
        private const int HashSeed = 17;
        private const int HashMultiplier = 23;

        // Optimized overloads to avoid params array allocation
        public static int Hash(object arg1)
        {
            unchecked
            {
                int hash = HashSeed;
                if (arg1 != null)
                {
                    hash = (hash * HashMultiplier) + arg1.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(object arg1, object arg2)
        {
            unchecked
            {
                int hash = HashSeed;
                if (arg1 != null)
                {
                    hash = (hash * HashMultiplier) + arg1.GetHashCode();
                }
                if (arg2 != null)
                {
                    hash = (hash * HashMultiplier) + arg2.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(object arg1, object arg2, object arg3)
        {
            unchecked
            {
                int hash = HashSeed;
                if (arg1 != null)
                {
                    hash = (hash * HashMultiplier) + arg1.GetHashCode();
                }
                if (arg2 != null)
                {
                    hash = (hash * HashMultiplier) + arg2.GetHashCode();
                }
                if (arg3 != null)
                {
                    hash = (hash * HashMultiplier) + arg3.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(params object[] args)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = HashSeed;

                foreach (object Item in args)
                {
                    if (Item != null)
                    {
                        hash = (hash * HashMultiplier) + Item.GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}
