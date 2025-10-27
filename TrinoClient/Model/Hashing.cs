namespace TrinoClient.Model
{
    public static class Hashing
    {
        // Optimized overloads to avoid params array allocation
        public static int Hash(object arg1)
        {
            unchecked
            {
                int hash = 17;
                if (arg1 != null)
                {
                    hash = (hash * 23) + arg1.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(object arg1, object arg2)
        {
            unchecked
            {
                int hash = 17;
                if (arg1 != null)
                {
                    hash = (hash * 23) + arg1.GetHashCode();
                }
                if (arg2 != null)
                {
                    hash = (hash * 23) + arg2.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(object arg1, object arg2, object arg3)
        {
            unchecked
            {
                int hash = 17;
                if (arg1 != null)
                {
                    hash = (hash * 23) + arg1.GetHashCode();
                }
                if (arg2 != null)
                {
                    hash = (hash * 23) + arg2.GetHashCode();
                }
                if (arg3 != null)
                {
                    hash = (hash * 23) + arg3.GetHashCode();
                }
                return hash;
            }
        }

        public static int Hash(params object[] args)
        {
            unchecked // Overflow is fine, just wrap
            {
                int Hash = 17;

                foreach (object Item in args)
                {
                    if (Item != null)
                    {
                        Hash = (Hash * 23) + Item.GetHashCode();
                    }
                }

                return Hash;
            }
        }
    }
}
