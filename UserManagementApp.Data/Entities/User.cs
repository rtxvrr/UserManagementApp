﻿namespace UserManagementApp.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
