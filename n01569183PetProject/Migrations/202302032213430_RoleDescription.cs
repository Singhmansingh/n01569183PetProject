namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "RoleDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleDescription");
        }
    }
}
