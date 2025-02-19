using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Admin : User
    {   
        [JsonIgnore]
        public ICollection<SubAdmin> SubAdmins { get; set; } = [];
    }
}
