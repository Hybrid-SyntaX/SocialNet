using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNet.Models
{
    public class Post
    {
        [Key] public Guid Id { set; get; }
        [Required] public string Title { set; get; }
        [Required] public string Content { set; get; }

        [Required]
        public ApplicationUser OriginalPoster { set; get; }

        [Required] public int Score { set; get; }

        [Required] public DateTime Created { set; get; }
        [Required] public DateTime LastUpdated { set; get; }

    }
}
