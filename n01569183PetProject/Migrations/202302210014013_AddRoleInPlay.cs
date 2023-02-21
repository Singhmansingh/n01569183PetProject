namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleInPlay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "RoleInPlay", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleInPlay");
        }
    }
}
