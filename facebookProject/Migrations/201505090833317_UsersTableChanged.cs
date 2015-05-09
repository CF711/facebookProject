namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersTableChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "event_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "event_id", c => c.Int(nullable: false, identity: true));
        }
    }
}
