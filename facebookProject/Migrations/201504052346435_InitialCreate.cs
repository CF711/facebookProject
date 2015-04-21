namespace facebookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        transaction_id = c.Int(nullable: false, identity: true),
                        user_id = c.String(),
                        stock_id = c.String(),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        datetime = c.DateTime(nullable: false),
                        amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.transaction_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
        }
    }
}
