namespace QuanLyHocSinhCap3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RoleID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Role", t => t.RoleID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleType = c.String(),
                        Test = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        ClassID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        GradeID = c.Int(nullable: false),
                        HeadTeacherID = c.Int(),
                        HeadTeacher_TeacherID = c.Int(),
                    })
                .PrimaryKey(t => t.ClassID)
                .ForeignKey("dbo.Grade", t => t.GradeID, cascadeDelete: true)
                .ForeignKey("dbo.Teacher", t => t.HeadTeacher_TeacherID)
                .Index(t => t.GradeID)
                .Index(t => t.HeadTeacher_TeacherID);
            
            CreateTable(
                "dbo.Grade",
                c => new
                    {
                        GradeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GradeID);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        TeacherID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Birthday = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherID);
            
            CreateTable(
                "dbo.TeachingAssignment",
                c => new
                    {
                        TeachingAssignmentID = c.Int(nullable: false, identity: true),
                        TeacherID = c.Int(nullable: false),
                        ClassID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeachingAssignmentID)
                .ForeignKey("dbo.Class", t => t.ClassID, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.SubjectID, cascadeDelete: true)
                .ForeignKey("dbo.Teacher", t => t.TeacherID, cascadeDelete: true)
                .Index(t => t.TeacherID)
                .Index(t => t.ClassID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.Subject",
                c => new
                    {
                        SubjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SubjectID);
            
            CreateTable(
                "dbo.Result",
                c => new
                    {
                        ResultID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        SemesterID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                        Average = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ResultID)
                .ForeignKey("dbo.Student", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Semester", t => t.SemesterID, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.SemesterID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.ResultDetail",
                c => new
                    {
                        ResultDetailID = c.Int(nullable: false, identity: true),
                        Mark = c.Double(nullable: false),
                        Month = c.Int(nullable: false),
                        ResultTypeID = c.Int(nullable: false),
                        ResultID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResultDetailID)
                .ForeignKey("dbo.Result", t => t.ResultID, cascadeDelete: true)
                .ForeignKey("dbo.ResultType", t => t.ResultTypeID, cascadeDelete: true)
                .Index(t => t.ResultTypeID)
                .Index(t => t.ResultID);
            
            CreateTable(
                "dbo.ResultType",
                c => new
                    {
                        ResultTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Coefficient = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResultTypeID);
            
            CreateTable(
                "dbo.Semester",
                c => new
                    {
                        SemesterID = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SemesterID);
            
            CreateTable(
                "dbo.Conduct",
                c => new
                    {
                        ConductID = c.Int(nullable: false, identity: true),
                        StundentID = c.Int(nullable: false),
                        SemesterID = c.Int(nullable: false),
                        Student_StudentID = c.Int(),
                    })
                .PrimaryKey(t => t.ConductID)
                .ForeignKey("dbo.Semester", t => t.SemesterID, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.Student_StudentID)
                .Index(t => t.SemesterID)
                .Index(t => t.Student_StudentID);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        Address = c.String(),
                        ClassID = c.Int(),
                    })
                .PrimaryKey(t => t.StudentID)
                .ForeignKey("dbo.Class", t => t.ClassID)
                .Index(t => t.ClassID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeachingAssignment", "TeacherID", "dbo.Teacher");
            DropForeignKey("dbo.TeachingAssignment", "SubjectID", "dbo.Subject");
            DropForeignKey("dbo.Result", "SubjectID", "dbo.Subject");
            DropForeignKey("dbo.Result", "SemesterID", "dbo.Semester");
            DropForeignKey("dbo.Result", "StudentID", "dbo.Student");
            DropForeignKey("dbo.Conduct", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.Student", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Conduct", "SemesterID", "dbo.Semester");
            DropForeignKey("dbo.ResultDetail", "ResultTypeID", "dbo.ResultType");
            DropForeignKey("dbo.ResultDetail", "ResultID", "dbo.Result");
            DropForeignKey("dbo.TeachingAssignment", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Class", "HeadTeacher_TeacherID", "dbo.Teacher");
            DropForeignKey("dbo.Class", "GradeID", "dbo.Grade");
            DropForeignKey("dbo.Account", "RoleID", "dbo.Role");
            DropIndex("dbo.Student", new[] { "ClassID" });
            DropIndex("dbo.Conduct", new[] { "Student_StudentID" });
            DropIndex("dbo.Conduct", new[] { "SemesterID" });
            DropIndex("dbo.ResultDetail", new[] { "ResultID" });
            DropIndex("dbo.ResultDetail", new[] { "ResultTypeID" });
            DropIndex("dbo.Result", new[] { "SubjectID" });
            DropIndex("dbo.Result", new[] { "SemesterID" });
            DropIndex("dbo.Result", new[] { "StudentID" });
            DropIndex("dbo.TeachingAssignment", new[] { "SubjectID" });
            DropIndex("dbo.TeachingAssignment", new[] { "ClassID" });
            DropIndex("dbo.TeachingAssignment", new[] { "TeacherID" });
            DropIndex("dbo.Class", new[] { "HeadTeacher_TeacherID" });
            DropIndex("dbo.Class", new[] { "GradeID" });
            DropIndex("dbo.Account", new[] { "RoleID" });
            DropTable("dbo.Student");
            DropTable("dbo.Conduct");
            DropTable("dbo.Semester");
            DropTable("dbo.ResultType");
            DropTable("dbo.ResultDetail");
            DropTable("dbo.Result");
            DropTable("dbo.Subject");
            DropTable("dbo.TeachingAssignment");
            DropTable("dbo.Teacher");
            DropTable("dbo.Grade");
            DropTable("dbo.Class");
            DropTable("dbo.Role");
            DropTable("dbo.Account");
        }
    }
}
