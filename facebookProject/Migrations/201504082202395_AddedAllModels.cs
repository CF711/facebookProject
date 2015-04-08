namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.String(nullable: false, maxLength: 128),
                        fb_username = c.String(),
                        first_name = c.String(),
                        last_name = c.String(),
                    })
                .PrimaryKey(t => t.user_id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        event_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        eventstart = c.DateTime(nullable: false),
                        eventend = c.DateTime(nullable: false),
                        resource = c.String(),
                        allday = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.event_id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        note_id = c.Int(nullable: false, identity: true),
                        user_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.note_id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.Event_Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        event_id = c.Int(nullable: false),
                        user_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Events", t => t.event_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.event_id)
                .Index(t => t.user_id);
            
            AlterColumn("dbo.Transactions", "user_id", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Transactions", "user_id", "dbo.Users", "user_id");
            CreateIndex("dbo.Transactions", "user_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Event_Users", new[] { "user_id" });
            DropIndex("dbo.Event_Users", new[] { "event_id" });
            DropIndex("dbo.Notes", new[] { "user_id" });
            DropIndex("dbo.Transactions", new[] { "user_id" });
            DropForeignKey("dbo.Event_Users", "user_id", "dbo.Users");
            DropForeignKey("dbo.Event_Users", "event_id", "dbo.Events");
            DropForeignKey("dbo.Notes", "user_id", "dbo.Users");
            DropForeignKey("dbo.Transactions", "user_id", "dbo.Users");
            AlterColumn("dbo.Transactions", "user_id", c => c.String());
            DropTable("dbo.Event_Users");
            DropTable("dbo.Notes");
            DropTable("dbo.Events");
            DropTable("dbo.Users");
        }
    }
}
