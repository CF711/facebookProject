namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedprimarykeygenerationforEvent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "id", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "id", c => c.Int(nullable: false));
        }
    }
}
