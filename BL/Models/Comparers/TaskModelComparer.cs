using Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BL.Models.Comparers
{
    public sealed class TaskModelComparer : IEqualityComparer<TaskModel>
    {
        public bool Equals([AllowNull] TaskModel x, [AllowNull] TaskModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return string.Equals(x.CronFormat, y.CronFormat, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.Description, y.Description, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.Url, y.Url, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.Email, y.Email, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] TaskModel obj)
        {
            if (obj == null) return 0;

            var hash = new HashCode();
            hash.Add(obj.Url);
            hash.Add(obj.CronFormat);
            hash.Add(obj.Name);
            hash.Add(obj.Description);
            hash.Add(obj.Email);

            return hash.ToHashCode();
        }
    }
}
