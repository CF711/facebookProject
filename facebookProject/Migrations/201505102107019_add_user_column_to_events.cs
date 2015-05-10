namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_user_column_to_events : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Event_Users", "event_id", "dbo.Events");
            DropForeignKey("dbo.Event_Users", "user_id", "dbo.Users");
            DropIndex("dbo.Event_Users", new[] { "event_id" });
            DropIndex("dbo.Event_Users", new[] { "user_id" });
            AddColumn("dbo.Events", "user_id", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Events", "user_id", "dbo.Users", "user_id");
            CreateIndex("dbo.Events", "user_id");
            DropTable("dbo.Event_Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Event_Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        event_id = c.Int(nullable: false),
                        user_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            
            DropIndex("dbo.Events", new[] { "user_id" });
            DropForeignKey("dbo.Events", "user_id", "dbo.Users");
            DropColumn("dbo.Events", "user_id");
            CreateIndex("dbo.Event_Users", "user_id");
            CreateIndex("dbo.Event_Users", "event_id");
            AddForeignKey("dbo.Event_Users", "user_id", "dbo.Users", "user_id");
            AddForeignKey("dbo.Event_Users", "event_id", "dbo.Events", "id", cascadeDelete: true);
        }
    }
}
