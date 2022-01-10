using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }

        public List<Category> Category { get; set; } = new List<Category>();
    }
}