using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Admin : AppUser
    {   
        [JsonIgnore]
        public ICollection<SubAdmin> SubAdmins { get; set; } = [];
    }
}
