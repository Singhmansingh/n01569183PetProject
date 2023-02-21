namespace n01569183PetProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "RoleHasImg", c => c.Boolean(nullable: false,defaultValue: false));
            AddColumn("dbo.Roles", "RoleImgExt", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleImgExt");
            DropColumn("dbo.Roles", "RoleHasImg");
        }
    }
}
