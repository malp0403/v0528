using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using v0528.Extensions;

namespace v0528.Entities
{
    [Table("AppUser")]
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        //public int GetAge()
        //{
        //    return DateOfBirth.CalculateAge();
        //} 
        //public string GetPhotoUrl()
        //{
        //    if (Photos == null || Photos.Count == 0) return null;
        //    return Photos.FirstOrDefault(x => x.IsMain)?.Url;
        //}
    }
}
