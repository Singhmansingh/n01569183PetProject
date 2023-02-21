namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleMaxCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "RoleMaxCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleMaxCount");
        }
    }
}
