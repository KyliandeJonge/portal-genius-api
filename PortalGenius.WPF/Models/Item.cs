using System;

namespace PortalGenius.WPF.Models
{
    public class Item
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public DateTime Created { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }
}
