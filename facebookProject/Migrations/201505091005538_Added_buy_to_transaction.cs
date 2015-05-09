namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_buy_to_transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "buy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "buy");
        }
    }
}
