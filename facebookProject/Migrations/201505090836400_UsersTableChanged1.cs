namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersTableChanged1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Event_Users", "event_id", "dbo.Events");
            DropIndex("dbo.Event_Users", new[] { "event_id" });
            AddColumn("dbo.Events", "id", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "user_id", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Events", new[] { "event_id" });
            AddPrimaryKey("dbo.Events", "id");
            AddForeignKey("dbo.Event_Users", "event_id", "dbo.Events", "id", cascadeDelete: true);
            CreateIndex("dbo.Event_Users", "event_id");
            DropColumn("dbo.Events", "event_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "event_id", c => c.Int(nullable: false));
            DropIndex("dbo.Event_Users", new[] { "event_id" });
            DropForeignKey("dbo.Event_Users", "event_id", "dbo.Events");
            DropPrimaryKey("dbo.Events", new[] { "id" });
            AddPrimaryKey("dbo.Events", "event_id");
            AlterColumn("dbo.Users", "user_id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Events", "id");
            CreateIndex("dbo.Event_Users", "event_id");
            AddForeignKey("dbo.Event_Users", "event_id", "dbo.Events", "event_id", cascadeDelete: true);
        }
    }
}
