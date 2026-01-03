namespace ReadyHire.Models.Dto.CompanyProfileDto
{
    public class CompanyProfileDto
    {
        public int Id { get; set; } // معرف الشركة

        public string CompanyName { get; set; } = null!; // اسم الشركة

        public string ResponsiblePersonJobTitle { get; set; } = null!; // وظيفة المسؤول

        public string Industry { get; set; } = null!; // مجال الصناعة

        public string OfficialContactMethods { get; set; } = null!; // وسائل الاتصال الرسمية

        public string Location { get; set; } = null!; // عنوان الشركة

        public DateTime YearEstablished { get; set; } // سنة التأسيس

        public DateTime CreatedAt { get; set; } // وقت إنشاء البروفايل
        public string ApplicationUserId { get; set; }

        public string? CompanyImageUrl { get; set; }




    }
}
