using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNet.Models
{
    public class Post
    {
        [Key] public Guid Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
    }
}
