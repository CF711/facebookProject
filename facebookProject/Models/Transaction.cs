using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace facebookProject.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int transaction_id {get; set;}
        
        public string user_id {get; set;}
        [ForeignKey("user_id")]
        public virtual User transaction_owner { get; set; }
        public string stock_id { get; set; }
        public decimal price { get; set; }
        public DateTime datetime { get; set; }
        public int amount { get; set; }
    }

    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string user_id { get; set; }
        public string fb_username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
    [Table("Events")]
    public class Event
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int event_id { get; set; }
        public string name { get; set; }
        public DateTime eventstart { get; set; }
        public DateTime eventend { get; set; }
        public string resource { get; set; }
        public bool allday { get; set; }

    }
    [Table("Notes")]
    public class Note
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int note_id { get; set; }
        public string user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User note_user { get; set; }
    }
    [Table("Event_Users")]
    public class Event_User
    {
        [Key]
        public int id { get; set; }
        public int event_id { get; set; }
        [ForeignKey("event_id")]
        public virtual Event calendar_event { get; set; }
        public string user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User calendar_user { get; set; }
    }
    public class NosebookContext : DbContext
    {
        public NosebookContext() : base("TransactionContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<NosebookContext>());
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Event_User> Event_Users { get; set; }
    }
}