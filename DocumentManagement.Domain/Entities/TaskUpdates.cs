//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DocumentManagement.Domain.Entities
//{
//    [Table("TaskUpdates")]
//    public class TaskUpdates
//    {
//        [Key]
//        public int Id { get; set; }

//        public int TaskId { get; set; }

//        public string UpdateDescription {  get; set; }

//        public DateTime UpdateTime { get; set; }

//        public int UpdateBy{ get; set; }

//        public int Progress {  get; set; }

//        public string Comment { get; set; }

//        public Users User { get; set; }

//        public Tasks task { get; set; }
//    }
//}
