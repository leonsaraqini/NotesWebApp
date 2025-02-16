using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesWebApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public bool IsFavorite { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public string Color { get; set; } = "#ffffff";
        public virtual ApplicationUser User { get; set; }
    }
}