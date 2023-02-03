namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Players : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.Players",
               c => new
               {
                   PlayerId = c.Int(nullable: false, identity: true),
                   PlayerName = c.String(nullable: false),
                   PlayerPassword = c.String(nullable: false),
                   PlayerAlive = c.Boolean(nullable: false, defaultValue: false),
                   PlayerScore = c.Int(nullable: false, defaultValue: 0),
                   RoleId = c.Int(nullable: true),
               })
               .PrimaryKey(t => t.PlayerId)
               .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
               .Index(t => t.RoleId);
        }
        
        public override void Down()
        {
            DropTable("Players");
        }
    }
}
