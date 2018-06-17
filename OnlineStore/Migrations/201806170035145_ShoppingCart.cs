namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        Paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "ShoppingCart_ID", c => c.Int());
            CreateIndex("dbo.Products", "ShoppingCart_ID");
            AddForeignKey("dbo.Products", "ShoppingCart_ID", "dbo.ShoppingCarts", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ShoppingCart_ID", "dbo.ShoppingCarts");
            DropIndex("dbo.Products", new[] { "ShoppingCart_ID" });
            DropColumn("dbo.Products", "ShoppingCart_ID");
            DropTable("dbo.ShoppingCarts");
        }
    }
}
