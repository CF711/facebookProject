namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedprimarykeygenerationforEvent : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "id");
            AddColumn("dbo.Events", "id", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "id");
            AddColumn("dbo.Events", "id", c => c.Int(nullable: false));
        }
    }
}
