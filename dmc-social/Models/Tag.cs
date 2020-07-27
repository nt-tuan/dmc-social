using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DmcSocial.Models
{
    public class Tag : BaseEntity
    {
        public string Value { get; set; }
        public bool IsSystemTag { get; set; } = false;
        public int PostCount { get; set; }
        public string NormalizeValue { get; set; }
        public Tag() { }
        public Tag(string tag)
        {
            Value = tag;
            IsSystemTag = true;
            NormalizeValue = "";
            if (string.IsNullOrEmpty(tag))
                return;
            NormalizeValue = Repositories.Helper.NormalizeString(tag);
        }
    }
}
