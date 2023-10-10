using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamScheduleSystem.Migrations
{
    public partial class DbInits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.ClassroomId);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                });

            migrationBuilder.CreateTable(
                name: "Proctorings",
                columns: table => new
                {
                    ProctoringId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProctoringName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamSlotId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Compensation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proctorings", x => x.ProctoringId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "StudentLists",
                columns: table => new
                {
                    StudentListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLists", x => x.StudentListId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MajorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                    table.ForeignKey(
                        name: "FK_Semesters_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                    table.ForeignKey(
                        name: "FK_User_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SemesterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentListId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudentLists",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseStudentListId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudentLists", x => new { x.CourseId, x.StudentListId });
                    table.ForeignKey(
                        name: "FK_CourseStudentLists_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudentLists_StudentLists_StudentListId",
                        column: x => x.StudentListId,
                        principalTable: "StudentLists",
                        principalColumn: "StudentListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSlots",
                columns: table => new
                {
                    ExamSlotId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamSlotName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProctoringId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSlots", x => x.ExamSlotId);
                    table.ForeignKey(
                        name: "FK_ExamSlots_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSchedules",
                columns: table => new
                {
                    ExamScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamSlotId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassroomId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedules", x => x.ExamScheduleId);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_ExamSlots_ExamSlotId",
                        column: x => x.ExamSlotId,
                        principalTable: "ExamSlots",
                        principalColumn: "ExamSlotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSlotProctorings",
                columns: table => new
                {
                    ExamSlotId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProctoringId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamSlotProctoringId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSlotProctorings", x => new { x.ExamSlotId, x.ProctoringId });
                    table.ForeignKey(
                        name: "FK_ExamSlotProctorings_ExamSlots_ExamSlotId",
                        column: x => x.ExamSlotId,
                        principalTable: "ExamSlots",
                        principalColumn: "ExamSlotId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSlotProctorings_Proctorings_ProctoringId",
                        column: x => x.ProctoringId,
                        principalTable: "Proctorings",
                        principalColumn: "ProctoringId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomExamSchedules",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassroomExamScheduleId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomExamSchedules", x => new { x.ExamScheduleId, x.ClassroomId });
                    table.ForeignKey(
                        name: "FK_ClassroomExamSchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomExamSchedules_ExamSchedules_ExamScheduleId",
                        column: x => x.ExamScheduleId,
                        principalTable: "ExamSchedules",
                        principalColumn: "ExamScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomExamSchedules_ClassroomId",
                table: "ClassroomExamSchedules",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentLists_StudentListId",
                table: "CourseStudentLists",
                column: "StudentListId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_ExamSlotId",
                table: "ExamSchedules",
                column: "ExamSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSlotProctorings_ProctoringId",
                table: "ExamSlotProctorings",
                column: "ProctoringId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSlots_CourseId",
                table: "ExamSlots",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_MajorId",
                table: "Semesters",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomExamSchedules");

            migrationBuilder.DropTable(
                name: "CourseStudentLists");

            migrationBuilder.DropTable(
                name: "ExamSlotProctorings");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "ExamSchedules");

            migrationBuilder.DropTable(
                name: "StudentLists");

            migrationBuilder.DropTable(
                name: "Proctorings");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ExamSlots");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Majors");
        }
    }
}
